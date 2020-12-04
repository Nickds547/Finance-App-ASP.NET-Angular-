using Microsoft.EntityFrameworkCore;
using server.Entities;
using server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Services
{
    public class TransactionService
    {
        private readonly TransactionObjectContext _context;
        private readonly AnalyticsService _analyticsService;
        public TransactionService(TransactionObjectContext context)
        {
            _context = context;
            _analyticsService = new AnalyticsService();
        }


        public async Task<TransactionObject> getTransactionById(long id)
        {
            var transaction = await _context.TransactionObjects.FindAsync(id);

            return transaction;
        }

        public async Task<IEnumerable<TransactionObject>> GetTransactionsByUserId(int id)
        {
            var transactionList = await _context.TransactionObjects.Where(x => x.Id == id).ToListAsync();

            transactionList.Sort((x, y) => DateTime.Compare(y.Date, x.Date));

            return transactionList;
        }

        public async Task<bool> TransactionExists(long id)
        {
            return  _context.TransactionObjects.Any(e => e.Id == id);
        }


        public async Task<TransactionObject> AddTransaction(TransactionObject transaction)
        {
            var addedTrainsaction =  await _context.AddAsync(transaction);

            if (addedTrainsaction == null)
                return null;

            await _context.SaveChangesAsync();

            return transaction;
        }

        public async Task<(TransactionObject, string)> UpdateTransaction(long id, TransactionObject transaction)
        {
            var dbTransaction = await _context.TransactionObjects.FindAsync(id);

            //Checking if the id exists and if the id sent and the transaction id match
            if (id != transaction.TransactionId || dbTransaction == null)
                return (null, "Error validating User's association with Transaction");

            dbTransaction.Name = transaction.Name;
            dbTransaction.Amount = transaction.Amount;
            dbTransaction.Date = transaction.Date;
            dbTransaction.Type = transaction.Type;

            _context.Entry(dbTransaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TransactionExists(id))
                {
                    return (null, "Error saving Transaction");
                }
                else
                {
                    throw;
                }
            }


            return (dbTransaction, "Transaction Added successfully");
        }

        public async Task<TransactionObject> DeleteTransaction(long id, int userId)
        {
            var transaction = await _context.TransactionObjects.FindAsync(id);

            if (transaction == null || transaction.Id != userId)
                return null;

            _context.TransactionObjects.Remove(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        public async Task<Analytics> GetAllUserTransactionAnalytics(int userId)
        {
            List<TransactionObject> userTransactions = (List<TransactionObject>)await GetTransactionsByUserId(userId);
            Analytics analytics = _analyticsService.GetBaseAnalytics(userTransactions);

            return analytics;
        }

       
    }
}

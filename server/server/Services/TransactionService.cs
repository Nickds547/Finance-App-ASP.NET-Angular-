using Microsoft.EntityFrameworkCore;
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
        public TransactionService(TransactionObjectContext context)
        {
            _context = context;
        }


        public async Task<TransactionObject> getTransactionById(long id)
        {
            var transaction = await _context.TransactionObjects.FindAsync(id);

            return transaction;
        }

        public async Task<IEnumerable<TransactionObject>> GetTransactionsByUserId(int id)
        {
            return  await _context.TransactionObjects.Where(x => x.Id == id).ToListAsync();
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

        public async Task<TransactionObject> UpdateTransaction(long id, TransactionObject transaction)
        {
            var dbTransaction = await _context.TransactionObjects.FindAsync(id);

            //Checking if the id exists and if the id sent and the transaction id match
            if (id != transaction.Id || dbTransaction == null)
                return null;

            dbTransaction.Name = transaction.Name;
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
                    return null;
                }
                else
                {
                    throw;
                }
            }


            return dbTransaction;
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
    }
}

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


        public async Task<TransactionObject> getTransactionById(int id)
        {
            var transaction = await _context.TransactionObjects.FindAsync(id);

            return transaction;
        }

        public async Task<bool> TransactionExists(int id)
        {
            var transaction = await _context.TransactionObjects.FindAsync(id);

            if (transaction == null)
                return false;

            return true;
        }


        public async Task<TransactionObject> AddTransaction(TransactionObject transaction)
        {
            var addedTrainsaction =  await _context.AddAsync<TransactionObject>(transaction);

            if (addedTrainsaction == null)
                return null;

            await _context.SaveChangesAsync();

            return transaction;
        }

        public async Task<TransactionObject> UpdateTransaction(int id, TransactionObject transaction)
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
    }
}

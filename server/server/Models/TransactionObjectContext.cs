using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class TransactionObjectContext: DbContext
    {
        public TransactionObjectContext(DbContextOptions<TransactionObjectContext> options): base(options)
        {

        }

        public DbSet<TransactionObject> TransactionObjects { get; set; }
    }
}

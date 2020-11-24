using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace server.Models
{
    public class UserObjectContext: DbContext
    {
        public UserObjectContext(DbContextOptions<UserObjectContext> options): base(options)
        {

        }

        public DbSet<UserObject> UserObjects { get; set; }
    }
}

using System.Collections.Generic;
using System.Data.Entity;

namespace E_Commerce.Configuration
{
    public class OutboxDbContext : DbContext
    {
        public DbSet OutboxMessages { get; set; }
    }
}

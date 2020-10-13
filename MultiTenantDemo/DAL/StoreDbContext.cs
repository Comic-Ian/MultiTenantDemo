using Microsoft.EntityFrameworkCore;

namespace MultiTenantDemo.DAL
{
    public class StoreDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public StoreDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}

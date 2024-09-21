using Microsoft.EntityFrameworkCore;

namespace ApiNetCore8.Data
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> opt) : base(opt) 
        {

        }

        #region
        public DbSet<Laptop>? Laptops { get; set; }
        #endregion
    }
}

using Microsoft.EntityFrameworkCore;
using ApiNetCore8.Models;

namespace ApiNetCore8.Data
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> opt) : base(opt)
        {
        }

        #region DbSet
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        #endregion

        // Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                        .HasMany(c => c.Products)
                        .WithOne(p => p.Category)
                        .HasForeignKey(p => p.CategoryID);

            modelBuilder.Entity<Order>()
                        .HasMany(o => o.OrderDetails)
                        .WithOne(od => od.Order)
                        .HasForeignKey(od => od.OrderId);

            modelBuilder.Entity<OrderDetail>()
                        .HasOne(od => od.Product)
                        .WithMany(p => p.OrderDetails)
                        .HasForeignKey(od => od.ProductId);
        }
    }
}

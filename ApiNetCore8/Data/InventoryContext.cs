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
            // Thiết lập mối quan hệ 1-many giữa Category và Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasOne(p => p.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(p => p.CategoryID);
            });

            // Thiết lập mối quan hệ 1-1 giữa Category và Supplier
            modelBuilder.Entity<Category>()
                .HasOne(c => c.Supplier)
                .WithOne(s => s.Category)
                .HasForeignKey<Supplier>(s => s.CategoryId);

            // Thiết lập mối quan hệ 1-n giữa Order và OrderDetail
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasOne(od => od.Order)
                      .WithMany(o => o.OrderDetails)
                      .HasForeignKey(od => od.OrderId);
            });

        }
    }
}

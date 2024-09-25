using ApiNetCore8.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCore8.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Thiết lập quan hệ khóa ngoại
            modelBuilder.Entity<Supplier>()
                .HasOne(s => s.ProductCategory)
                .WithMany(pc => pc.Suppliers)
                .HasForeignKey(s => s.IdPC);
        }
    }

}

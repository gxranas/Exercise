using Microsoft.EntityFrameworkCore;
using Exercise.Models;

namespace Exercise.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<ShippingInfo> ShippingInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasOne(o => o.ShippingInfo)
                .WithMany()
                .HasForeignKey(o => o.ShippingInfoId)
                .IsRequired();

            modelBuilder.Entity<Order>()
           .Property(o => o.Id)
           .ValueGeneratedOnAdd();

            modelBuilder.Entity<Order>()
              .HasMany(o => o.Products)
              .WithOne(p => p.Order)
              .HasForeignKey(p => p.OrderID);

            modelBuilder.Entity<Products>()
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();
        }

    }
}

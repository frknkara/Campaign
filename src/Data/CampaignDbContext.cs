using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class CampaignDbContext : DbContext
    {
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<SystemConfig> SystemConfigs { get; set; }

        public CampaignDbContext(DbContextOptions<CampaignDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Campaign>()
                .HasOne<Product>(s => s.Product)
                .WithMany(g => g.Campaigns)
                .HasForeignKey(s => s.ProductId);

            modelBuilder.Entity<Order>()
                .HasOne<Product>(s => s.Product)
                .WithMany(g => g.Orders)
                .HasForeignKey(s => s.ProductId);

            modelBuilder.Entity<Product>()
                .HasIndex(s => s.Code)
                .IsUnique();

            modelBuilder.Entity<SystemConfig>()
                .HasIndex(s => s.Code)
                .IsUnique();
        }
    }
}

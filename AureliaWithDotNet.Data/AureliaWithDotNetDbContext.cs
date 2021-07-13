using AureliaWithDotNet.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AureliaWithDotNet.Data
{
    public partial class AureliaWithDotNetDbContext : DbContext
    {
        public AureliaWithDotNetDbContext()
        {
        }

        public AureliaWithDotNetDbContext(DbContextOptions<AureliaWithDotNetDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Asset> Assets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asset>(entity =>
            {
                entity.HasKey(e => e.Id)
                       .HasName("PK_Assets_Id");

                entity.Property(e => e.AssetName).HasMaxLength(255);

                entity.Property(e => e.Department);

                entity.Property(e => e.CountryOfDepartment).HasMaxLength(30);

                entity.Property(e => e.EMailAdressOfDepartment).HasMaxLength(150);

                entity.Property(e => e.Broken);

                entity.Property(e => e.PurchaseDate).HasColumnType("datetime");
            });
            
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

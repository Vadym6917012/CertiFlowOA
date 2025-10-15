using Domain.Entities;
using Domain.Entities.GoogleOAuth;
using Domain.Entities.JWT;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<JwtRefreshTokens> JwtTokens { get; set; }
        public DbSet<GoogleToken> GoogleTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
            .HasOne(u => u.Faculty)
            .WithMany(f => f.Users)
            .HasForeignKey(u => u.FacultyId);

            modelBuilder.Entity<Order>()
                .Property(o => o.Format)
                .HasConversion(
                    v => v.ToString(),
                    v => (DocumentFormat)Enum.Parse(typeof(DocumentFormat), v)
                )
                .HasMaxLength(20)
                .IsUnicode(false);

            modelBuilder.Entity<Document>()
                .Property(d => d.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (DocumentStatus)Enum.Parse(typeof(DocumentStatus), v)
                )
                .HasMaxLength(20)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Document)
                .WithMany(d => d.Orders)
                .HasForeignKey(o => o.DocumentId);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.GoogleToken)
                .WithOne(gt => gt.User)
                .HasForeignKey<GoogleToken>(gt => gt.UserId)
                .OnDelete(DeleteBehavior.Cascade); // If GoogleToken is deleted when ApplicationUser is deleted

            modelBuilder.Entity<GoogleToken>(entity =>
            {
                entity.HasIndex(t => t.UserId).IsUnique();
                entity.Property(t => t.AccessToken).IsRequired().HasMaxLength(2000);
            });

            // Налаштування для JwtRefreshTokens
            modelBuilder.Entity<JwtRefreshTokens>(entity =>
            {
                entity.HasIndex(t => t.Token).IsUnique();
                entity.HasIndex(t => t.UserId);
                entity.Property(t => t.Token).IsRequired().HasMaxLength(500);
            });
        }
    }
}

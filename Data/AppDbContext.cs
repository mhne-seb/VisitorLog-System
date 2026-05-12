using Microsoft.EntityFrameworkCore;
using VisitorLog.Models;

namespace VisitorLog.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Visitor> Visitors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Visitor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PurposeOfVisit).IsRequired().HasMaxLength(200);
                entity.Property(e => e.PersonToVisit).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ContactNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.DateTimeVisited).IsRequired();
            });
        }
    }
}

using EnterpriseApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Enterprise> Enterprises { get; set; }
        public DbSet<Code> Codes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relación Uno a Muchos entre Enterprise y Code
            modelBuilder.Entity<Code>()
                .HasOne(c => c.Owner)
                .WithMany(e => e.Codes)
                .HasForeignKey(c => c.OwnerId)
                .OnDelete(DeleteBehavior.Cascade); // Si se elimina una empresa, se eliminan sus códigos
        }
    }
}

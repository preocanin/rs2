using Microsoft.EntityFrameworkCore;

namespace rs2.Models.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Record> Records { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => x.UserId);

            modelBuilder.Entity<User>()
                        .HasAlternateKey(x => x.Email)
                        .HasName("Email_AlternateKey");

            modelBuilder.Entity<Record>().HasOne(x => x.User).WithMany(x => x.Records).OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}

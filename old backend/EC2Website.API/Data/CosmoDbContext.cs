using Microsoft.EntityFrameworkCore;

namespace EC2Website.API.Data
{
    public class CosmoDbContext : DbContext
    {
        public CosmoDbContext(DbContextOptions<CosmoDbContext> options) : base(options) { }

        public DbSet<Picture> Pictures { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map to the existing lowercase table in the public schema
            modelBuilder.Entity<Picture>().ToTable("pictures", "public");
        }
    }
}

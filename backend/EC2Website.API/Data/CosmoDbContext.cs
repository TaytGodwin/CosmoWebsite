using Microsoft.EntityFrameworkCore;

namespace EC2Website.API.Data
{
    public class CosmoDbContext : DbContext
    {
        public CosmoDbContext(DbContextOptions<CosmoDbContext> options)
            : base(options)
        {
        }

        public DbSet<Picture> Pictures { get; set; } = null!;
    }
}

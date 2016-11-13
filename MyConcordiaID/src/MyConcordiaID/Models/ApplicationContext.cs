using Microsoft.EntityFrameworkCore;

namespace MyConcordiaID.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options)
            : base(options) { }

        public DbSet<Application> Applications { get; set; }
    }
}

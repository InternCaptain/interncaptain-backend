using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    public class APIContext: DbContext
    {
        public APIContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Internship> Internships { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Application> Applications { get; set; }
    }
}
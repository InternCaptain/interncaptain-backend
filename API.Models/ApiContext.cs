using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    public class APIContext: DbContext
    {
        public APIContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
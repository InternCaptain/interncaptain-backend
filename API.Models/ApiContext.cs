using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    public class ApiContext: DbContext
    {
        public ApiContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
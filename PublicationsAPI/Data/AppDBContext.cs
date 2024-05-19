using Microsoft.EntityFrameworkCore;
using PublicationsAPI.Models;

namespace PublicationsAPI.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> DBContextOptions) : base(DBContextOptions)
        {
            
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Publications> Publications { get; set; }
    }
}
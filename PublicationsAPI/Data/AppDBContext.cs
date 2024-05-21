using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PublicationsAPI.Models;

namespace PublicationsAPI.Data
{
    public class AppDBContext : IdentityDbContext<Users, ApplicationRole, int>
    {
        public AppDBContext(DbContextOptions<AppDBContext> DBContextOptions) : base(DBContextOptions)
        {
            
        }

        //public DbSet<Users> Users { get; set; }
        public DbSet<Publications> Publications { get; set; }
    }

    public class ApplicationRole : IdentityRole<int>
    {
    }
}
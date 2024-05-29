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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            List<ApplicationRole> roles = new List<ApplicationRole>
            {
                new ApplicationRole
                {
                    Id = 1,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },

                new ApplicationRole
                {
                    Id = 2,
                    Name = "User",
                    NormalizedName = "USER"
                }
            };

            builder.Entity<ApplicationRole>().HasData(roles);

            base.OnModelCreating(builder);
        }

        //public DbSet<Users> Users { get; set; }   // -> it is already on the ASP.NET Core Identity
        public DbSet<Publications> Publications { get; set; }
    }

    public class ApplicationRole : IdentityRole<int>
    {
    }
}
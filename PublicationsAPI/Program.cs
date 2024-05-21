using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PublicationsAPI.Data;
using PublicationsAPI.Interfaces;
using PublicationsAPI.Models;
using PublicationsAPI.Repositories;
using PublicationsAPI.Services.Users;
using System.Text;

namespace PublicationsAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            builder.Services.AddDbContext<AppDBContext>(options => {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            
            builder.Services.AddIdentity<Users, ApplicationRole>(options => {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 0;
            });

            builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = "";
                options.DefaultChallengeScheme = "";
                options.DefaultScheme = "";
                options.DefaultForbidScheme = "";
                options.DefaultSignInScheme = "";
                options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters{
                    ValidateIssuer = true,
                    ValidIssuer = ,
                    ValidateAudience = true,
                    ValidAudience = ,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
                    )
                };
            });

            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<IUsersServices, UsersServices>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

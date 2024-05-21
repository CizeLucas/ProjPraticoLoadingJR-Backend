using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PublicationsAPI.Data;
using PublicationsAPI.Interfaces;
using PublicationsAPI.Models;
using PublicationsAPI.Repositories;
using PublicationsAPI.Services;
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
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;

                options.User.RequireUniqueEmail = true;

                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                
                // Disable two-factor authentication
                options.Tokens.AuthenticatorIssuer = null;
                options.Tokens.AuthenticatorTokenProvider = null;
                options.Tokens.ChangePhoneNumberTokenProvider = null;
                options.Tokens.EmailConfirmationTokenProvider = null;
                options.Tokens.PasswordResetTokenProvider = null;
            }).AddEntityFrameworkStores<AppDBContext>().AddDefaultUI();

            builder.Services.Configure<Users>(options => {
                options.EmailConfirmed = true;
                options.PhoneNumberConfirmed = true;
                options.TwoFactorEnabled = false;
            });

            builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = 
                options.DefaultChallengeScheme = 
                options.DefaultScheme = 
                options.DefaultForbidScheme = 
                options.DefaultSignInScheme = 
                options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters{
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
                    ),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<IUsersServices, UsersServices>();
            builder.Services.AddScoped<IAccountsService, AccountsService>();
            builder.Services.AddScoped<ITokenService, TokenService>();

            
            var app = builder.Build();

            app.MapIdentityApi<Users>();

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

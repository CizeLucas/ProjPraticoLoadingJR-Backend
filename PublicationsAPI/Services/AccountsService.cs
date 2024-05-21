using PublicationsAPI.Interfaces;
using PublicationsAPI.DTO.AccountDto;
using PublicationsAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace PublicationsAPI.Services {
    public class AccountsService : IAccountsService
    {

        private readonly UserManager<Users> _userManager;

        public AccountsService(UserManager<Users> userManager)
        {
            _userManager = userManager;
        }
        public Users RegisterUser(RegisterDto registerDto){
            return new Users
                {
                    //User info:
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                    Uuid = Guid.NewGuid().ToString("N"), //creates and formats the GUID
                    CreatedAt = DateTime.UtcNow,
                    ImageUrl = string.Empty,
                    Name = string.Empty,
                    Bio = string.Empty,

                    //Account login info:
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                };
        }
    }
}
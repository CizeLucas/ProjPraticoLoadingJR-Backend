using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PublicationsAPI.Models;
using PublicationsAPI.DTO.AccountDto;
using PublicationsAPI.Interfaces;
using PublicationsAPI.DTO.Mappers;

namespace PublicationsAPI.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountsController : ControllerBase {
        
        private readonly UserManager<Users> _userManager;
        private readonly IAccountsService _accountServices;
        private readonly ITokenService _tokenService;
        public AccountsController(UserManager<Users> userManager, IAccountsService accountsService, ITokenService tokenService)
        {
            _userManager = userManager;
            _accountServices = accountsService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try {

                if(!ModelState.IsValid)
                    BadRequest(ModelState);

                Users user = _accountServices.RegisterUser(registerDto);

                var createUser = await _userManager.CreateAsync(user, registerDto.Password);

                if(createUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, "User");
                    if(roleResult.Succeeded)
                        return Ok(UsersDTOMappers.UsersToNewlyRegisteredUser(user, _tokenService.CreateToken(user), _tokenService.GetExpirationTimeInMinutes())); 
                    else
                        return StatusCode(500, roleResult.Errors);
                }
                else
                {
                    return StatusCode(500, createUser.Errors);
                }
                
            } catch(Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }
    }
}
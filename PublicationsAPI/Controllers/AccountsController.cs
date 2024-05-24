using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PublicationsAPI.Models;
using PublicationsAPI.DTO.AccountDto;
using PublicationsAPI.Interfaces;
using PublicationsAPI.DTO.Mappers;
using PublicationsAPI.DTO.UserDTOs;
using Microsoft.EntityFrameworkCore;

namespace PublicationsAPI.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountsController : ControllerBase {
        
        private readonly UserManager<Users> _userManager;
        private readonly IAccountsService _accountServices;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<Users> _signInManager;
        public AccountsController(UserManager<Users> userManager, IAccountsService accountsService, ITokenService tokenService, SignInManager<Users> signInManager)
        {
            _userManager = userManager;
            _accountServices = accountsService;
            _tokenService = tokenService;
            _signInManager = signInManager;
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
                    {
                        return Ok(
                            UsersDTOMappers.UsersToNewlyLoggedInUserResponse(user, _tokenService.CreateToken(user), _tokenService.GetExpirationTimeInMinutes())
                        ); 
                    }
                    else
                        return BadRequest(roleResult.Errors);
                }
                else
                {
                    return BadRequest(createUser.Errors);
                }
                
            } catch(Exception e)
            {
                return StatusCode(500, e.ToString());
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login ([FromBody] LoginDto loginDto) {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == loginDto.Email.ToLower());
            
            if(user == null)
                return NotFound("Email does not exist");

            var logInResult = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            
            if(!logInResult.Succeeded)
                return Unauthorized("Email and/or Password incorrect!");

            return Ok(
                UsersDTOMappers.UsersToNewlyLoggedInUserResponse(user, _tokenService.CreateToken(user), _tokenService.GetExpirationTimeInMinutes())
            );
        }
    }
}
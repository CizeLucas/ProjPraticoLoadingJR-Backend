using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicationsAPI.DTO.Mappers;
using PublicationsAPI.DTO.UserDTOs;
using PublicationsAPI.Extensions;
using PublicationsAPI.Interfaces;
using PublicationsAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using PublicationsAPI.DTO.AccountDto;
using Microsoft.AspNetCore.Identity;

namespace PublicationsAPI.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UsersController : ControllerBase
    {
        public readonly IUsersRepository _usersRepository;
        private readonly UserManager<Users> _userManager;
        public UsersController(IUsersRepository usersRepository, UserManager<Users> userManager)
        {
            _usersRepository = usersRepository;
            _userManager = userManager;
        }

        [HttpGet("{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LoggedOutUserResponse>> GetByUsername([FromQuery] string username) {

            var user = await _usersRepository.GetByUsernameAsync(username);

            if(user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("uuid")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LoggedOutUserResponse>> GetByUuid([FromQuery] string uuid) {

            if(String.IsNullOrEmpty(uuid) || uuid.Length != 32)
                return BadRequest("The UUID of the request is incorrect. It needs to have exactly 32 characters");

            LoggedOutUserResponse? user = await _usersRepository.GetByUuidAsync(uuid);
            if(user == null)
                return NotFound();
            
            return Ok(user);
        }

        [HttpGet("getall")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<LoggedOutUserResponse>>> GetAll() {
            var users = await _usersRepository.GetAllAsync();

            if(!users.Any()) 
                return NoContent();

            return Ok(users);
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LoggedInUserResponse>> GetUser() {
            
            string? uuid = User.GetUuid();
            
            if(string.IsNullOrEmpty(uuid))
                return Unauthorized("Um problema com o seu token JWT foi identificado"); 
            
            LoggedInUserResponse? user = await _usersRepository.GetUserAsync(uuid);

            if(user == null)
                return NotFound();
            
            return Ok(user);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LoggedInUserResponse>> AddUser([FromBody] UserRequest user){
            
            string? uuid = User.GetUuid();

            var result = await _usersRepository.AddUserAsync(user, uuid);

            if(result == null)
                return BadRequest("A problem occured while processing the request of adding a new user.");

            return CreatedAtAction(nameof(AddUser), new {uuid = result.Uuid}, result);
        }


        [Authorize]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LoggedInUserResponse>> UpdateUser([FromBody] UserRequest user)
        {
            
            string? uuid = User.GetUuid();

            if(string.IsNullOrEmpty(uuid))
                return Unauthorized("Um problema com o seu token JWT foi identificado");
            try{
                var result = await _usersRepository.UpdateUserAsync(user, uuid);
                if(result == null)
                    return BadRequest("");
                return Ok(result);

            } catch(Exception e) {
                return BadRequest(new { e.Message });
            }
        }

        [Authorize]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser() {
            string? uuid = User.GetUuid();

            bool operationState = await _usersRepository.DeleteUserAsync(uuid);

            if(operationState == false)
                return StatusCode(500);

            return NoContent();
        }
    }
}

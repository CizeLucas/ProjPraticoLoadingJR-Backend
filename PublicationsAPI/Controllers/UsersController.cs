using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using PublicationsAPI.DTO;
using PublicationsAPI.Interfaces;
using PublicationsAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace PublicationsAPI.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UsersController : ControllerBase
    {
        public readonly IUsersRepository _usersRepository;
        public UsersController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<List<Users>>> GetAll() {
            var users = await _usersRepository.GetAllAsync();

            if(!users.Any()) 
                return NoContent();

            return Ok(users);
        }

        [HttpGet("/checkemail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> checkEmailAvailability([FromQuery] string emailAddress){

            bool isValidEmail = new EmailAddressAttribute().IsValid(emailAddress) || string.IsNullOrEmpty(emailAddress);
            
            if(isValidEmail)
                return UnprocessableEntity();

            if(await _usersRepository.EmailExistsAsync(emailAddress))
                return Conflict();
            else
                return Ok();
        }

        [HttpGet("by-uuid/{uuid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByUuid([FromRoute] string uuid) {

            if(uuid.Length != 32)
                return BadRequest("The UUID of the request is incorrect. It needs to have exactly 32 characters");

            var user = await _usersRepository.GetByUuidAsync(uuid);
            if(user == null)
                return NotFound();
            
            return Ok(user);
        }


        [HttpGet("by-username/{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByUsername([FromRoute] string username) {

            var user = await _usersRepository.GetByUsernameAsync(username);

            if(user == null)
                return NotFound();

            return Ok(user);
        }


        [HttpPost("/cadastrar")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Users>> AddUser([FromBody] UsersDTO user){
            var result = await _usersRepository.AddUserAsync(user);

            user.CreatetAt = DateTime.Now;

            if(result == null)
                return BadRequest("A problem occured while processing the request of adding a new user.");

            return CreatedAtAction(nameof(AddUser), new {uuid = user.Uuid}, result);
        }

        //AUTHORIZE
        [HttpPut("/atualizar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser([FromBody] UsersDTO user, [FromQuery(Name = "uuid")] string uuid){
            
            var result = await _usersRepository.UpdateUserAsync(user, uuid);

            if(result == null)
                return BadRequest("");
            
            return Ok(await GetByUuid(uuid));
        }
        
        //AUTHORIZE
        [HttpPut("/password/change")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUserPassword([FromBody] string passwordHash, [FromQuery(Name = "user-uuid")] string uuid){
            
            var result = await _usersRepository.UpdateUserPasswordAsync(passwordHash, uuid);

            if(result == false)
                return StatusCode(500, "user password not changed due to an Internal Server Error");
            
            return Ok(await GetByUuid(uuid));
        }

        //AUTHORIZE
        [HttpDelete("/deletar")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser([FromQuery] string uuid) {
            bool operationState = await _usersRepository.DeleteUserAsync(uuid);

            if(operationState == false)
                return StatusCode(500);

            return NoContent();
        }
    }
}

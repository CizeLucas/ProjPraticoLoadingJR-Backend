using Microsoft.AspNetCore.Mvc;
using PublicationsAPI.DTO.Mappers;
using PublicationsAPI.DTO.UserDTOs;
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
        public async Task<ActionResult<List<LoggedOutUserResponse>>> GetAll() {
            var users = await _usersRepository.GetAllAsync();

            if(!users.Any()) 
                return NoContent();

            return Ok(users);
        }

        [HttpGet("by-uuid/{uuid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LoggedOutUserResponse>> GetByUuid([FromRoute] string uuid) {

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
        public async Task<ActionResult<LoggedOutUserResponse>> GetByUsername([FromRoute] string username) {

            var user = await _usersRepository.GetByUsernameAsync(username);

            if(user == null)
                return NotFound();

            return Ok(user);
        }

        //AUTHORIZE
        [HttpPost("/cadastrar")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LoggedInUserResponse>> AddUser([FromBody] UserRequest user){ //ActionResult<UserRequest>

            var result = await _usersRepository.AddUserAsync(user);

            if(result == null)
                return BadRequest("A problem occured while processing the request of adding a new user.");

            return CreatedAtAction(nameof(AddUser), new {uuid = result.Uuid}, result);
        }

        //AUTHORIZE
        [HttpPut("/atualizar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LoggedInUserResponse>> UpdateUser([FromBody] UserRequest user, [FromQuery(Name = "uuid")] string uuid){
            
            var result = await _usersRepository.UpdateUserAsync(user, uuid);

            if(result == null)
                return BadRequest("");
            
            return Ok(result);
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

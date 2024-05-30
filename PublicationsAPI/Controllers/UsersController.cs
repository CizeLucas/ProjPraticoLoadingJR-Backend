using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using PublicationsAPI.DTO.UserDTOs;
using PublicationsAPI.Extensions;
using PublicationsAPI.Interfaces;
using PublicationsAPI.Models;

namespace PublicationsAPI.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersServices;
        private readonly IImageService _imageService;
        public UsersController(IUsersService usersServices, IImageService imageService)
        {
            _usersServices = usersServices;
            _imageService = imageService;
        }

        [HttpGet("username/{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LoggedOutUserResponse>> GetByUsername([FromRoute] string username) {

            var user = await _usersServices.GetUserByUsernameService(username);

            if(user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("uuid/{uuid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LoggedOutUserResponse>> GetByUuid([FromRoute] string uuid) {

            if(string.IsNullOrEmpty(uuid) || uuid.Length != 32)
                return BadRequest("The UUID of the request is incorrect. It needs to have exactly 32 characters");

            var user = await _usersServices.GetUserByUuidService(uuid);
            if(user == null)
                return NotFound();
            
            return Ok(user);
        }

        [Authorize]
        [HttpGet("/where")]
        public async Task<ActionResult<LoggedInUserResponse>> GetUsersPaginated([FromQuery(Name = "p")] int page, [FromQuery(Name = "ps")] int pageSize)
        {
            return Ok(await _usersServices.GetUsersPaginatedService(page, pageSize));
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LoggedInUserResponse>> GetUser() {
            
            string? uuid = User.GetUuid();
            
            if(string.IsNullOrEmpty(uuid))
                return BadRequest("A problem with your JWT token was found, please login again."); 
            
            LoggedInUserResponse? user = await _usersServices.GetUserService(uuid);

            if(user == null)
                return NotFound();
            
            return Ok(user);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LoggedInUserResponse>> AddUser([FromForm] UserRequest user, [FromForm] ImageUploadModel image){
            
            string? uuid = User.GetUuid();

            var result = await _usersServices.AddUserService(user, uuid, image);

            if(result == null)
                return BadRequest("A problem occured while processing the request of adding a new user.");

            return CreatedAtAction(nameof(AddUser), new {uuid = result.Uuid}, result);
        }

        [Authorize]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LoggedInUserResponse>> UpdateUser([FromForm] UserRequest user, [FromForm] ImageUploadModel image)
        {
            
            string? uuid = User.GetUuid();

            if(string.IsNullOrEmpty(uuid))
                return BadRequest("A problem with your JWT token was found, please login again.");

            try{
                var result = await _usersServices.UpdateUserService(user, uuid, image);

                if(result == null)
                    return BadRequest("");

                return Ok(result);

            } catch(Exception e) {
                var error = e.ToString();
                return BadRequest(new { error });
            }
        }

        [Authorize]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser() {

            string? uuid = User.GetUuid();

            if(string.IsNullOrEmpty(uuid))
                return BadRequest();

            bool operationState = await _usersServices.DeleteUserService(uuid);

            if(operationState == false)
                return StatusCode(500);

            return NoContent();
        }
    }
}

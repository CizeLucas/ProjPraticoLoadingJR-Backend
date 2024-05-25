using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicationsAPI.DTO.Publication;
using PublicationsAPI.Extensions;
using PublicationsAPI.Interfaces;

namespace PublicationsAPI.Controllers
{
    [Route("api/publication")]
    [ApiController]
    public class PublicationsController : ControllerBase
    {

        private readonly IPublicationsService _publicationsService;
        public PublicationsController(IPublicationsService publicationsService)
        {
            _publicationsService = publicationsService;
        }
        
        [HttpGet("{publicationUuid}")]
        public async Task<ActionResult<PublicationResponseDTO>> GetPublication([FromRoute] string publicationUuid)
        {
            var publication = await _publicationsService.GetPublicationAsync(publicationUuid);

            if(publication == null)
                return NotFound();

            return Ok(publication);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<PublicationResponseDTO>> GetPublicationsFromUser()
        {
            string? userUuid = User.GetUuid();

            if(string.IsNullOrEmpty(userUuid))
                return Unauthorized();

            var publication = await _publicationsService.GetPublicationsFromUserAsync(userUuid);

            return Ok(publication);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<PublicationResponseDTO>> CreatePublication([FromBody] PublicationDTO publicationDto)
        {
            string? userUuid = User.GetUuid();

            if(string.IsNullOrEmpty(userUuid))
                return Unauthorized();

            var createdPublication = await _publicationsService.AddPublicationAsync(userUuid, publicationDto);

            return CreatedAtAction(nameof(CreatePublication), new {publicationUuid = createdPublication.Uuid}, createdPublication);
        }

        [Authorize]
        [HttpPut("{publicationUuid}")]
        public async Task<IActionResult> UpdatePublication([FromRoute] string publicationUuid, [FromBody] PublicationDTO publicationDto)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            string userUuid = User.GetUuid();

            if(string.IsNullOrEmpty(userUuid))
                return Unauthorized();
            
            var updatedPublication = await _publicationsService.UpdatePublicationAsync(publicationUuid, publicationDto, userUuid);

            if(updatedPublication == null)
                return StatusCode(500);

            return Ok(updatedPublication);
        }

        [Authorize]
        [HttpDelete("{publicationUuid}")]
        public async Task<IActionResult> DeletePublication([FromRoute] string publicationUuid)
        {
            string? userUuid = User.GetUuid();

            if(await _publicationsService.DeletePublicationAsync(publicationUuid, userUuid))
                return NoContent();
            else
                return StatusCode(500, "Publication not deleted because of a server error");
        }


    }
}
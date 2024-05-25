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
        
        [HttpGet("uuid")]
        public async Task<ActionResult<PublicationResponseDTO>> GetPublication([FromQuery] string publicationUuid)
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





    }
}
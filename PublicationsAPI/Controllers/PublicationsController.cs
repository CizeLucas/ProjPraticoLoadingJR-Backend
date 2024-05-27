using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicationsAPI.DTO.Publication;
using PublicationsAPI.Extensions;
using PublicationsAPI.Interfaces;
using PublicationsAPI.Models;

namespace PublicationsAPI.Controllers
{
    [Route("api/publication")]
    [ApiController]
    public class PublicationsController : ControllerBase
    {

        private readonly IPublicationsService _publicationsService;
        private readonly IImageService _imageService;
        public PublicationsController(IPublicationsService publicationsService, IImageService imageService)
        {
            _publicationsService = publicationsService;
            _imageService = imageService;
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
        [HttpGet("where")]
        public async Task<ActionResult<PublicationResponseDTO>> GetPublicationsPaginated([FromQuery(Name = "p")] int page, [FromQuery(Name = "ps")] int pageSize)
        {
            string? userUuid = User.GetUuid();

            if(string.IsNullOrEmpty(userUuid))
                return BadRequest();

            return Ok(await _publicationsService.GetPublicationsPaginatedAsync(userUuid, page, pageSize));
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
        public async Task<ActionResult<PublicationResponseDTO>> CreatePublication(
            [FromForm] PublicationDTO publicationDto, [FromForm] ImageUploadModel image
        ) {
            string? userUuid = User.GetUuid();

            if(string.IsNullOrEmpty(userUuid))
                return Unauthorized();

            var createdPublication = await _publicationsService.AddPublicationAsync(userUuid, publicationDto, image);

            return CreatedAtAction(nameof(CreatePublication), new {publicationUuid = createdPublication.Uuid}, createdPublication);
        }


        [Authorize]
        [HttpPut("{publicationUuid}")]
        public async Task<IActionResult> UpdatePublication(
            [FromRoute] string publicationUuid, [FromForm] PublicationDTO publicationDto, [FromForm] ImageUploadModel image
        ) {
            if(!ModelState.IsValid)
                return BadRequest();

            string userUuid = User.GetUuid();

            try {

                var updatedPublication = await _publicationsService.UpdatePublicationAsync(publicationUuid, publicationDto, image, userUuid);
                
                if(updatedPublication == null)
                    return StatusCode(500);

                return Ok(updatedPublication);
            
            } catch (UnauthorizedAccessException ex) {
                return Unauthorized(ex.Message);
            } catch (NullReferenceException ex) {
                return BadRequest(ex.Message);
            } catch (Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{publicationUuid}")]
        public async Task<IActionResult> DeletePublication([FromRoute] string publicationUuid)
        {
            string? userUuid = User.GetUuid();
            
            try{
                if(await _publicationsService.DeletePublicationAsync(publicationUuid, userUuid))
                    return NoContent();
                else
                    return StatusCode(500, "Publication not deleted because of a server error");
            } catch (UnauthorizedAccessException ex) {
                return Unauthorized(new {error = ex.Message });
            }
        }


    }
}
using Microsoft.AspNetCore.Mvc;
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

        

    }
}
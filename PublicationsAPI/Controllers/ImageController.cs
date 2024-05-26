using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PublicationsAPI.Interfaces;
using PublicationsAPI.Models;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PublicationsAPI.Controllers
{
    [Route("api/images")]
    [ApiController]
    public class ImageController : ControllerBase
    {
            private readonly IImageService _imageService;
        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }


        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] ImageUploadModel model)
        {
            var imageLink = await _imageService.UploadImageAsync(model);

            return Ok(imageLink);
        }


    }
}
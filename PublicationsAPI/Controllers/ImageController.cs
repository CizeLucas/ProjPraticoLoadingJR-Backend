using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public ImageController(IAmazonS3 s3Client, IConfiguration configuration)
        {
            _s3Client = s3Client;
            _bucketName = configuration["AWS:AwsS3Bucket"];
        }
        
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] ImageUploadModel model)
        {
            if (model.Image == null || model.Image.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var fileName = $"{Guid.NewGuid().ToString("N")}_{FormatFileName(model.Image.FileName)}";
            using (var newMemoryStream = new MemoryStream())
            {
                model.Image.CopyTo(newMemoryStream);

                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = newMemoryStream,
                    Key = fileName,
                    BucketName = _bucketName,
                    ContentType = model.Image.ContentType,
                    CannedACL = S3CannedACL.PublicRead // Defines the object as publicly visible
                };

                var fileTransferUtility = new TransferUtility(_s3Client);
                await fileTransferUtility.UploadAsync(uploadRequest);
            }

            //signs and returns an Image Url with expiration time
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = fileName,
                Expires = DateTime.Now.AddHours(1) // Defines the expiration time for the image URL
            };

            var fileUrl = _s3Client.GetPreSignedURL(request);

            return Ok(new { fileUrl });
        }

    /*    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage([FromForm] ImageUploadModel model)
    {
        if (model.Image == null || model.Image.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var fileName = FormatFileName(model.Image.FileName);
        using (var newMemoryStream = new MemoryStream())
        {
            model.Image.CopyTo(newMemoryStream);

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = newMemoryStream,
                Key = fileName,
                BucketName = _publicationsBucket,
                ContentType = model.Image.ContentType
            };

            var fileTransferUtility = new TransferUtility(_s3Client);
            await fileTransferUtility.UploadAsync(uploadRequest);
        }

        var fileUrl = GeneratePreSignedURL(fileName);

        // Save the file URL to your database here

        return Ok(new { fileUrl });
    }*/

        private string GeneratePreSignedURL(string fileName)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = fileName,
                Expires = DateTime.Now.AddHours(1) // Defina o tempo de expiração da URL
            };

            return _s3Client.GetPreSignedURL(request);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllBucketAsync()
        {
            var data = await _s3Client.ListBucketsAsync();
            var buckets = data.Buckets.Select(b => { return b.BucketName;});
            return Ok(buckets);
        }


        [HttpPost("create")]
        public async Task<IActionResult> CreateBucketAsync(string bucketName)
        {
            var bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);
            if (bucketExists) return BadRequest($"Bucket {bucketName} already exists.");
            await _s3Client.PutBucketAsync(bucketName);
            return Ok($"Bucket {bucketName} created.");
        }

        private string FormatFileName(string originalFileName)
        {
            // Remover caracteres especiais, exceto hífens, underlines e pontos
            var cleanedFileName = Regex.Replace(originalFileName, @"[^a-zA-Z0-9\-_\.]", "");

            // Substituir espaços por underlines
            cleanedFileName = cleanedFileName.Replace(" ", "_");

            // Adicionar um GUID ao nome do arquivo para garantir unicidade
            var uniqueFileName = $"{Guid.NewGuid()}_{cleanedFileName}";

            // Limitar o comprimento do nome do arquivo (opcional)
            if (uniqueFileName.Length > 255)
            {
                uniqueFileName = uniqueFileName.Substring(0, 255);
            }

            return uniqueFileName.ToLower();
        }


    }
}
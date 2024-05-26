using System.Text.RegularExpressions;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using PublicationsAPI.Interfaces;
using PublicationsAPI.Models;

namespace PublicationsAPI.Services {
    public class ImageService : IImageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public ImageService(IAmazonS3 s3Client, IConfiguration configuration)
        {
            _s3Client = s3Client;
            _bucketName = configuration["AWS:AwsS3Bucket"];
        }

        public async Task<string> UploadImageAsync(ImageUploadModel model)
        {
            if (model.Image == null || model.Image.Length == 0)
                throw new ArgumentException("ImageService Class: No image file was uploaded");

            var fileName = FormatFileName(model.Image.FileName);
            using (var newMemoryStream = new MemoryStream())
            {
                model.Image.CopyTo(newMemoryStream);

                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = newMemoryStream,
                    Key = fileName,
                    BucketName = _bucketName,
                    ContentType = model.Image.ContentType,
                    CannedACL = S3CannedACL.PublicRead // Define o objeto como publicamente acessível
                };

                var fileTransferUtility = new TransferUtility(_s3Client);
                await fileTransferUtility.UploadAsync(uploadRequest);
            }

            var fileUrl = $"https://{_bucketName}.s3.{_s3Client.Config.RegionEndpoint.SystemName}.amazonaws.com/{fileName}";

            return fileUrl;
        }

        public async Task<string> updateImage(string? oldImageUrl, ImageUploadModel newImage)
        {
            //Checks if the string is valid and if it is, then tries to delete the old image file
            if(!string.IsNullOrEmpty(oldImageUrl))
                try {
                    if( !await DeleteImageLink(oldImageUrl) )
                    {
                        if( !await DeleteImageLink(oldImageUrl) )
                            throw new Exception("ImageService Class: The old file could not be deleted");
                    }
                } catch (AmazonS3Exception ex) {
                    throw new Exception("ImageService Class: The old file could not be deleted", ex);
                }

            //Uploads the new image and returns the link of it
            return await UploadImageAsync(newImage);
        }

        public async Task<bool> DeleteImageLink(string? imageUrl)
        {
            string[] splitURL = imageUrl.Split("/");
            string fileName = splitURL[splitURL.Length - 1];

            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }

            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileName
                };

                await _s3Client.DeleteObjectAsync(deleteObjectRequest);

                return true;
            }
            catch (AmazonS3Exception ex)
            {
                throw new Exception($"ImageService Class: the Image was not upload correctly.", ex);
            }
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

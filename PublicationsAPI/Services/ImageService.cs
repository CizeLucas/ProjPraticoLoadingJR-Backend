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
            //Checks if the oldImageUrl string is valid and if it is, it then tries to delete the old image file
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

            //Checks if the new image is valid, IF IT IS, then uploads the new image, or, IF IT ISN'T, then returns an empty string
            if(newImage.Image == null)
                return "";

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
            // Removes special characters, except hyphens, underscores and periods from the image name
            var cleanedFileName = Regex.Replace(originalFileName, @"[^a-zA-Z0-9\-_\.]", "");

            // Replace spaces with underscores
            cleanedFileName = cleanedFileName.Replace(" ", "_");

            // Adds a GUID to the beggining of the file name to ensure uniqueness
            var uniqueFileName = $"{Guid.NewGuid()}_{cleanedFileName}";

            // Limits file name length
            if (uniqueFileName.Length > 255)
            {
                uniqueFileName = uniqueFileName.Substring(0, 255);
            }

            return uniqueFileName.ToLower();
        }

    }
}

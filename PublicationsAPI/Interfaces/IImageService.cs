using PublicationsAPI.Models;

namespace PublicationsAPI.Interfaces
{
    public interface IImageService
    {
        public Task<string> UploadImageAsync(ImageUploadModel model);

        public Task<bool> DeleteImageLink(string? imageUrl);

        public Task<string> updateImage(string oldImageUrl, ImageUploadModel newImage);
    }
}
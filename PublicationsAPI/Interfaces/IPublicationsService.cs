using PublicationsAPI.DTO.PublicationDTOs;
using PublicationsAPI.Models;

namespace PublicationsAPI.Interfaces
{
    public interface IPublicationsService
    {
        public Task<PublicationResponseDTO> GetPublicationAsync(string publicationUuid);
        public Task<IEnumerable<PublicationResponseDTO>> GetPublicationAsync();
        public Task<IEnumerable<PublicationResponseDTO>> GetPublicationsFromUserAsync(string publisherUuid);
        public Task<IEnumerable<PublicationResponseDTO>> GetPublicationsPaginatedAsync(string publisherUuid, int page, int pageSize);
        public Task<PublicationResponseDTO> AddPublicationAsync(string publisherUuid, PublicationDTO publicationDto, ImageUploadModel image);
        public Task<PublicationResponseDTO> UpdatePublicationAsync(string publicationUuid, PublicationDTO publicationDto, ImageUploadModel image, string authorUuid);
        public Task<PublicationResponseDTO> UpdatePublicationImageAsync(string publicationUuid, ImageUploadModel image);
        public Task<bool> DeletePublicationAsync(string publicationUuid, string authorUuid);
    }
}
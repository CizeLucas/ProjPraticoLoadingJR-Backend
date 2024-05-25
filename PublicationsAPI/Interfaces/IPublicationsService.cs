using PublicationsAPI.DTO.Publication;
using PublicationsAPI.Models;

namespace PublicationsAPI.Interfaces
{
    public interface IPublicationsService
    {


        //AUTHORIZED METHODS:
        public Task<PublicationResponseDTO> GetPublicationAsync(string publicationUuid);
        public Task<IEnumerable<PublicationResponseDTO>> GetPublicationsFromUserAsync(string publisherUuid);
        public Task<IEnumerable<PublicationResponseDTO>> GetPublicationsPaginatedAsync(string publisherUuid, int page, int pageSize);
        public Task<PublicationResponseDTO> AddPublicationAsync(string publisherUuid, PublicationDTO publicationDto);
        public Task<PublicationResponseDTO> UpdatePublicationAsync(string publicationUuid, PublicationDTO publicationDto);
        public Task<bool> DeletePublicationAsync(string publicationUuid);

        
    }
}
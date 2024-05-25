using PublicationsAPI.DTO.Mappers;
using PublicationsAPI.DTO.Publication;
using PublicationsAPI.Interfaces;
using PublicationsAPI.Models;
using PublicationsAPI.Helper;
using Microsoft.IdentityModel.Tokens;

namespace PublicationsAPI.Services {
    public class PublicationsService : IPublicationsService
    {

        private readonly IPublicationsRepository _publicationsRepository;
        private readonly IUsersServices _usersServices;
        public PublicationsService(IPublicationsRepository publicationsRepository, IUsersServices usersServices)
        {
            _publicationsRepository = publicationsRepository;
            _usersServices = usersServices;
        }

        public async Task<PublicationResponseDTO> AddPublicationAsync(string publisherUuid, PublicationDTO publicationDto)
        {
            Publications? publication = new Publications();
            
            publication.Title = publicationDto.Title;
            publication.Description = publicationDto.Description;
            publication.PublicationType = publicationDto.PublicationType;
            publication.ImageURL = publicationDto.ImageURL;
            publication.Uuid = UuidCreator.CreateUuid();
            publication.CreatedAt = DateTime.UtcNow;
            publication.UpdatedAt = DateTime.UtcNow;
            publication.AuthorUuid = publisherUuid;

            return (await _publicationsRepository.AddPublicationAsync(publication)).PublicationsToPublicationResponseDTO();
        }

        public async Task<bool> DeletePublicationAsync(string publicationUuid, string authorUuid)
        {
            if( !((await _publicationsRepository.GetPublicationAsync(publicationUuid)).AuthorUuid == authorUuid) )
            {
                return false;
                //throw unauthenticated exception
            }
            else 
                return await _publicationsRepository.DeletePublicationAsync(publicationUuid);
        }

        public async Task<PublicationResponseDTO> GetPublicationAsync(string publicationUuid)
        {
           return (await _publicationsRepository.GetPublicationAsync(publicationUuid)).PublicationsToPublicationResponseDTO();
        }

        public async Task<IEnumerable<PublicationResponseDTO>> GetPublicationsFromUserAsync(string publisherUuid)
        {   
            var userPublications = await _usersServices.GetPublicationsByUserUuidAsync(publisherUuid);
            return userPublications.Select(p => p.PublicationsToPublicationResponseDTO()).ToList();
        }

        public async Task<IEnumerable<PublicationResponseDTO>> GetPublicationsPaginatedAsync(string publisherUuid, int page, int pageSize)
        {

            var publications =  await _publicationsRepository.GetPublicationsPaginatedAsync(publisherUuid, page, pageSize);
			
            return publications.Select(publications => publications.PublicationsToPublicationResponseDTO());
        }

        public async Task<PublicationResponseDTO> UpdatePublicationAsync(string publicationUuid, PublicationDTO publicationDto, string authorUuid)
        {
            Publications publicationToUpdate = await _publicationsRepository.GetPublicationAsync(publicationUuid);

            if(publicationToUpdate == null)
                throw new NullReferenceException("Any Publication with this UUID was found");

            publicationToUpdate.Title = publicationDto.Title;
            publicationToUpdate.Description = publicationDto.Description;
            publicationToUpdate.ImageURL = publicationDto.ImageURL;
            publicationToUpdate.PublicationType = publicationDto.PublicationType;
            publicationToUpdate.UpdatedAt = DateTime.UtcNow;

            Publications? publicationUpdated = await _publicationsRepository.UpdatePublicationAsync(publicationToUpdate);

            if(publicationUpdated == null)
                throw new ArgumentNullException("An error occured with the update requested on this publication");

            return publicationUpdated.PublicationsToPublicationResponseDTO();
        }
    }
}
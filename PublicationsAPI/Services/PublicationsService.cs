using PublicationsAPI.DTO.Mappers;
using PublicationsAPI.DTO.Publication;
using PublicationsAPI.Interfaces;
using PublicationsAPI.Models;
using PublicationsAPI.Helper;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;

namespace PublicationsAPI.Services {
    public class PublicationsService : IPublicationsService
    {

        private readonly IPublicationsRepository _publicationsRepository;
        private readonly IUsersServices _usersServices;
        private readonly IImageService _imageService;
        public PublicationsService(IPublicationsRepository publicationsRepository, IUsersServices usersServices, IImageService imageService)
        {
            _publicationsRepository = publicationsRepository;
            _usersServices = usersServices;
            _imageService = imageService;
        }

        public async Task<PublicationResponseDTO> AddPublicationAsync(string publisherUuid, PublicationDTO publicationDto, ImageUploadModel image)
        {
            Publications? publication = new Publications();
            
            if(image.Image != null)
            {
                try{    
                    publication.ImageURL = await _imageService.UploadImageAsync(image);
                } catch (Exception ex) {
                    throw new Exception("PublicationService Class: a problem with the image update occoured.", ex);
                }
            }

            publication.Title = publicationDto.Title;
            publication.Description = publicationDto.Description;
            publication.PublicationType = PublicationTypes.getIntValueFromString(publicationDto.PublicationType);
            publication.Uuid = UuidCreator.CreateUuid();
            publication.CreatedAt = DateTime.UtcNow;
            publication.UpdatedAt = DateTime.UtcNow;
            publication.AuthorUuid = publisherUuid;

            return (await _publicationsRepository.AddPublicationAsync(publication)).PublicationsToPublicationResponseDTO();
        }

        public async Task<bool> DeletePublicationAsync(string publicationUuid, string authorUuid)
        {
            Publications? publicationToDelete = await _publicationsRepository.GetPublicationAsync(publicationUuid);
            if ( !(publicationToDelete.AuthorUuid == authorUuid) )
                throw new UnauthorizedAccessException("PublicationsService Class: You don't have permission to delete this publication because it is owned by other user!");
            else 
            {
                await _imageService.DeleteImageLink(publicationToDelete.ImageURL);
                return await _publicationsRepository.DeletePublicationAsync(publicationUuid);
            }
        }

        public async Task<PublicationResponseDTO> GetPublicationAsync(string publicationUuid)
        { 
            Publications? publication = await _publicationsRepository.GetPublicationAsync(publicationUuid);
            return publication.PublicationsToPublicationResponseDTO();
        }

        public async Task<IEnumerable<PublicationResponseDTO>> GetPublicationsFromUserAsync(string publisherUuid)
        {   
            var userPublications = await _usersServices.GetPublicationsByUserUuidService(publisherUuid);
            return userPublications.Select(p => p.PublicationsToPublicationResponseDTO()).ToList();
        }

        public async Task<IEnumerable<PublicationResponseDTO>> GetPublicationsPaginatedAsync(string publisherUuid, int page, int pageSize)
        {

            var publications =  await _publicationsRepository.GetPublicationsPaginatedAsync(publisherUuid, page, pageSize);
			
            return publications.Select(publications => publications.PublicationsToPublicationResponseDTO());
        }

        public async Task<PublicationResponseDTO> UpdatePublicationAsync(string publicationUuid, PublicationDTO publicationDto, ImageUploadModel image, string authorUuid)
        {
            Publications publicationToUpdate = await _publicationsRepository.GetPublicationAsync(publicationUuid);

            if(!(publicationToUpdate.AuthorUuid == authorUuid))
                throw new UnauthorizedAccessException("PublicationsService Class: You don't have permission to edit this publication because it is owned by other user!");

            if(publicationToUpdate == null)
                throw new NullReferenceException("PublicationsService Class: Any Publication with this UUID was found");
            
            string? oldImageURL = publicationToUpdate.ImageURL;

            if(image.Image != null)
            {
                try {
                    publicationToUpdate.ImageURL = await _imageService.updateImage(oldImageURL, image);
                } catch (Exception ex){
                    throw new Exception("PublicationService Class: a problem with the image update occoured.", ex);
                }
            }
            publicationToUpdate.Title = publicationDto.Title;
            publicationToUpdate.Description = publicationDto.Description;
            publicationToUpdate.PublicationType = PublicationTypes.getIntValueFromString(publicationDto.PublicationType);
            publicationToUpdate.UpdatedAt = DateTime.UtcNow;

            Publications? publicationUpdated = await _publicationsRepository.UpdatePublicationAsync(publicationToUpdate);

            if(publicationUpdated == null)
                throw new ArgumentNullException("An error occured with the update requested on this publication");

            return publicationUpdated.PublicationsToPublicationResponseDTO();
        }

        public async Task<PublicationResponseDTO> UpdatePublicationImageAsync(string publicationUuid, ImageUploadModel image)
        {
            Publications publicationToUpdate = await _publicationsRepository.GetPublicationAsync(publicationUuid);

            string? oldImageURL = publicationToUpdate.ImageURL;

            try {
                publicationToUpdate.ImageURL = await _imageService.updateImage(oldImageURL, image);
            } catch (Exception ex){
                throw ex;
            }

            publicationToUpdate.UpdatedAt = DateTime.UtcNow;

            return publicationToUpdate.PublicationsToPublicationResponseDTO();
        }
    
    }
}
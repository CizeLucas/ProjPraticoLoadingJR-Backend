using PublicationsAPI.DTO.Publication;
using PublicationsAPI.Helper;
using PublicationsAPI.Models;

namespace PublicationsAPI.DTO.Mappers
{
    public static class PublicationsDTOMappers
    {
        public static PublicationDTO PublicationsToPublicationDTO(this Publications publication)
        {
            return new PublicationDTO
            {
                Title = publication.Title,
                Description = publication.Description,
                PublicationType = PublicationTypes.getStringValueFromInt(publication.PublicationType),
            };
        }

        public static Publications PublicationDTOToPublications(this PublicationDTO publicationDTO)
        {
            return new Publications
            {
                Title = publicationDTO.Title,
                Description = publicationDTO.Description,
                PublicationType = PublicationTypes.getIntValueFromString(publicationDTO.PublicationType),
            };
        }

        public static PublicationResponseDTO PublicationsToPublicationResponseDTO(this Publications publication)
        {
            return new PublicationResponseDTO
            {
                Uuid = publication.Uuid,
                Title = publication.Title,
                Description = publication.Description,
                PublicationType = PublicationTypes.getStringValueFromInt(publication.PublicationType),
                ImageURL = publication.ImageURL,
                CreatedAt = publication.CreatedAt,
                UpdatedAt = publication.UpdatedAt
            };
        }

        public static Publications PublicationResponseDTOToPublications(this PublicationResponseDTO publicationRespDTO)
        {
            return new Publications
            {
                Uuid = publicationRespDTO.Uuid,
                Title = publicationRespDTO.Title,
                Description = publicationRespDTO.Description,
                PublicationType = PublicationTypes.getIntValueFromString(publicationRespDTO.PublicationType),
                ImageURL = publicationRespDTO.ImageURL,
                CreatedAt = publicationRespDTO.CreatedAt,
                UpdatedAt = publicationRespDTO.UpdatedAt
            };
        } 
    }
}
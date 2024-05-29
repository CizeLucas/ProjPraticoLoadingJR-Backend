using PublicationsAPI.DTO.PublicationDTOs;
using PublicationsAPI.Helper;
using PublicationsAPI.Models;

#pragma warning disable //disables warnings on this file

namespace PublicationsAPI.DTO.Mappers
{
    public static class PublicationsDTOMappers
    {
/*
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
*/
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
                UpdatedAt = publication.UpdatedAt,
                AuthorUuid = publication.AuthorUuid
            };
        }

    }
}
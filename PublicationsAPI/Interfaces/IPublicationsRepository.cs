using PublicationsAPI.Models;

namespace PublicationsAPI.Interfaces
{
    public interface IPublicationsRepository
    {
        public Task<IEnumerable<Publications>> GetAllPublicationsFromUserAsync(string publisherUuid);

        public Task<Publications> GetPublicationAsync(string Uuid);

        public Task<IEnumerable<Publications>> GetLatestPublicationsAsync(int pageSize);

        public Task<IEnumerable<Publications>> GetPublicationsPaginatedAsync(string publisherUuid, int page, int pageSize);

        public Task<Publications> AddPublicationAsync(Publications publication);

        public Task<Publications> UpdatePublicationAsync(Publications publication);

        public Task<bool> DeletePublicationAsync(string Uuid);

    }
}
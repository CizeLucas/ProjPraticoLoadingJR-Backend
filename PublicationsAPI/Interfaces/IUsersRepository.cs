using PublicationsAPI.Models;

namespace PublicationsAPI.Interfaces
{
    public interface IUsersRepository
    {
        public Task<IEnumerable<Users>>? GetUsersPaginatedAsync(int page, int pageSize);
        public Task<Users>? GetByUuidAsync(string uuid);
        public Task<Users>? GetByUsernameAsync(string username);
        public Task<IEnumerable<Users>>? GetAllAsync();
        public Task<Users>? GetUserAsync(string uuid);
        public Task<Users>? getPublicationsByUser(string userUuid);
    }
}

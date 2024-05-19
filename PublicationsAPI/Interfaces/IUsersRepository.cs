using System;
using PublicationsAPI.Models;
using PublicationsAPI.DTO;

namespace PublicationsAPI.Interfaces
{
    public interface IUsersRepository
    {
        public Task<ICollection<Users>> GetAllAsync();

        public Task<ICollection<Users>> GetPaginatedAsync(int page, int pageSize);

        public Task<Users> GetByUuidAsync(string uuid);

        public Task<Users>? GetByUsernameAsync(string username);

        public Task<Users> AddUserAsync(UsersDTO user);

        public Task<Users> UpdateUserAsync(UsersDTO user, string userUuid);

        public Task<bool> UpdateUserPasswordAsync(string uuid, string passwordHash);

        public Task<bool> DeleteUserAsync(string uuid);
        
    }
}

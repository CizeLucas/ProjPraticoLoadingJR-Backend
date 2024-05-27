using System;
using PublicationsAPI.Models;
using PublicationsAPI.DTO.UserDTOs;

namespace PublicationsAPI.Interfaces
{
    public interface IUsersRepository
    {
        
        public Task<IEnumerable<Users>>? GetUsersPaginatedAsync(int page, int pageSize);
        public Task<Users>? GetByUuidAsync(string uuid);
        public Task<Users>? GetByUsernameAsync(string username);


        public Task<IEnumerable<Users>>? GetAllAsync();
        public Task<Users>? GetUserAsync(string uuid); //LoggedInUserResponse
        //public Task<Users>? AddUserAsync(UserRequest userDTO, string uuid);
        //public Task<Users>? UpdateUserAsync(UserRequest user, string userUuid);
        //public Task<bool> DeleteUserAsync(string uuid);

        public Task<Users>? getPublicationsByUser(string userUuid);
    }
}

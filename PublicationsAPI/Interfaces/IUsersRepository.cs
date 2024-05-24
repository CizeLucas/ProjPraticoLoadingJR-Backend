using System;
using PublicationsAPI.Models;
using PublicationsAPI.DTO.UserDTOs;

namespace PublicationsAPI.Interfaces
{
    public interface IUsersRepository
    {
        //NOT AUTHORIZED (PUBLIC) METHODS:
        
        public Task<IEnumerable<LoggedOutUserResponse>> GetPaginatedAsync(int page, int pageSize);
        public Task<LoggedOutUserResponse>? GetByUuidAsync(string uuid);
        public Task<LoggedOutUserResponse>? GetByUsernameAsync(string username);


        //ONLY IF USER IS AUTHORIZED METHODS:
        public Task<IEnumerable<LoggedOutUserResponse>> GetAllAsync();
        public Task<LoggedInUserResponse> GetUserAsync(string uuid);
        public Task<LoggedInUserResponse> AddUserAsync(UserRequest userDTO, string uuid);
        public Task<LoggedInUserResponse> UpdateUserAsync(UserRequest user, string userUuid);
        public Task<bool> DeleteUserAsync(string uuid);
    }
}

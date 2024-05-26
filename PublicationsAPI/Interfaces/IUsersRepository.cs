using System;
using PublicationsAPI.Models;
using PublicationsAPI.DTO.UserDTOs;

namespace PublicationsAPI.Interfaces
{
    public interface IUsersRepository
    {
        //NOT AUTHORIZED (PUBLIC) METHODS:
        
        public Task<IEnumerable<Users>>? GetUsersPaginatedAsync(int page, int pageSize);
        public Task<Users>? GetByUuidAsync(string uuid);
        public Task<Users>? GetByUsernameAsync(string username);


        //ONLY IF USER IS AUTHORIZED METHODS:
        public Task<IEnumerable<Users>>? GetAllAsync();
        public Task<Users>? GetUserAsync(string uuid); //LoggedInUserResponse
        public Task<Users>? AddUserAsync(UserRequest userDTO, string uuid, ImageUploadModel image);
        public Task<Users>? UpdateUserAsync(UserRequest user, string userUuid, ImageUploadModel image);
        public Task<bool> DeleteUserAsync(string uuid);

        public Task<Users>? getPublicationsByUser(string userUuid);
    }
}

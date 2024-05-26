using System;
using PublicationsAPI.Models;
using PublicationsAPI.DTO.UserDTOs;

namespace PublicationsAPI.Interfaces
{
    public interface IUsersServices
    {
        public Task<IEnumerable<Publications>> GetPublicationsByUserUuidService(string userUuid);

        public Task<LoggedOutUserResponse> GetUserByUsernameService(string Username);

        public Task<LoggedOutUserResponse> GetUserByUuidService(string Uuid);
        
        public Task<LoggedInUserResponse> GetUserService(string Uuid);  

        public Task<bool> DeleteUserService(string Uuid);

        public Task<IEnumerable<LoggedOutUserResponse>> GetAllUsersService();

        public Task<LoggedInUserResponse> AddUserService(UserRequest userDTO, string uuid, ImageUploadModel image);

        public Task<LoggedInUserResponse> UpdateUserService(UserRequest user, string userUuid, ImageUploadModel image);

        public Task<IEnumerable<LoggedOutUserResponse>>? GetUsersPaginatedService(int page, int pageSize);

    }
}
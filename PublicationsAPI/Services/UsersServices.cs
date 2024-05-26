using Microsoft.AspNetCore.Identity;
using PublicationsAPI.DTO.AccountDto;
using PublicationsAPI.DTO.Mappers;
using PublicationsAPI.DTO.UserDTOs;
using PublicationsAPI.Interfaces;
using PublicationsAPI.Models;

namespace PublicationsAPI.Services {
    public class UsersServices : IUsersServices
    {
        private readonly IUsersRepository _usersRepository;
        private readonly UserManager<Users> _userManager;
        private readonly IImageService _imageService;
        public UsersServices(IUsersRepository usersRepository, UserManager<Users> userManager, IImageService imageService)
        {
            _usersRepository = usersRepository;
            _userManager = userManager;
            _imageService = imageService;
        }

        public async Task<LoggedOutUserResponse> GetUserByUsernameService(string Username)
        {
            return (await _usersRepository.GetByUsernameAsync(Username)).UsersToLoggedOutUser();
        }

        public async Task<LoggedOutUserResponse> GetUserByUuidService(string Uuid)
        {
            return (await _usersRepository.GetByUuidAsync(Uuid)).UsersToLoggedOutUser();
        }

        public async Task<LoggedInUserResponse> GetUserService(string Uuid)
        {
            return (await _usersRepository.GetUserAsync(Uuid)).UsersToLoggedInUser();
        }

        public async Task<bool> DeleteUserService(string Uuid)
        {
            return await _usersRepository.DeleteUserAsync(Uuid);
        }

        public async Task<IEnumerable<Publications>> GetPublicationsByUserUuidService(string userUuid)
        {
            var user = await _usersRepository.getPublicationsByUser(userUuid);
            return user?.UserPublications ?? new List<Publications>();
        }

        public async Task<IEnumerable<LoggedOutUserResponse>> GetAllUsersService()
        {
            var users = await _usersRepository.GetAllAsync();

            if(users == null || users.Any())
                throw new Exception("UserServices Class: There is no user registered in the Database!");

            return users.Select(user => user.UsersToLoggedOutUser());
        }

        public async Task<LoggedInUserResponse> AddUserService(UserRequest userDTO, string uuid, ImageUploadModel image)
        {
            return (await _usersRepository.AddUserAsync(userDTO, uuid, image)).UsersToLoggedInUser();
        }

        public async Task<LoggedInUserResponse> UpdateUserService(UserRequest user, string userUuid, ImageUploadModel image)
        {
            return (await _usersRepository.UpdateUserAsync(user, userUuid, image)).UsersToLoggedInUser();
        }

        public async Task<IEnumerable<LoggedOutUserResponse>>? GetUsersPaginatedService(int page, int pageSize)
        {   
            var paginatedUsers = await _usersRepository.GetUsersPaginatedAsync(page, pageSize);

            return paginatedUsers.Select(user => user.UsersToLoggedOutUser());
        }
    }
}
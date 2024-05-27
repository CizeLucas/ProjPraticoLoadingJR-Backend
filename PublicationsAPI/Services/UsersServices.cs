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
        public async Task<IEnumerable<LoggedOutUserResponse>>? GetUsersPaginatedService(int page, int pageSize)
        {   
            var paginatedUsers = await _usersRepository.GetUsersPaginatedAsync(page, pageSize);

            return paginatedUsers.Select(user => user.UsersToLoggedOutUser());
        }


        public async Task<LoggedInUserResponse> AddUserService(UserRequest userDTO, string uuid, ImageUploadModel image)
        {
            if(userDTO == null)
				return null;

			Users? user = await _userManager.FindByIdAsync(uuid);

			if(user == null)
				return null;

			user.Name = userDTO.Name;
			user.UserName = userDTO.UserName;
			user.Bio = userDTO.Bio;
			if(image.Image != null)
            {
                try {    
                    user.ImageUrl = await _imageService.UploadImageAsync(image);
                } catch (Exception ex) {
                    throw new Exception("PublicationService Class: a problem with the image update occoured.", ex);
                }
            }

			var createResult = await _userManager.UpdateAsync(user);

            if (!createResult.Succeeded)
			{
				var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
				throw new Exception($"Failed to update user: {errors}");
			}

            return user.UsersToLoggedInUser();
        }


        public async Task<LoggedInUserResponse> UpdateUserService(UserRequest userDTO, string userUuid, ImageUploadModel image)
        {
             Users? user = await _userManager.FindByIdAsync(userUuid);

			if (user == null)
				return null;

			user.Name = userDTO.Name;
			user.UserName = userDTO.UserName;
			user.Bio = userDTO.Bio;

            //If an image is NOT already present and a new one is sent, it Uploads the new image and Assingns the imageUrl proprierty of the user
            //If an image is present and a new one is sent, it Uploads the new image and Updates the imageUrl proprierty of the user
            //If an image is present but none is sent in the update request, the existing image is deleted and the url user propierty is set to ""
            //If an image is NOT present and none is sent, it assigns "" to the url user proprierty.
            string? oldImageURL = user.ImageUrl;
            try {
                user.ImageUrl = await _imageService.updateImage(oldImageURL, image);
            } catch (Exception ex){
                throw new Exception("PublicationService Class: a problem with the image update occoured.", ex);
            }

			var updateResult = await _userManager.UpdateAsync(user);

			if (!updateResult.Succeeded)
			{
				var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
				throw new Exception($"Failed to update user: {errors}");
			}
			
            return user.UsersToLoggedInUser();
        }


        public async Task<bool> DeleteUserService(string Uuid)
        {
            Users? user = await _userManager.FindByIdAsync(Uuid);

			if(user == null)
				return false;
			
			var deleteResult = await _userManager.DeleteAsync(user);
			
			if (!deleteResult.Succeeded)
			{
				var errors = string.Join(", ", deleteResult.Errors.Select(e => e.Description));
				throw new Exception($"Failed to update user: {errors}");
			}

			return deleteResult.Succeeded;
        }
        
    }
}
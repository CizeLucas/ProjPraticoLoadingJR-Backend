using System.Linq;
using Microsoft.EntityFrameworkCore;
using PublicationsAPI.Data;
using PublicationsAPI.DTO.UserDTOs;
using PublicationsAPI.DTO.Mappers;
using PublicationsAPI.Interfaces;
using PublicationsAPI.Models;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;

namespace PublicationsAPI.Repositories
{
	public class UsersRepository : IUsersRepository
	{

		private readonly AppDBContext _context;
		private readonly UserManager<Users> _userManager;

		public UsersRepository(AppDBContext context, UserManager<Users> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		
        public async Task<IEnumerable<Users>> GetAllAsync()
        {
			return await _context.Users.ToListAsync<Users>();
        }
        public async Task<Users>? GetByUsernameAsync(string UserName)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.UserName == UserName);
        }
		public async Task<Users>? GetUserAsync(string uuid) 
		{
			return await _context.Users.FirstOrDefaultAsync(user => user.Uuid == uuid);
		}
        public async Task<Users>? GetByUuidAsync(string uuid)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Uuid == uuid);
        }

        public async Task<Users>? AddUserAsync(UserRequest userToCreate, string uuid, ImageUploadModel image)
        {
			if(userToCreate == null)
				return null;

			Users? user = await _userManager.FindByIdAsync(uuid);

			if(user == null)
				return null;

			user.Name = userToCreate.Name;
			user.UserName = userToCreate.UserName;
			user.Bio = userToCreate.Bio;
			user.ImageUrl = userToCreate.ImageUrl;

			var createResult = await _userManager.UpdateAsync(user);
            
			if (!createResult.Succeeded)
			{
				var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
				throw new Exception($"Failed to update user: {errors}");
			}

			return user; //await _userManager.FindByIdAsync(uuid);
        }

        public async Task<bool> DeleteUserAsync(string uuid)
        {
            Users? user = await _userManager.FindByIdAsync(uuid);

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
        public async Task<IEnumerable<Users>>? GetUsersPaginatedAsync(int page, int pageSize)
        {
			return await _context.Users.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<Users>? UpdateUserAsync(UserRequest updatedUser, string userUuid, ImageUploadModel image)
        {
            Users? user = await _userManager.FindByIdAsync(userUuid);

			if (user == null)
				return null;
			
			

			user.Name = updatedUser.Name;
			user.UserName = updatedUser.UserName;
			user.Bio = updatedUser.Bio;
			user.ImageUrl = updatedUser.ImageUrl;

			var updateResult = await _userManager.UpdateAsync(user);

			if (!updateResult.Succeeded)
			{
				var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
				throw new Exception($"Failed to update user: {errors}");
			}
			
			return user;
        }

        public async Task<Users>? getPublicationsByUser(string userUuid)
        {
            return await _context.Users.Include(u => u.UserPublications).FirstOrDefaultAsync(u => u.Uuid == userUuid);
        }
    }
}

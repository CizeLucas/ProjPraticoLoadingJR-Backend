using System.Linq;
using Microsoft.EntityFrameworkCore;
using PublicationsAPI.Data;
using PublicationsAPI.DTO.UserDTOs;
using PublicationsAPI.DTO.Mappers;
using PublicationsAPI.Interfaces;
using PublicationsAPI.Models;

namespace PublicationsAPI.Repositories
{
	public class UsersRepository : IUsersRepository
	{

		private readonly AppDBContext _context;

		public UsersRepository(AppDBContext context)
		{
			_context = context;
		}

        public async Task<LoggedInUserResponse> AddUserAsync(UserRequest userDTO)
        {

            if(userDTO == null)
				return null;

			Users user = UsersDTOMappers.UserRequestToUsers(userDTO);

			user.CreatedAt = DateTime.Now;
			user.Uuid = Guid.NewGuid().ToString("N"); //creates and formats the GUID

			if(user.UserName == null)
				user.UserName = user.Name.Replace(" ", "").Trim().ToLower();

			try{
				_context.Users.Update(user);
				await _context.SaveChangesAsync();
			}catch(Exception){
				return null;
			}

			return UsersDTOMappers.UsersToLoggedInUser(await GetUserWithUuid(user.Uuid));
        }

        public async Task<bool> DeleteUserAsync(string uuid)
        {
            Users user = await GetUserWithUuid(uuid);

			if(user == null)
				return false;
			
			_context.Users.Remove(user);
			return await _context.SaveChangesAsync() > 0 ;
        }

        public async Task<IEnumerable<LoggedOutUserResponse>> GetAllAsync()
        {
			var users = await _context.Users.ToListAsync<Users>();
			
            return users.Select(user => user.UsersToLoggedOutUser());
        }

        public async Task<LoggedOutUserResponse>? GetByUsernameAsync(string UserName)
        {
            return UsersDTOMappers.UsersToLoggedOutUser( 
				await _context.Users.FirstOrDefaultAsync(user => user.UserName == UserName)
			);
        }

        public async Task<LoggedOutUserResponse>? GetByUuidAsync(string uuid)
        {
            return UsersDTOMappers.UsersToLoggedOutUser( 
				await _context.Users.FirstOrDefaultAsync(user => user.Uuid == uuid) 
				);
        }

		public async Task<bool> EmailExistsAsync(string emailAddress){
			return await _context.Users.AnyAsync(u => u.Email == emailAddress);
		}

        public async Task<IEnumerable<LoggedOutUserResponse>> GetPaginatedAsync(int page, int pageSize)
        {
			var paginatedResponse = await _context.Users.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return paginatedResponse.Select(user => user.UsersToLoggedOutUser());
        }
		
        public async Task<bool> UpdateUserPasswordAsync(string uuid, string passwordHash)
        {
            Users user = await GetUserWithUuid(uuid);    
		
			if (user == null)
				return false;

			user.PasswordHash = passwordHash;

			try{
				_context.Users.Update(user);
				await _context.SaveChangesAsync();
			} catch(Exception){
				return false;
			}

			return true;
		}

        public async Task<LoggedInUserResponse> UpdateUserAsync(UserRequest updatedUser, string userUuid)
        {
            Users user = await GetUserWithUuid(userUuid);

			if (user == null)
				return null;

			
			user.Name = updatedUser.Name;
			user.UserName = updatedUser.UserName;
			user.Bio = updatedUser.Bio;
			user.Email = updatedUser.Email;
			user.ImageUrl = updatedUser.ImageUrl;

			try{
				_context.Users.Update(user);
				await _context.SaveChangesAsync();
			}catch(Exception){
				return null;
			}
			
			return UsersDTOMappers.UsersToLoggedInUser(await GetUserWithUuid(userUuid));
        }

		public async Task<LoggedInUserResponse> GetPersonalUserInfo(string uuid){
			throw new NotImplementedException();
		}

		private async Task<Users> GetUserWithUuid(string uuid){
			return await _context.Users.FirstOrDefaultAsync(user => user.Uuid == uuid) ;
		}

    }
}

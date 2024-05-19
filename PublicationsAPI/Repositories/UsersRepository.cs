using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PublicationsAPI.Data;
using PublicationsAPI.DTO;
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

        public async Task<Users> AddUserAsync(UsersDTO user)
        {
            if(user == null)
				return null;

			user.CreatetAt = DateTime.Now;
			var userGUID = Guid.NewGuid().ToString("N"); //creates and formats the GUID
			user.Uuid = userGUID;

			if(user.Username == null)
				user.Username = user.Name.Replace(" ", "").Trim().ToLower();

			try{
				_context.Users.Update(user);
				await _context.SaveChangesAsync();
			}catch(Exception){
				return null;
			}

			return await GetByUuidAsync(userGUID);
        }

        public async Task<bool> DeleteUserAsync(string uuid)
        {
            Users user = await GetByUuidAsync(uuid);

			if(user == null)
				return false;
			
			_context.Users.Remove(user);
			return( (await _context.SaveChangesAsync()) > 0 );
        }

        public async Task<ICollection<Users>> GetAllAsync()
        {
            return await _context.Users.ToListAsync<Users>();
        }

        public async Task<Users>? GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Username == username);
        }

        public async Task<Users>? GetByUuidAsync(string uuid)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Uuid == uuid);
        }

        public async Task<ICollection<Users>> GetPaginatedAsync(int page, int pageSize)
        {
            return await _context.Users.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<bool> UpdateUserPasswordAsync(string uuid, string passwordHash)
        {
            Users user = await GetByUuidAsync(uuid);        

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

        public async Task<Users> UpdateUserAsync(UsersDTO updatedUser, string userUuid)
        {
            Users user = await GetByUuidAsync(userUuid);

			if (user == null)
				return null;

			user.Bio = updatedUser.Bio;
			user.Username = updatedUser.Username;
			user.Name = updatedUser.Name;
			user.ImageUrl = updatedUser.ImageUrl;

			try{
				_context.Users.Update(user);
				await _context.SaveChangesAsync();
			}catch(Exception){
				return null;
			}
			
			return await GetByUuidAsync(userUuid);
        }
    }
}

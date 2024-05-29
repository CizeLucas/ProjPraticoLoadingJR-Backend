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
        
        public async Task<IEnumerable<Users>>? GetUsersPaginatedAsync(int page, int pageSize)
        {
			return await _context.Users.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<Users>? getPublicationsByUser(string userUuid)
        {
            return await _context.Users.Include(u => u.UserPublications).FirstOrDefaultAsync(u => u.Uuid == userUuid);
        }
		
    }
}

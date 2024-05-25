using PublicationsAPI.DTO.AccountDto;
using PublicationsAPI.Interfaces;
using PublicationsAPI.Models;

namespace PublicationsAPI.Services {
    public class UsersServices : IUsersServices
    {
        private readonly IUsersRepository _usersRepository;
        public UsersServices(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }
        public async Task<IEnumerable<Publications>> GetPublicationsByUserUuidAsync(string userUuid)
        {
            var user = await _usersRepository.getPublicationsByUser(userUuid);
            return user?.UserPublications ?? new List<Publications>();
        }
    }
}
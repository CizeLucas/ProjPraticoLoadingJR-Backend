using PublicationsAPI.Models;

namespace PublicationsAPI.Interfaces
{
    public interface ITokenService
    {
        public string CreateToken(Users user);
        public int GetExpirationTimeInMinutes();
    }
}
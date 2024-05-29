using PublicationsAPI.DTO.AccountDto;
using PublicationsAPI.Models;

namespace PublicationsAPI.Interfaces
{
    public interface IAccountsService
    {
        public Users RegisterUser(RegisterDto registerDto);

    }
}
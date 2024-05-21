using System;
using PublicationsAPI.DTO.AccountDto;
using PublicationsAPI.Models;
using PublicationsAPI.Services;

namespace PublicationsAPI.Interfaces
{
    public interface IAccountsService
    {
        public Users RegisterUser(RegisterDto registerDto);

    }
}
using System;
using AutoMapper;
using PublicationsAPI.Models;
using PublicationsAPI.DTO;

namespace PublicationsAPI.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Users, UsersDTO>();
        }

    }
}


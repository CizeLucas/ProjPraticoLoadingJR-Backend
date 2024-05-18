using System;
using AutoMapper;
using PublicationsAPI.Models;
using PublicationsAPI.Dto;

namespace PublicationsAPI.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Users, UsersDto>();
        }

    }
}


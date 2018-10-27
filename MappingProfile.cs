using AuthWebApi.Data.Users.Entities;
using AuthWebApi.Dto;
using AutoMapper;

namespace AuthWebApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // TODO Create mapping profiles
            // CreateMap<T1, T2>();

            CreateMap<RegisterDto, User>();
            CreateMap<UserDataDto, User>();
        }
    }
}
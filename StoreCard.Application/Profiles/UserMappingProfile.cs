using AutoMapper;
using StoreCard.Application.Dtos.User;
using StoreCard.Domain.Entities;

namespace StoreCard.Application.Profiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}

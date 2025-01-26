using AutoMapper;
using UserAPI.Models.DTOs;

namespace UserAPI.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
        CreateMap<UserDtoCreate, User>();
        CreateMap<User, UserDtoCreate>();
    }
}
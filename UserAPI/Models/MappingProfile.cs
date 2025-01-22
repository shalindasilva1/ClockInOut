using UserAPI.Models.DTOs;
using AutoMapper;

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
using UserAPI.Models;
using UserAPI.Models.DTOs;

namespace UserAPI.Services;

public interface IUserService
{
    Task<UserDto> RegisterUserAsync(UserDtoCreate userDto);
    Task<string> LoginUserAsync(UserDtoLogin userDto);
    Task<UserDto> GetUserByIdAsync(int id);
    Task<UserDto> GetUserByUsernameAsync(string username);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto> UpdateUserAsync(int id, UserDto userDto);
    Task DeleteUserAsync(int id);
}
using UserAPI.Models;
using UserAPI.Models.DTOs;

namespace UserAPI.Services;

public interface IUserService
{
    Task RegisterUserAsync(UserDto userDto);
    Task<UserDto> GetUserByIdAsync(int id);
    Task<UserDto> GetUserByUsernameAsync(string username);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto> UpdateUserAsync(int id, UserDto userDto);
    Task DeleteUserAsync(int id);
}
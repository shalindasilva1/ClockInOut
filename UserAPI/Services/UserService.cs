using AutoMapper;
using UserAPI.Models;
using UserAPI.Models.DTOs;
using UserAPI.Repositories;

namespace UserAPI.Services;

public class UserService(IUserRepository userRepository, IMapper mapper) : IUserService
{
    public async Task RegisterUserAsync(UserDto userDto)
    { 
        await userRepository.AddAsync(mapper.Map<User>(userDto));
    }

    public async Task<UserDto> GetUserByIdAsync(int id)
    {
        var result = await userRepository.GetByIdAsync(id);
        return mapper.Map<UserDto>(result);
    }

    public async Task<UserDto> GetUserByUsernameAsync(string username)
    {
        var result = await userRepository.GetByUsernameAsync(username);
        return mapper.Map<UserDto>(result);
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var result = await userRepository.GetAllAsync();
        return mapper.Map<IEnumerable<UserDto>>(result);
    }

    public async Task<UserDto> UpdateUserAsync(int id, UserDto userDto)
    {
        var user = await userRepository.GetByIdAsync(id);
        // user.Username = userDto.Username;
        // user.PasswordHash = userDto.PasswordHash;
        await userRepository.UpdateAsync(user);
        return mapper.Map<UserDto>(user);
    }

    public async Task DeleteUserAsync(int id)
    {
        await userRepository.DeleteAsync(id);
    }
}
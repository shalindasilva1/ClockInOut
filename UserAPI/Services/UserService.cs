using AutoMapper;
using FluentValidation;
using UserAPI.Models;
using UserAPI.Models.DTOs;
using UserAPI.Repositories;

namespace UserAPI.Services;

public class UserService(IUserRepository userRepository, IMapper mapper) : IUserService
{
    public async Task<UserDto> RegisterUserAsync(UserDtoCreate userDto)
    { 
        // 1. Validation (using FluentValidation)
        var validator = new UserDtoValidator();
        var validationResult = await validator.ValidateAsync(userDto);

        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors;
            throw new ValidationException(errorMessages);
        }
        
        // 2. Check if username or email already exists
        var existingUser = await userRepository.CheckUserAvailability(userDto.Username, userDto.Email);
        if (existingUser != null)
        {
            throw new Exception("Username or email already exists");
        }
        
        // 3. Hash password (using BCrypt)
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

        // 4. Mapping (using AutoMapper)
        var user = mapper.Map<User>(userDto);
        user.PasswordHash = hashedPassword;
        
        await userRepository.AddAsync(mapper.Map<User>(user));
        
        return mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> LoginUserAsync(UserDtoLogin userDto)
    {
        var user = await userRepository.GetByUsernameAsync(userDto.Username);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        if (!BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
        {
            throw new Exception("Invalid password");
        }

        return mapper.Map<UserDto>(user);
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
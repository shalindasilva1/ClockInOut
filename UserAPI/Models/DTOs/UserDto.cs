namespace UserAPI.Models.DTOs;

public record UserDto(int Id, string Username, string PasswordHash, string Email, string FirstName, string LastName );
public record UserDtoCreate(string Username, string Password, string Email, string FirstName, string LastName);
public record UserDtoLogin(string Username, string Password);
namespace UserAPI.Models.DTOs;

public record UserDto(int Id, string Username, string PasswordHash, string Email, string FirstName, string LastName );
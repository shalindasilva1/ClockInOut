namespace TeamAPI.Services;

public interface IUserGrpcService
{
    Task<bool> CheckUserExistsAsync(int userId);
}
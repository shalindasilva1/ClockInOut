using ClockInOut.Protos.User;

namespace TeamAPI.Services;

public class UserGrpcService : IUserGrpcService
{
    private readonly gRPCService.gRPCServiceClient _userClient;
    private readonly ILogger<UserGrpcService> _logger;

    public UserGrpcService(gRPCService.gRPCServiceClient userClient, ILogger<UserGrpcService> logger)
    {
        _userClient = userClient;
        _logger = logger;
    }
    public async Task<bool> CheckUserExistsAsync(int userId)
    {
        try
        {
            var response = await _userClient.CheckUserExistsAsync(
                new UserExistsRequest { UserId = userId });
            
            return response.Exists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking user existence for userId: {UserId}", userId);
            throw;
        }
    }
}
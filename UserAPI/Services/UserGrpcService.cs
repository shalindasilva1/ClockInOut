using ClockInOut.Protos.User;
using Grpc.Core;
using UserAPI.Repositories;

namespace UserAPI.Services
{
    public class UserGrpcService(IUserRepository userRepository) : gRPCService.gRPCServiceBase
    {
        public override async Task<UserExistsResponse> CheckUserExists(
            UserExistsRequest request, 
            ServerCallContext context)
        {
            try
            {
                var exists = await userRepository.GetByIdAsync(request.UserId);
                
                return new UserExistsResponse 
                { 
                    Exists = exists != null,
                };
            }
            catch (Exception ex)
            {
                return new UserExistsResponse 
                { 
                    Exists = false,
                };
            }
        }
    }
}
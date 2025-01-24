using Grpc.Core;
using UserAPI.Protos;

namespace UserAPI.Services;

public class GrpcService(IUserService userService) : Protos.UserGrpcService.UserGrpcServiceBase
{
    public override async Task<GetUserByIdResponse> GetUserById(GetUserByIdRequest request, ServerCallContext context)
    {
        var user = await userService.GetUserByIdAsync(request.Id);

        return new GetUserByIdResponse
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }
}
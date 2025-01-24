using AutoMapper;
using FluentValidation;
using TeamAPI.Models;
using TeamAPI.Models.DTOs;
using TeamAPI.Repositories;

    
namespace TeamAPI.Services;

public class TeamService(
    ITeamRepository teamRepository,
    IMapper mapper,
    IConfiguration configuration
) : ITeamService
{
    public async Task<TeamDto> CreateTeamAsync(TeamDto teamDto)
    {
        // 1. Validation (using TeamDtoValidator)
        var validator = new TeamDtoValidator();
        var validationResult = await validator.ValidateAsync(teamDto);
        
        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors;
            throw new ValidationException(errorMessages);
        }
        
        // 2. Mapping (map TeamDto to Team domain model)
        var team = mapper.Map<Team>(teamDto);

        // 3. Database persistence
        await teamRepository.AddAsync(team);

        return mapper.Map<TeamDto>(team);
    }

    public async Task<TeamDetailsDto> GetTeamByIdAsync(int id)
    {
        var team = await teamRepository.GetByIdAsync(id);
        return mapper.Map<TeamDetailsDto>(team);
    }

    public async Task<List<TeamDto>> GetAllTeamsAsync()
    {
        var teams = await teamRepository.GetAllAsync();
        return mapper.Map<List<TeamDto>>(teams);
    }

    public async Task<TeamDto> UpdateTeamAsync(int id, TeamDto teamDto)
    {
        // 1. Validation (using TeamDtoValidator)
        var validator = new TeamDtoValidator();
        var validationResult = await validator.ValidateAsync(teamDto);
        
        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors;
            throw new ValidationException(errorMessages);
        }
        
        // 2. Get the team from the database
        var team = await teamRepository.GetByIdAsync(id);
        if (team == null)
        {
            // Handle team not found
            throw new Exception("User not found");
        }

        // 3. Update team properties
        mapper.Map(teamDto, team);

        // 4. Database persistence
        await teamRepository.UpdateAsync(team);

        return mapper.Map<TeamDto>(team);
    }

    public async Task DeleteTeamAsync(int id)
    {
        await teamRepository.DeleteAsync(id);
    }

    public async Task AddUserToTeamAsync(int teamId, int userId)
    {
        // 1. Validate if the team and user exist
        var team = await teamRepository.GetByIdAsync(teamId);
        if (team == null)
        {
            // Handle team not found
            throw new Exception("Team not found");
        }
        
        // call user service using gRPC to check if user exists
        // try
        // {
        //     var grpcHost = configuration["gRPC:Host"];
        //     var channel = GrpcChannel.ForAddress(grpcHost);
        //     var client = new UserGrpcService.UserGrpcServiceClient(channel);
        //
        //     // 2. gRPC Call
        //     var response = await client.GetUserByIdAsync(new GetUserByIdRequest { Id = userId });
        //
        //     // 3. Check if User Exists (response will contain user details if found)
        //     if (response.Id == 0) 
        //     {
        //         throw new Exception("User not found"); 
        //     }
        // }
        // catch (RpcException ex)
        // {
        //     throw new Exception("gRPC call failed", ex); 
        // }
        
        // 2. Add the user to the team
        await teamRepository.AddUserToTeamAsync(teamId, userId);
    }

    public async Task RemoveUserFromTeamAsync(int teamId, int userId)
    {
        // 1. Validate if the team and user exist
        var team = await teamRepository.GetByIdAsync(teamId);
        if (team == null)
        {
            // Handle team not found
            throw new Exception("Team not found");
        }
        
        // call user service using gRPC to check if user exists
        
        // 2. Remove the user from the team
        await teamRepository.RemoveUserFromTeamAsync(teamId, userId);
    }
}
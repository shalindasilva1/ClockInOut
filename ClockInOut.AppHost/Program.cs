using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<ClockAPI>("ClockAPI");
builder.AddProject<UserAPI>("UserAPI");
builder.AddProject<TeamAPI>("TeamAPI");

builder.Build().Run();
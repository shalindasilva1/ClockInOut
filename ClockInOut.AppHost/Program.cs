using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username", secret: true);
var password = builder.AddParameter("password", secret: true);

var postgres = builder.AddPostgres("postgres", username, password)
    .WithDataVolume(isReadOnly: false)
    .WithPgAdmin();
var clockInOutDb = postgres.AddDatabase("ClockInOutDB");

builder.AddProject<ClockAPI>("ClockAPI");
builder.AddProject<UserAPI>("UserAPI");
builder.AddProject<TeamAPI>("TeamAPI");

builder.Build().Run();
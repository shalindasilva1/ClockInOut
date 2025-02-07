using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("cache");
var username = builder.AddParameter("username", secret: true);
var password = builder.AddParameter("password", secret: true);
var keycloakUsername = builder.AddParameter("keycloak-username", secret: true);
var keycloakPassword = builder.AddParameter("keycloak-password", secret: true);

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume(isReadOnly: false)
    .WithPgAdmin();

var clockDb = postgres.AddDatabase("clockDb");
//var userDb = postgres.AddDatabase("userDb");
var teamDb = postgres.AddDatabase("teamDb");

var keycloak = builder.AddKeycloak("keycloak", 7100, keycloakUsername, keycloakPassword)
    .WithDataVolume();

builder.AddProject<ClockAPI>("ClockAPI")
    .WithReference(keycloak)
    .WithReference(redis)
    .WithReference(clockDb);

// builder.AddProject<UserAPI>("UserAPI")
//     .WithReference(keycloak)
//     .WithReference(redis)
//     .WithReference(userDb);

builder.AddProject<TeamAPI>("TeamAPI")
    .WithReference(keycloak)
    .WithReference(redis)
    .WithReference(teamDb);

builder.AddProject<ClockAPI_MigrationService>("clockMigrationService")
    .WaitFor(clockDb)
    .WithReference(clockDb);

builder.AddProject<TeamAPI_MigrationService>("teamMigrationService")
    .WaitFor(teamDb)
    .WithReference(teamDb);

// builder.AddProject<UserAPI_MigrationService>("userMigrationService")
//     .WaitFor(userDb)
//     .WithReference(userDb);

builder.Build().Run();
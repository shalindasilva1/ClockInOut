using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Scalar.AspNetCore;
using TeamAPI;
using TeamAPI.Repositories;
using TeamAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddRedisOutputCache("cache");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Register the DbContext with PostgreSQL
builder.Services.AddDbContextPool<TeamDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("teamDb"), sqlOptions =>
    {
        sqlOptions.MigrationsAssembly("TeamAPI.MigrationService");
        sqlOptions.ExecutionStrategy(c => new NpgsqlRetryingExecutionStrategy(c));
    }));
builder.EnrichNpgsqlDbContext<TeamDbContext>(settings =>
    // Disable Aspire default retries as we're using a custom execution strategy
    settings.DisableRetry = true);

// Register the UserRepository
builder.Services.AddScoped<ITeamRepository, TeamRepository>();

// Register the UserService
builder.Services.AddScoped<ITeamService, TeamService>();

// Register the UserDtoValidator
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// JWT Authentication
builder.Services.AddAuthorization();
builder.Services.AddAuthentication()
    .AddKeycloakJwtBearer("keycloak", "ClockInOut", options =>
    {
        options.RequireHttpsMetadata = false;
        options.Audience = "account";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithHttpBearerAuthentication(bearer =>
        {
            bearer.Token = "your-bearer-token";
        });
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseOutputCache();

app.Run();
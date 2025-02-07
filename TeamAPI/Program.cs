using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Scalar.AspNetCore;
using TeamAPI;
using TeamAPI.Repositories;
using TeamAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add default services and Redis output cache
builder.AddServiceDefaults();

// Add Redis output cache with the specified cache name
builder.AddRedisOutputCache("cache");

// Add services to the container
builder.Services.AddControllers();

// Add API explorer for endpoint documentation
builder.Services.AddEndpointsApiExplorer();

// Add OpenAPI/Swagger services
builder.Services.AddOpenApi();

// Add AutoMapper with assemblies from the current AppDomain
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Configure the DbContext pool for TeamDbContext with PostgreSQL
builder.Services.AddDbContextPool<TeamDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("teamDb"), sqlOptions =>
    {
        // Specify the migrations assembly
        sqlOptions.MigrationsAssembly("TeamAPI.MigrationService");
        // Use a retrying execution strategy for PostgreSQL TODO: remove this when Aspire supports it
        sqlOptions.ExecutionStrategy(c => new NpgsqlRetryingExecutionStrategy(c));
    }));

// Enrich the DbContext with additional settings
builder.EnrichNpgsqlDbContext<TeamDbContext>(settings => settings.DisableRetry = true);

// Add scoped services for repositories and services
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<ITeamService, TeamService>();

// Add FluentValidation services
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// Add authorization services
builder.Services.AddAuthorization();

// Add authentication services with Keycloak JWT Bearer
builder.Services.AddAuthentication()
    .AddKeycloakJwtBearer("keycloak", "ClockInOut", options =>
    {
        options.RequireHttpsMetadata = false;
        options.Audience = "account";
    });

var app = builder.Build();

// Configure middleware for development environment
if (app.Environment.IsDevelopment())
{
    // Map OpenAPI endpoints
    app.MapOpenApi();
    // Map Scalar API reference with HTTP Bearer authentication
    app.MapScalarApiReference(options =>
    {
        options.WithHttpBearerAuthentication(bearer => bearer.Token = "your-bearer-token");
    });
}

// Use HTTPS redirection
app.UseHttpsRedirection();

// Use authentication middleware
app.UseAuthentication();

// Use authorization middleware
app.UseAuthorization();

// Map controller routes
app.MapControllers();

// Use output cache middleware
app.UseOutputCache();

app.Run();
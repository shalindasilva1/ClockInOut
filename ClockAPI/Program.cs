using ClockAPI;
using ClockAPI.Repositories;
using ClockAPI.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add default services to the builder
builder.AddServiceDefaults();

// Add Redis output cache with the specified cache name
builder.AddRedisOutputCache("cache");

// Add controllers to the services
builder.Services.AddControllers();

// Add API explorer for endpoint documentation
builder.Services.AddEndpointsApiExplorer();

// Add OpenAPI/Swagger services
builder.Services.AddOpenApi();

// Add AutoMapper with assemblies from the current AppDomain
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Configure the DbContext pool for ClockDbContext with PostgreSQL
builder.Services.AddDbContextPool<ClockDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("clockDb"), sqlOptions =>
    {
        // Specify the migrations assembly
        sqlOptions.MigrationsAssembly("ClockAPI.MigrationService");
        // Use a retrying execution strategy for PostgreSQL
        sqlOptions.ExecutionStrategy(c => new NpgsqlRetryingExecutionStrategy(c));
    }));

// Enrich the DbContext with additional settings
builder.EnrichNpgsqlDbContext<ClockDbContext>(settings => settings.DisableRetry = true);

// Add scoped services for repositories and services
builder.Services.AddScoped<ITimeEntryRepository, TimeEntryRepository>();
builder.Services.AddScoped<ITimeEntryService, TimeEntryService>();

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

// Run the application
app.Run();
using System.Text;
using ClockAPI;
using ClockAPI.Repositories;
using ClockAPI.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddRedisOutputCache("cache");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Register the DbContext with PostgreSQL
builder.Services.AddDbContextPool<ClockDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("clockDb"), sqlOptions =>
    {
        sqlOptions.MigrationsAssembly("ClockAPI.MigrationService");
        sqlOptions.ExecutionStrategy(c => new NpgsqlRetryingExecutionStrategy(c));
    }));

builder.EnrichNpgsqlDbContext<ClockDbContext>(settings =>
    // Disable Aspire default retries as we're using a custom execution strategy
    settings.DisableRetry = true);

// Register the TimeEntryRepository
builder.Services.AddScoped<ITimeEntryRepository, TimeEntryRepository>();

// Register the TimeEntryService
builder.Services.AddScoped<ITimeEntryService, TimeEntryService>();

// Register the TimeEntryDtoValidator
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
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
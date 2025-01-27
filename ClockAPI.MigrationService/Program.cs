using ClockAPI;
using ClockAPI.MigrationService;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHostedService<Worker>();

//builder.AddNpgsqlDbContext<ClockDbContext>("clockDb");
builder.Services.AddDbContextPool<ClockDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("clockDb"), sqlOptions =>
    {
        sqlOptions.MigrationsAssembly("ClockAPI.MigrationService");
        sqlOptions.ExecutionStrategy(c => new NpgsqlRetryingExecutionStrategy(c));
    }));
builder.EnrichNpgsqlDbContext<ClockDbContext>(settings =>
    // Disable Aspire default retries as we're using a custom execution strategy
    settings.DisableRetry = true);

var host = builder.Build();

host.Run();

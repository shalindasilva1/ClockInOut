using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using TeamAPI;
using TeamAPI.MigrationService;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHostedService<Worker>();

builder.Services.AddDbContextPool<TeamDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("teamDb"), sqlOptions =>
    {
        sqlOptions.MigrationsAssembly("TeamAPI.MigrationService");
        sqlOptions.ExecutionStrategy(c => new NpgsqlRetryingExecutionStrategy(c));
    }));
builder.EnrichNpgsqlDbContext<TeamDbContext>(settings =>
    // Disable Aspire default retries as we're using a custom execution strategy
    settings.DisableRetry = true);

var host = builder.Build();
host.Run();
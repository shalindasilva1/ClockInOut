using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using UserAPI;
using UserAPI.MigrationService;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHostedService<Worker>();

builder.Services.AddDbContextPool<UserDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("userDb"), sqlOptions =>
    {
        sqlOptions.MigrationsAssembly("UserAPI.MigrationService");
        sqlOptions.ExecutionStrategy(c => new NpgsqlRetryingExecutionStrategy(c));
    }));
builder.EnrichNpgsqlDbContext<UserDbContext>(settings =>
    // Disable Aspire default retries as we're using a custom execution strategy
    settings.DisableRetry = true);

var host = builder.Build();
host.Run();
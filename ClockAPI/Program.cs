using ClockAPI;
using ClockAPI.Repositories;
using ClockAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Register the DbContext with PostgreSQL
builder.Services.AddDbContext<ClockInOutDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register the TimeEntryRepository
builder.Services.AddScoped<ITimeEntryRepository, TimeEntryRepository>();

// Register the TimeEntryService
builder.Services.AddScoped<ITimeEntryService, TimeEntryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
using Microsoft.EntityFrameworkCore;
using ClassCompass.Shared.Data;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Server=localhost;Database=ClassCompassDb;User=root;Password=;";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.MapControllers();

// Health check endpoint
app.MapGet("/health", async (AppDbContext db) =>
{
    try
    {
        await db.Database.CanConnectAsync();
        return Results.Ok("Healthy - MySQL Connected");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Unhealthy - Database Error: {ex.Message}");
    }
});

app.Run();

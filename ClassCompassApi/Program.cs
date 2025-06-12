using Microsoft.EntityFrameworkCore;
using ClassCompass.Shared.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Entity Framework with MySQL (using your existing connection)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection") ?? 
        "Server=localhost;Database=classcompass_db;Uid=root;Pwd=your_password;",
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection") ?? 
                                "Server=localhost;Database=classcompass_db;Uid=root;Pwd=your_password;")
    ));

// Add CORS for your mobile app
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS
app.UseCors("AllowAll");

app.UseRouting();

app.UseAuthorization();

// Add a simple health check
app.MapGet("/health", () => Results.Ok(new { 
    status = "healthy", 
    timestamp = DateTime.UtcNow,
    database = "classcompass_db"
}));

app.MapControllers();

Console.WriteLine("🚀 ClassCompass API Starting...");
Console.WriteLine("🌐 Server: http://0.0.0.0:5004");
Console.WriteLine("📊 Swagger: http://localhost:5004/swagger");
Console.WriteLine("🗄️ Database: classcompass_db (MySQL)");

app.Run();

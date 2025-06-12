using Microsoft.EntityFrameworkCore;
using ClassCompass.Shared.Data;
using ClassCompassApi.Data;

namespace ClassCompassApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add MySQL Database
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(
                    Configuration.GetConnectionString("ClassCompassConnection"),
                    ServerVersion.AutoDetect(Configuration.GetConnectionString("ClassCompassConnection"))
                ));

            // Add controllers
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                });

            // Add API documentation
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
                { 
                    Title = "ClassCompass API", 
                    Version = "v1",
                    Description = "ClassCompass API with MySQL Database - Core Functions Only"
                });
            });

            // Add CORS for mobile app access
            services.AddCors(options =>
            {
                options.AddPolicy("AllowMobileApp", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Create database and tables if they don't exist
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                try
                {
                    context.Database.EnsureCreated();
                    Console.WriteLine("✅ MySQL Database and tables created successfully!");
                    Console.WriteLine("📊 Database: classcompass_db");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Database initialization failed: {ex.Message}");
                }
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClassCompass API v1");
                    c.RoutePrefix = "swagger";
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowMobileApp");
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("ClassCompass API with MySQL is running! 📱✅ Core endpoints: /api/students, /api/teachers, /api/schools");
                });
                endpoints.MapGet("/health", async context =>
                {
                    await context.Response.WriteAsync("Healthy - MySQL Connected");
                });
            });

            // Seed minimal data
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                try
                {
                    DataSeeder.SeedData(context);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Seeding failed: {ex.Message}");
                }
            }

            Console.WriteLine("🚀 ClassCompass API (Core) with MySQL Database started!");
            Console.WriteLine("🌐 API URLs: http://localhost:5004 and https://localhost:5005");
            Console.WriteLine("📱 Core endpoints working: Students, Teachers, Schools");
        }
    }
}

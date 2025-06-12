using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ClassCompassApp.Services;

namespace ClassCompassApp.Infrastructure.Sockets
{
    public static class EncryptedSocketServiceExtensions
    {
        public static IServiceCollection AddEncryptedSocketServices(this IServiceCollection services)
        {
            // Register socket-related services
            services.AddSingleton<IEncryptionService, EncryptionService>();
            services.AddSingleton<EncryptedTcpSocketServer>();
            
            // Add as hosted service if you want it to run in background
            // services.AddHostedService<EncryptedTcpSocketServer>();
            
            return services;
        }
    }
}


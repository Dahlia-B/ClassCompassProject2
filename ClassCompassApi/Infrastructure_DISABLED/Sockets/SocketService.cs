using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ClassCompass.Infrastructure.Sockets
{
    public interface ISocketService
    {
        Task SendNotificationAsync(string userId, string message);
        Task BroadcastMessageAsync(string message);
        Task SendToClassAsync(string classId, string message);
        Task SendToTeacherAsync(string teacherId, string message);
        Task SendToStudentAsync(string studentId, string message);
        int GetConnectedClientsCount();
    }

    public class SocketService : ISocketService
    {
        private readonly TcpSocketServer _server;
        private readonly ILogger<SocketService> _logger;

        public SocketService(TcpSocketServer server, ILogger<SocketService> logger)
        {
            _server = server;
            _logger = logger;
        }

        public async Task SendNotificationAsync(string userId, string message)
        {
            try
            {
                await _server.SendNotificationAsync(userId, message);
                _logger.LogInformation($"Notification sent to user {userId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send notification to user {userId}");
            }
        }

        public async Task BroadcastMessageAsync(string message)
        {
            try
            {
                await _server.BroadcastAsync(message);
                _logger.LogInformation("Message broadcasted to all clients");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to broadcast message");
            }
        }

        public async Task SendToClassAsync(string classId, string message)
        {
            // In a real implementation, you'd query the database to get all students in the class
            // For now, we'll use a targeted notification approach
            var notificationMessage = $"Class {classId}: {message}";
            await BroadcastMessageAsync(notificationMessage);
        }

        public async Task SendToTeacherAsync(string teacherId, string message)
        {
            await SendNotificationAsync($"teacher_{teacherId}", message);
        }

        public async Task SendToStudentAsync(string studentId, string message)
        {
            await SendNotificationAsync($"student_{studentId}", message);
        }

        public int GetConnectedClientsCount()
        {
            return _server.GetConnectedClientsCount();
        }
    }

    // Extension method to register socket services
    public static class SocketServiceExtensions
    {
        public static IServiceCollection AddSocketServices(this IServiceCollection services, int port = 8080)
        {
            services.AddSingleton<TcpSocketServer>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<TcpSocketServer>>();
                return new TcpSocketServer(logger, port);
            });

            services.AddSingleton<ISocketService, SocketService>();
            services.AddHostedService<TcpSocketServer>(provider => provider.GetRequiredService<TcpSocketServer>());

            return services;
        }
    }
}

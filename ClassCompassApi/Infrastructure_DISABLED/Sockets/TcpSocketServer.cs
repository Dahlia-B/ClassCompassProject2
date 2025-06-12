using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace ClassCompass.Infrastructure.Sockets
{
    public class TcpSocketServer : BackgroundService
    {
        private readonly ILogger<TcpSocketServer> _logger;
        private readonly TcpListener _listener;
        private readonly List<ConnectedClient> _clients;
        private readonly object _clientsLock = new object();
        private readonly int _port;

        public TcpSocketServer(ILogger<TcpSocketServer> logger, int port = 8080)
        {
            _logger = logger;
            _port = port;
            _listener = new TcpListener(IPAddress.Any, _port);
            _clients = new List<ConnectedClient>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _listener.Start();
                _logger.LogInformation($"TCP Socket Server started on port {_port}");

                while (!stoppingToken.IsCancellationRequested)
                {
                    var tcpClient = await _listener.AcceptTcpClientAsync();
                    _ = Task.Run(() => HandleClientAsync(tcpClient, stoppingToken), stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in TCP Socket Server");
            }
            finally
            {
                _listener?.Stop();
            }
        }

        private async Task HandleClientAsync(TcpClient tcpClient, CancellationToken cancellationToken)
        {
            var clientId = Guid.NewGuid().ToString();
            var client = new ConnectedClient
            {
                Id = clientId,
                TcpClient = tcpClient,
                ConnectedAt = DateTime.UtcNow
            };

            lock (_clientsLock)
            {
                _clients.Add(client);
            }

            _logger.LogInformation($"Client {clientId} connected from {tcpClient.Client.RemoteEndPoint}");

            try
            {
                var stream = tcpClient.GetStream();
                var buffer = new byte[4096];

                while (!cancellationToken.IsCancellationRequested && tcpClient.Connected)
                {
                    var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                    
                    if (bytesRead == 0)
                    {
                        break; // Client disconnected
                    }

                    var message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    await ProcessMessageAsync(client, message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error handling client {clientId}");
            }
            finally
            {
                lock (_clientsLock)
                {
                    _clients.Remove(client);
                }
                
                tcpClient.Close();
                _logger.LogInformation($"Client {clientId} disconnected");
            }
        }

        private async Task ProcessMessageAsync(ConnectedClient client, string message)
        {
            try
            {
                _logger.LogDebug($"Received message from {client.Id}: {message}");

                var socketMessage = JsonSerializer.Deserialize<SocketMessage>(message);
                
                switch (socketMessage.Type)
                {
                    case "ping":
                        await SendToClientAsync(client, new SocketMessage 
                        { 
                            Type = "pong", 
                            Content = "Server alive",
                            Timestamp = DateTime.UtcNow 
                        });
                        break;

                    case "register":
                        client.UserId = socketMessage.Content;
                        await SendToClientAsync(client, new SocketMessage 
                        { 
                            Type = "registered", 
                            Content = $"Registered as {client.UserId}",
                            Timestamp = DateTime.UtcNow 
                        });
                        break;

                    case "broadcast":
                        await BroadcastMessageAsync(socketMessage, client.Id);
                        break;

                    case "notification":
                        await HandleNotificationAsync(socketMessage);
                        break;

                    default:
                        _logger.LogWarning($"Unknown message type: {socketMessage.Type}");
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing message from client {client.Id}");
            }
        }

        private async Task SendToClientAsync(ConnectedClient client, SocketMessage message)
        {
            try
            {
                if (client.TcpClient.Connected)
                {
                    var json = JsonSerializer.Serialize(message);
                    var data = Encoding.UTF8.GetBytes(json);
                    await client.TcpClient.GetStream().WriteAsync(data, 0, data.Length);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending message to client {client.Id}");
            }
        }

        private async Task BroadcastMessageAsync(SocketMessage message, string excludeClientId = null)
        {
            var clients = new List<ConnectedClient>();
            lock (_clientsLock)
            {
                clients.AddRange(_clients);
            }

            var tasks = new List<Task>();
            foreach (var client in clients)
            {
                if (client.Id != excludeClientId)
                {
                    tasks.Add(SendToClientAsync(client, message));
                }
            }

            await Task.WhenAll(tasks);
        }

        private async Task HandleNotificationAsync(SocketMessage message)
        {
            // Send notification to specific user or broadcast
            if (!string.IsNullOrEmpty(message.TargetUserId))
            {
                var targetClient = GetClientByUserId(message.TargetUserId);
                if (targetClient != null)
                {
                    await SendToClientAsync(targetClient, message);
                }
            }
            else
            {
                await BroadcastMessageAsync(message);
            }
        }

        private ConnectedClient GetClientByUserId(string userId)
        {
            lock (_clientsLock)
            {
                return _clients.Find(c => c.UserId == userId);
            }
        }

        public async Task SendNotificationAsync(string userId, string content)
        {
            var message = new SocketMessage
            {
                Type = "notification",
                Content = content,
                TargetUserId = userId,
                Timestamp = DateTime.UtcNow
            };

            await HandleNotificationAsync(message);
        }

        public async Task BroadcastAsync(string content)
        {
            var message = new SocketMessage
            {
                Type = "broadcast",
                Content = content,
                Timestamp = DateTime.UtcNow
            };

            await BroadcastMessageAsync(message);
        }

        public int GetConnectedClientsCount()
        {
            lock (_clientsLock)
            {
                return _clients.Count;
            }
        }

        public override void Dispose()
        {
            _listener?.Stop();
            
            lock (_clientsLock)
            {
                foreach (var client in _clients)
                {
                    client.TcpClient?.Close();
                }
                _clients.Clear();
            }
            
            base.Dispose();
        }
    }

    public class ConnectedClient
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public TcpClient TcpClient { get; set; }
        public DateTime ConnectedAt { get; set; }
    }

    public class SocketMessage
    {
        public string Type { get; set; }
        public string Content { get; set; }
        public string TargetUserId { get; set; }
        public DateTime Timestamp { get; set; }
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
    }
}

using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ClassCompass.Infrastructure.Sockets
{
    public class TcpSocketClient : IDisposable
    {
        private readonly ILogger<TcpSocketClient> _logger;
        private TcpClient _client;
        private NetworkStream _stream;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _receiveTask;
        private readonly string _serverHost;
        private readonly int _serverPort;
        private bool _isConnected;

        public event EventHandler<SocketMessage> MessageReceived;
        public event EventHandler Connected;
        public event EventHandler Disconnected;

        public bool IsConnected => _isConnected && _client?.Connected == true;

        public TcpSocketClient(ILogger<TcpSocketClient> logger, string serverHost = "localhost", int serverPort = 8080)
        {
            _logger = logger;
            _serverHost = serverHost;
            _serverPort = serverPort;
        }

        public async Task<bool> ConnectAsync()
        {
            try
            {
                _client = new TcpClient();
                await _client.ConnectAsync(_serverHost, _serverPort);
                
                _stream = _client.GetStream();
                _cancellationTokenSource = new CancellationTokenSource();
                _isConnected = true;

                _receiveTask = Task.Run(ReceiveMessagesAsync, _cancellationTokenSource.Token);

                _logger.LogInformation($"Connected to server {_serverHost}:{_serverPort}");
                Connected?.Invoke(this, EventArgs.Empty);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to connect to server {_serverHost}:{_serverPort}");
                return false;
            }
        }

        public async Task DisconnectAsync()
        {
            try
            {
                _isConnected = false;
                _cancellationTokenSource?.Cancel();
                
                if (_receiveTask != null)
                {
                    await _receiveTask;
                }

                _stream?.Close();
                _client?.Close();

                _logger.LogInformation("Disconnected from server");
                Disconnected?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during disconnect");
            }
        }

        private async Task ReceiveMessagesAsync()
        {
            var buffer = new byte[4096];

            try
            {
                while (!_cancellationTokenSource.Token.IsCancellationRequested && _client.Connected)
                {
                    var bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length, _cancellationTokenSource.Token);
                    
                    if (bytesRead == 0)
                    {
                        break; // Server disconnected
                    }

                    var messageJson = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    
                    try
                    {
                        var message = JsonSerializer.Deserialize<SocketMessage>(messageJson);
                        MessageReceived?.Invoke(this, message);
                    }
                    catch (JsonException ex)
                    {
                        _logger.LogError(ex, $"Failed to deserialize message: {messageJson}");
                    }
                }
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Error receiving messages");
            }
            finally
            {
                _isConnected = false;
                Disconnected?.Invoke(this, EventArgs.Empty);
            }
        }

        public async Task<bool> SendMessageAsync(SocketMessage message)
        {
            if (!IsConnected)
            {
                _logger.LogWarning("Cannot send message - not connected to server");
                return false;
            }

            try
            {
                var json = JsonSerializer.Serialize(message);
                var data = Encoding.UTF8.GetBytes(json);
                await _stream.WriteAsync(data, 0, data.Length);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message");
                return false;
            }
        }

        public async Task<bool> SendPingAsync()
        {
            return await SendMessageAsync(new SocketMessage
            {
                Type = "ping",
                Content = "ping",
                Timestamp = DateTime.UtcNow
            });
        }

        public async Task<bool> RegisterAsync(string userId)
        {
            return await SendMessageAsync(new SocketMessage
            {
                Type = "register",
                Content = userId,
                Timestamp = DateTime.UtcNow
            });
        }

        public async Task<bool> SendNotificationAsync(string targetUserId, string content)
        {
            return await SendMessageAsync(new SocketMessage
            {
                Type = "notification",
                Content = content,
                TargetUserId = targetUserId,
                Timestamp = DateTime.UtcNow
            });
        }

        public async Task<bool> BroadcastAsync(string content)
        {
            return await SendMessageAsync(new SocketMessage
            {
                Type = "broadcast",
                Content = content,
                Timestamp = DateTime.UtcNow
            });
        }

        public void Dispose()
        {
            try
            {
                _cancellationTokenSource?.Cancel();
                _receiveTask?.Wait(TimeSpan.FromSeconds(5));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during disposal");
            }
            finally
            {
                _stream?.Dispose();
                _client?.Dispose();
                _cancellationTokenSource?.Dispose();
            }
        }
    }
}

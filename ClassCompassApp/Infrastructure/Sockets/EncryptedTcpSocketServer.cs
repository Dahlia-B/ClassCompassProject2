#nullable enable

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ClassCompassApp.Infrastructure.Sockets
{
    public class EncryptedTcpSocketServer
    {
        private TcpListener? _listener;
        private bool _isRunning;

        public async Task StartAsync(IPAddress ipAddress, int port)
        {
            _listener = new TcpListener(ipAddress, port);
            _listener.Start();
            _isRunning = true;

            while (_isRunning)
            {
                try
                {
                    var client = await _listener.AcceptTcpClientAsync();
                    _ = Task.Run(() => HandleClientAsync(client));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error accepting client: {ex.Message}");
                }
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            try
            {
                using (client)
                {
                    var stream = client.GetStream();
                    // Handle client communication here
                    await Task.Delay(100); // Placeholder
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling client: {ex.Message}");
            }
        }

        public void Stop()
        {
            _isRunning = false;
            _listener?.Stop();
        }
    }
}


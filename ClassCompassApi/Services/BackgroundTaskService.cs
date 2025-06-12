using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ClassCompass.Shared.Services.Threading
{
    public class BackgroundTaskService : BackgroundService
    {
        private readonly ILogger<BackgroundTaskService> _logger;
        private readonly ConcurrentQueue<Func<CancellationToken, Task>> _taskQueue;
        private readonly SemaphoreSlim _semaphore;

        public BackgroundTaskService(ILogger<BackgroundTaskService> logger)
        {
            _logger = logger;
            _taskQueue = new ConcurrentQueue<Func<CancellationToken, Task>>();
            _semaphore = new SemaphoreSlim(0);
        }

        public void QueueTask(Func<CancellationToken, Task> task)
        {
            _taskQueue.Enqueue(task);
            _semaphore.Release();
        }

        public void QueueAction(Action action)
        {
            QueueTask(_ =>
            {
                action();
                return Task.CompletedTask;
            });
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Background task service starting");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _semaphore.WaitAsync(stoppingToken);

                    if (_taskQueue.TryDequeue(out var task))
                    {
                        try
                        {
                            await task(stoppingToken);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error executing background task");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Expected when stopping
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error in background task service");
                }
            }

            _logger.LogInformation("Background task service stopped");
        }
    }
}
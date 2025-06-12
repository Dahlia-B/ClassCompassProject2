using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ClassCompass.Shared.Interfaces;

namespace ClassCompass.Shared.Services.Threading
{
    public class ThreadingService : IThreadingService
    {
        private readonly ILogger<ThreadingService> _logger;
        private readonly SynchronizationContext? _mainContext;

        public ThreadingService(ILogger<ThreadingService> logger)
        {
            _logger = logger;
            _mainContext = SynchronizationContext.Current;
        }

        public async Task RunOnMainThreadAsync(Action action)
        {
            if (_mainContext == null || SynchronizationContext.Current == _mainContext)
            {
                action();
                return;
            }

            var tcs = new TaskCompletionSource<bool>();

            _mainContext.Post(_ =>
            {
                try
                {
                    action();
                    tcs.SetResult(true);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error executing action on main thread");
                    tcs.SetException(ex);
                }
            }, null);

            await tcs.Task;
        }

        public async Task<T> RunOnMainThreadAsync<T>(Func<T> func)
        {
            if (_mainContext == null || SynchronizationContext.Current == _mainContext)
            {
                return func();
            }

            var tcs = new TaskCompletionSource<T>();

            _mainContext.Post(_ =>
            {
                try
                {
                    var result = func();
                    tcs.SetResult(result);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error executing function on main thread");
                    tcs.SetException(ex);
                }
            }, null);

            return await tcs.Task;
        }

        public async Task RunOnBackgroundThreadAsync(Action action)
        {
            await Task.Run(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error executing action on background thread");
                    throw;
                }
            });
        }

        public async Task<T> RunOnBackgroundThreadAsync<T>(Func<T> func)
        {
            return await Task.Run(() =>
            {
                try
                {
                    return func();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error executing function on background thread");
                    throw;
                }
            });
        }

        public async Task RunWithTimeoutAsync(Task task, TimeSpan timeout)
        {
            using var cts = new CancellationTokenSource(timeout);

            try
            {
                await task.WaitAsync(cts.Token);
            }
            catch (OperationCanceledException) when (cts.Token.IsCancellationRequested)
            {
                _logger.LogWarning("Task timed out after {Timeout}", timeout);
                throw new TimeoutException($"Task timed out after {timeout}");
            }
        }

        public async Task<T> RunWithTimeoutAsync<T>(Task<T> task, TimeSpan timeout)
        {
            using var cts = new CancellationTokenSource(timeout);

            try
            {
                return await task.WaitAsync(cts.Token);
            }
            catch (OperationCanceledException) when (cts.Token.IsCancellationRequested)
            {
                _logger.LogWarning("Task timed out after {Timeout}", timeout);
                throw new TimeoutException($"Task timed out after {timeout}");
            }
        }

        public CancellationTokenSource CreateCancellationTokenSource(TimeSpan? timeout = null)
        {
            return timeout.HasValue
                ? new CancellationTokenSource(timeout.Value)
                : new CancellationTokenSource();
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClassCompass.Shared.Interfaces
{
    public interface IThreadingService
    {
        Task RunOnMainThreadAsync(Action action);
        Task<T> RunOnMainThreadAsync<T>(Func<T> func);
        Task RunOnBackgroundThreadAsync(Action action);
        Task<T> RunOnBackgroundThreadAsync<T>(Func<T> func);
        Task RunWithTimeoutAsync(Task task, TimeSpan timeout);
        Task<T> RunWithTimeoutAsync<T>(Task<T> task, TimeSpan timeout);
        CancellationTokenSource CreateCancellationTokenSource(TimeSpan? timeout = null);
    }
}
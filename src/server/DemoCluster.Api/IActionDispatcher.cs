using System;
using System.Threading.Tasks;

namespace DemoCluster.Api
{
    public interface IActionDispatcher : IDisposable
    {
        bool CanDispatch();

        Task DispatchAsync(Func<Task> action);

        Task<T> DispatchAsync<T>(Func<Task<T>> action);
    }
}

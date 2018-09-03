using System;
using System.Threading.Tasks;

namespace CameraTest.Core.Common.Interfaces
{
    public interface IServerCommandWrapperService
    {
        event EventHandler<bool> IsBusyChanged;
        Task InternetServerCommandWrapper(Func<Task> action);
        Task InternetServerCommandWrapperWithBusy(Func<Task> action);
		Task DelayedInternetServerCommandWrapperWithBusy(Func<Task> action);
        Task ServerCommandWrapper(Func<Task> action);
		Task DelayedServerCommandWrapper(Func<Task> action);
        Task ServerCommandWrapperWithoutNotify(Func<Task> action);
        Task ServerCommandWrapperParallel(Func<Task> action);
        Task ServerCommandWrapperParallelWithoutNotify(Func<Task> action);   
    }
}

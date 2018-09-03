using System;
using System.Threading.Tasks;
using CameraTest.Core.Common.Interfaces;
using CameraTest.Core.PlatformAbstractions;
using CameraTest.Core.Resources;
using MvvmCross;

namespace CameraTest.Core.Common.Services
{
    public class ServerCommandWrapperService : IServerCommandWrapperService
    {
		public event EventHandler<bool> IsBusyChanged;

		bool _isBusy;
		public bool IsBusy
		{
			get { return _isBusy; }
			private set
			{
				_isBusy = value;
				IsBusyChanged?.Invoke(this, value);
			}
		}

		bool _internalBusy;

		public async Task InternetServerCommandWrapper(Func<Task> action)
		{
			if (!Mvx.Resolve<INetworkAccessibilityService>().HasAccess)
			{
				Mvx.Resolve<IUserInteractionService>().Alert(Translator.GetText("ConnectionRequiredString"));
				return;
			}
			try
			{
				await action();
			}
			catch (Exception ex)
			{
//				Mvx.Resolve<IExceptionHandlerService>().HandleException(ex);
			}
		}

		public async Task InternetServerCommandWrapperWithBusy(Func<Task> action)
		{
			if (!Mvx.Resolve<INetworkAccessibilityService>().HasAccess)
			{
				Mvx.Resolve<IUserInteractionService>().Alert(Translator.GetText("ConnectionRequiredString"));
				return;
			}
			try
			{
				IsBusy = true;
				await action();
			}
			catch (Exception ex)
			{
//				Mvx.Resolve<IExceptionHandlerService>().HandleException(ex);
			}
			finally
			{
				IsBusy = false;
			}
		}

		public async Task DelayedInternetServerCommandWrapperWithBusy(Func<Task> action)
		{
			if (!Mvx.Resolve<INetworkAccessibilityService>().HasAccess)
			{
				Mvx.Resolve<IUserInteractionService>().Alert(Translator.GetText("ConnectionRequiredString"));
				return;
			}

			if (_internalBusy)
				return;

			try
			{
				_internalBusy = true;
				await action();
				await Task.Delay(500);
			}
			catch (Exception ex)
			{
//				Mvx.Resolve<IExceptionHandlerService>().HandleException(ex);
			}
			finally
			{
				_internalBusy = false;
			}
		}

		public async Task DelayedServerCommandWrapper(Func<Task> action)
		{
			if (_internalBusy)
			{
				return;
			}
			try
			{
				_internalBusy = true;
				await action();
				await Task.Delay(500);
			}
			catch (Exception ex)
			{
//				Mvx.Resolve<IExceptionHandlerService>().HandleException(ex);
			}
			finally
			{
				_internalBusy = false;
			}
		}

		public async Task ServerCommandWrapper(Func<Task> action)
		{
			if (IsBusy)
			{
				return;
			}
			try
			{
				IsBusy = true;
				await action();
			}
			catch (Exception ex)
			{
//				Mvx.Resolve<IExceptionHandlerService>().HandleException(ex);
			}
			finally
			{
				IsBusy = false;
			}
		}

		public async Task ServerCommandWrapperWithoutNotify(Func<Task> action)
		{
			if (IsBusy)
				return;
			try
			{
				IsBusy = true;
				await action();
			}
			catch (Exception ex)
			{
//				Mvx.Resolve<IExceptionHandlerService>().HandleExceptionWithoutNotify(ex);
			}
			finally
			{
				IsBusy = false;
			}
		}

		public async Task ServerCommandWrapperParallel(Func<Task> action)
		{
			try
			{
				await action();
			}
			catch (Exception ex)
			{
//				Mvx.Resolve<IExceptionHandlerService>().HandleException(ex);
			}
		}

		public async Task ServerCommandWrapperParallelWithoutNotify(Func<Task> action)
		{
			try
			{
				await action();
			}
			catch (Exception ex)
			{
//				Mvx.Resolve<IExceptionHandlerService>().HandleExceptionWithoutNotify(ex);
			}
		}

	}
}

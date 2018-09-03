using CameraTest.Core.ViewModels;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using MvvmCross;

namespace CameraTest.Core
{
	public class AppStart : MvxAppStart
    {
	    private readonly IMvxNavigationService _mvxNavigationService;

        public AppStart(IMvxApplication application, IMvxNavigationService mvxNavigationService) :base(application, mvxNavigationService)
        {
        }

        protected override async void NavigateToFirstViewModel(object hint = null)
        {
            //#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            //            Task.Run(() => Mvx.Resolve<IPushNotificationService>().RegisterAppForRemoteNotification());
            //#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

//            _mvxNavigationService.Navigate<MainViewModel>();
            NavigationService.Navigate<MainViewModel>().GetAwaiter().GetResult();
        }
    }
}

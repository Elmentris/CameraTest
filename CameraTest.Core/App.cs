using System;
using CameraTest.Core.ViewModels;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.ViewModels;

namespace CameraTest.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterHandlers();
            RegisterObservers();
            CreateDefaultViewModelLocator();
            RegisterCustomAppStart<AppStart>();
        }

        private void RegisterHandlers()
        {
            //var handlerLocator = Mvx.Resolve<IHandlerLocatorService>();
            //handlerLocator.RegisterHandler(LoadApiCommand.NewLoad.Name, new NewLoadHandler());
            //handlerLocator.RegisterHandler(LoadApiCommand.CancelBid.Name, new CancelBidHandler());
            //handlerLocator.RegisterHandler(LoadApiCommand.RemoveLoad.Name, new RemoveLoadHandler());
            //handlerLocator.RegisterHandler(LoadApiCommand.UpdateLoadActivity.Name, new UpdateLoadActivityHandler());
            //handlerLocator.RegisterHandler(LoadApiCommand.LoadApproved.Name, new LoadApprovedHandler());
            //handlerLocator.RegisterHandler(LoadApiCommand.RejectLoad.Name, new RejectLoadHandler());
        }

        private void RegisterObservers()
        {
            //var notificationObserver = Mvx.Resolve<INotificationObservableService>();
            //notificationObserver.SetObserver((ResponseHandlerHubService)Mvx.Resolve<IResponseHandlerHubService>());
            //notificationObserver.SetObserver((ServerRequestQueueService)Mvx.Resolve<IServerRequestQueueService>());
        }

        protected override IMvxViewModelLocator CreateDefaultViewModelLocator()
        {
            // register the instance
            if (!Mvx.CanResolve<IMvxViewModelLocator>())
            {
                var locator = base.CreateDefaultViewModelLocator();
                Mvx.RegisterSingleton(locator);
            }

            return Mvx.Resolve<IMvxViewModelLocator>();
        }
    }
}

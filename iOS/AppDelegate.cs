using System;
using CameraTest.Core;
using Foundation;
using MvvmCross;
using MvvmCross.Platforms.Ios.Core;
using MvvmCross.Plugin.Messenger;
using UIKit;

namespace CameraTest.Ios
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : MvxApplicationDelegate<Setup, App>
    {
        // class-level declarations

        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            var result = base.FinishedLaunching(application, launchOptions);

            //Window = new UIWindow(UIScreen.MainScreen.Bounds) { BackgroundColor = UIColor.White };
            //var presenter = new MvxIosViewPresenter(this, Window);
            //var setup = new Setup(this, presenter);
            //setup.Initialize();
            //var startup = Mvx.Resolve<IMvxAppStart>();
            //startup.Start();

            //FFImageLoading.Config.Configuration config = FFImageLoading.ImageService.Instance.Config;
            //config.DiskCachePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), AppSettings.ImagesCachePath);
            //config.VerboseLogging = true;
            //config.Logger = new LoggerSmall();
            //ThemeHelper.SetStyle();

            //FFImageLoading.ImageService.Instance.Initialize(config);
            //Window.MakeKeyAndVisible();
            ////ApplicationDelegate.SharedInstance.FinishedLaunching(application, launchOptions);

            //if (!UIDevice.CurrentDevice.CheckSystemVersion(10, 0) || !UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            //    HandleStartPush(launchOptions);

            return result;
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
            try
            {
               // Mvx.Resolve<IMvxMessenger>().Publish(new ChangeStateAppMessages(this, true));
            }
            catch (Exception ex)
            {
                //Mvx.Resolve<IHockeyAppService>().LogError(ex);
            }
        }

        public override async void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.
            try
            {
                Window.EndEditing(false);
                //Mvx.Resolve<IMvxMessenger>().Publish(new ChangeStateAppMessages(this, true));

                //await Mvx.Resolve<IApplicationTokenCoreService>().Pause();
            }
            catch (Exception ex)
            {
                //Mvx.Resolve<IHockeyAppService>().LogError(ex);
            }
        }

        public override async void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            //await Mvx.Resolve<IPushNotificationService>().RegisterDevice(deviceToken.Description);
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            //TODO: Failed register for push
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
            try
            {
               // Mvx.Resolve<IMvxMessenger>().Publish(new ChangeStateAppMessages(this, false));

                //Mvx.Resolve<IApplicationTokenCoreService>().Resume();
            }
            catch (Exception ex)
            {
                //Mvx.Resolve<IHockeyAppService>().LogError(ex);
            }
        }
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            HandlePushNotification(userInfo);
        }

        //for iOS 10
        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            HandlePushNotification(userInfo);
        }

        void HandlePushNotification(NSDictionary userInfo)
        {
            try
            {
                //Mvx.Resolve<IPushNotificationService>().HandlePushNotification(userInfo, true);
            }
            catch (Exception ex)
            {
                //Mvx.Resolve<IMvxLog>().Trace(ex.Message, ex.StackTrace);
            }
        }

        private void HandleStartPush(NSDictionary options)
        {
            if (options != null)
            {
                try
                {
                    if (options.ContainsKey(new NSString("UIApplicationLaunchOptionsRemoteNotificationKey")))
                    {
                        NSDictionary data = options.ObjectForKey(new NSString("UIApplicationLaunchOptionsRemoteNotificationKey")) as NSDictionary;
                        //Mvx.Resolve<IPushNotificationService>().HandlePushNotification(data, true);
                    }
                }
                catch (Exception ex)
                {
                    //Mvx.Resolve<IHockeyAppService>().LogError(ex);
                }
            }
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }
    }
}


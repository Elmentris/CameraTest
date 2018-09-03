using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using CameraTest.Core;
using MvvmCross.Droid.Support.V7.AppCompat;
using Plugin.CurrentActivity;

namespace CameraTest.Droid
{
    //You can specify additional application information in this attribute
    [Application]
    public class MainApplication : MvxAppCompatApplication<Setup, App>, Application.IActivityLifecycleCallbacks
    {
        private readonly ActivityManager _activityManager;

        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          :base(handle, transer)
        {
            _activityManager = (ActivityManager)GetSystemService(ActivityService);
        }

        public override void OnCreate()
        {
            base.OnCreate();
            RegisterActivityLifecycleCallbacks(this);
            //A great place to initialize Xamarin.Insights and Dependency Services!
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
        }

        public bool IsAppInForeground
        {
            get
            {
                var appProcesses = _activityManager.RunningAppProcesses;
                if (appProcesses == null)
                {
                    return false;
                }

                string packageName = PackageName;
                foreach (var appProcess in appProcesses)
                {
                    if (appProcess.Importance == Importance.Foreground && appProcess.ProcessName.ToLowerInvariant() == packageName.ToLowerInvariant())
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}
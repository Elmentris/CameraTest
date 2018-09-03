using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using MvvmCross.Platforms.Android.Views;
using Android.Graphics;

namespace CameraTest.Droid.Views
{
    [Activity(MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, NoHistory = true)]
    public class SplashScreenActivity : MvxSplashScreenActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
    }
}
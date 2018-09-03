using System;
using System.Collections.Generic;
using System.Globalization;
using Android.App;
using Android.Content;
using Android.Telephony;
using Android.Widget;
using CameraTest.Core.PlatformAbstractions;
using CameraTest.Core.Resources;
using Java.Util;
using MvvmCross;
using MvvmCross.Platforms.Android;
using Plugin.CurrentActivity;

namespace CameraTest.Droid.Services
{
	[Android.Runtime.Preserve(AllMembers = true)]
	public class DroidPlatformService : IPlatformService
	{
        private readonly Context _context;
        private readonly ActivityManager _activityManager;

	    public DroidPlatformService()
        {
            _context = Application.Context;
            _activityManager = (ActivityManager)_context.GetSystemService(Context.ActivityService);
        }

		public string Local => Locale.Default.Language;

	    public string DefaultCountryISO
		{
			get
			{
				var tm = (TelephonyManager)_context.GetSystemService(Context.TelephonyService);
				var countryCode = tm?.NetworkCountryIso;
				if (!string.IsNullOrEmpty(countryCode))
					return countryCode.ToUpperInvariant();

				return Locale.Default.Country;
			}
		}

		public CultureInfo GetPreferredCulture()
		{
			return new CultureInfo(Locale.Default.Language);
		}

	    public void RateApp()
	    {
	        throw new NotImplementedException();
	    }

	    public string VersionApp
	    {
	        get
	        {            
	            return CrossCurrentActivity.Current.Activity.PackageManager.GetPackageInfo(Application.Context.PackageName, 0).VersionName;
	        }
	    }

	    public void OpenUrlLink(string url)
		{
		    if (string.IsNullOrEmpty(url))
		        return;

            var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(url));
			CrossCurrentActivity.Current.Activity.StartActivity(intent);
		}

		public void CopyText(string text)
		{
			ClipboardManager clipboard = (ClipboardManager)Application.Context.GetSystemService(Context.ClipboardService);
			ClipData clip = ClipData.NewPlainText(text, text);
			clipboard.PrimaryClip = clip;
		}

        public bool IsAppInForeground
        {
            get
            {
                IList<ActivityManager.RunningAppProcessInfo> appProcesses = _activityManager.RunningAppProcesses;
                if (appProcesses == null)
                {
                    return false;
                }
                string packageName = _context.PackageName;
                foreach (ActivityManager.RunningAppProcessInfo appProcess in appProcesses)
                {
                    if (appProcess.Importance == Importance.Foreground && appProcess.ProcessName.ToLowerInvariant() == packageName.ToLowerInvariant())
                    {
                        return true;
                    }
                }
                return false;
            }
        }

		public bool IsAppRunning
		{
			get
			{
				IList<ActivityManager.RunningAppProcessInfo> appProcesses = _activityManager.RunningAppProcesses;
				if (appProcesses == null)
					return false;

				string packageName = _context.PackageName;
				foreach (ActivityManager.RunningAppProcessInfo appProcess in appProcesses)
				{
					if (appProcess.ProcessName.ToLowerInvariant() == packageName.ToLowerInvariant())
						return true;
				}
				return false;
			}
		}

        public string UniqueDeviceId => string.Empty;

        public string GetCountryName(string code)
		{
			return new Locale("", code).DisplayName;
		}

	    public void Call(string phoneNumber)
	    {
            if(string.IsNullOrEmpty(phoneNumber))
                return;

		    var uri = Android.Net.Uri.Parse($"tel:{phoneNumber}");
		    var intent = new Intent(Intent.ActionDial, uri);
		    Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity?.StartActivity(intent);
        }

	    public void HideApp()
	    {
	        
	    }

	    public void SendEmail(string email)
	    {
	        if (string.IsNullOrEmpty(email))
	            return;

            Intent intent = new Intent(Intent.ActionSendto);
		    intent.SetType("message/rfc822");
		    intent.SetData(Android.Net.Uri.Parse($"mailto:{email}"));
		    intent.AddFlags(ActivityFlags.NewTask);
		    try
		    {
			    Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity?.StartActivity(intent);
		    }
		    catch (Exception)
		    {
			    Toast.MakeText(CrossCurrentActivity.Current.Activity, Translator.GetText("NoEmailClients"), ToastLength.Short)
				    .Show();
		    }
	    }

	    public void OpenWebSite(string webSite)
	    {
	        if (string.IsNullOrEmpty(webSite))
	            return;

            var uri = Android.Net.Uri.Parse(webSite);
	        var intent = new Intent(Intent.ActionView, uri);
	        Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity?.StartActivity(intent);
        }
	}
}

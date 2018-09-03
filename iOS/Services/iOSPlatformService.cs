using System;
using System.Globalization;
using CameraTest.Core.PlatformAbstractions;
using Foundation;
using UIKit;

namespace CameraTest.iOS.Services
{
    public class iOSPlatformService : IPlatformService
    {
        readonly CultureInfo _preferredculture;

        public iOSPlatformService()
        {
            _preferredculture = new CultureInfo(NSLocale.PreferredLanguages[0].Substring(0,2));
        }

        public string Local => NSLocale.CurrentLocale.LanguageCode;
//        public DevicePlatform Platform => DevicePlatform.Ios;
        public CultureInfo GetPreferredCulture() => _preferredculture;

        public void OpenUrlLink(string url)
        {
            if (string.IsNullOrEmpty(url))
                return;
			
			url = url.Trim();
			if (!url.StartsWith("http", StringComparison.Ordinal))
				url = "http://" + url;

            UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
        }

        public void RateApp()
        {
            throw new NotImplementedException();
        }

		public void CopyText(string text)
		{
			UIPasteboard clipboard = UIPasteboard.General;
			clipboard.String = text;
		}

		public string VersionApp
        {
            get { return NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"].ToString(); }
        }

        public bool IsAppInForeground
        {
            get
            {
                bool isAppInForeground = true;
                UIApplication.SharedApplication.InvokeOnMainThread(() =>
                {
                    isAppInForeground = UIApplication.SharedApplication.ApplicationState == UIApplicationState.Active;
                });
                return isAppInForeground;
            }
        }

		public bool IsAppRunning
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public string DefaultCountryISO => NSLocale.CurrentLocale.CountryCode;

        public string UniqueDeviceId { get; set; }

//        public ClientId ClientId => ClientId.osbb_client_mobile;

//        public ClientType ClientType => ClientType.iOS;

        public string GetCountryName(string code)
		{
			return NSLocale.SystemLocale.GetCountryCodeDisplayName(code);
		}

        public void Call(string phoneNumber)
        {
            if(string.IsNullOrEmpty(phoneNumber))
                return;

            UIApplication.SharedApplication.OpenUrl(NSUrl.FromString("tel:" + phoneNumber));
        }

        public void HideApp()
        {
            throw new NotImplementedException();
        }

        public void SendEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return;

            var url = new NSUrl($@"mailto:{email}");
            UIApplication.SharedApplication.OpenUrl(url);
        }
    }
}

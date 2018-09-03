using System.Runtime.CompilerServices;
using Android.App;
using Android.Content;
using Android.Preferences;
using CameraTest.Core.PlatformAbstractions;
using Newtonsoft.Json;

namespace CameraTest.Droid.Services
{
	public class DroidSettingsBaseService : ISettingsBaseService
	{
	    readonly ISharedPreferences _prefs;

		public DroidSettingsBaseService()
		{
			_prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
		}

		public T Get<T>(T defaultValue = default(T), [CallerMemberName] string key = "")
		{
			var str = _prefs.GetString(key, null);

            if (str == null || string.IsNullOrEmpty(str))
			{
				return defaultValue;
			}

            var obj = JsonConvert.DeserializeObject<T>(str);

            return obj;
		}

		public void Set<T>(T value, [CallerMemberName] string key = "")
		{
			var str = JsonConvert.SerializeObject(value);
			var editor = _prefs.Edit();
			editor.PutString(key, str);
			editor.Apply();
		}
	}
}

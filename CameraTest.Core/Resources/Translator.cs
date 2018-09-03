using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using CameraTest.Core.PlatformAbstractions;
using MvvmCross;
using MvvmCross.Logging;

namespace CameraTest.Core.Resources
{
	public static class Translator
	{
		static readonly ResourceManager ResManager;
		static readonly CultureInfo CultureInfo;

		static Translator()
		{
			var assembly = typeof(Translator).GetTypeInfo().Assembly;
			ResManager = new ResourceManager("CameraTest.Core.Resources.Resources", assembly);

            CultureInfo = Mvx.Resolve<IPlatformService>().GetPreferredCulture();
		}

		public static string GetText(string key = null, params object[] parameters)
		{
			try
			{				
				var text = ResManager.GetString(key, CultureInfo)?.Replace("\\n", "\n");
			    if (string.IsNullOrEmpty(text))
			        return key;

				return parameters != null && parameters.Any() ? string.Format(text, parameters) : text;
			}
			catch
			{
				Mvx.Resolve<IMvxLog>().Trace("Resource not found for {0}", key);
			}

			return key;
		}

	    public static string GetLanguage()
	    {
	        var cultureName = CultureInfo.Name;

	        if (cultureName == "ru")
	            return "ru";

	        //TODO: When other language ready
	        //if (cultureName == "en-US" || cultureName == "en-GB")
	        //return "ua";

	        return "uk";
	    }
    }
}

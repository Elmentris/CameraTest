using System.Globalization;

namespace CameraTest.Core.PlatformAbstractions
{
	public interface IPlatformService
	{
		CultureInfo GetPreferredCulture();

		void OpenUrlLink(string url);

		void CopyText(string text);
		bool IsAppInForeground { get; }
		bool IsAppRunning { get; }
		void RateApp();

		string VersionApp { get; }
		string Local { get; }
		string DefaultCountryISO { get; }
        string UniqueDeviceId { get; }
		string GetCountryName(string code);
	    void SendEmail(string email);
	    void Call(string phoneNumber);
	    void HideApp();
	}
}

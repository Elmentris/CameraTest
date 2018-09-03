using System;
using System.Linq;
using CameraTest.Core;
using MvvmCross.Converters;
using MvvmCross.IoC;
using MvvmCross.Logging;
using MvvmCross.Platforms.Ios.Core;
using Serilog;

namespace CameraTest.Ios
{
    public class Setup : MvxIosSetup<App>
    {
        protected override IMvxLogProvider CreateLogProvider()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.NSLog()
                .CreateLogger();
            return base.CreateLogProvider();
        }

  //      protected override System.Collections.Generic.IEnumerable<System.Reflection.Assembly> ValueConverterAssemblies
		//{
		//	get
		//	{
		//		var toReturn = base.ValueConverterAssemblies.ToList();
  //              //toReturn.Add(typeof(UserContactTypeToStringConverter).Assembly);
		//		return toReturn;
		//	}
		//}

		protected override void FillValueConverters(IMvxValueConverterRegistry registry)
		{
			base.FillValueConverters(registry);

			foreach (var assembly in ValueConverterAssemblies)
				foreach (var item in assembly.CreatableTypes().EndingWith("Converter"))
					registry.AddOrOverwrite(item.Name, (IMvxValueConverter)Activator.CreateInstance(item));
		}

        protected override void InitializePlatformServices()
        {
            base.InitializePlatformServices();
            
			CreatableTypes()
				.EndingWith("Service")
				.AsInterfaces()
				.RegisterAsLazySingleton();
        }

        //public override void LoadPlugins(MvvmCross.IMvxPluginManager pluginManager)
        //{
        //    base.LoadPlugins(pluginManager);
        //    Mvx.LazyConstructAndRegisterSingleton<IUserInteractionService, UserInteractionService>();
        //    //Mvx.LazyConstructAndRegisterSingleton<IFilePicker, Plugin.FilePicker.FilePickerImplementation>();
        //}

        //public override void Initialize()
        //{
        //    base.Initialize();
        //    Mvx.ConstructAndRegisterSingleton<IHockeyAppService, iOSHockeyAppService>();
        //    ServicePointManager.ServerCertificateValidationCallback += CertificateValidationCallBack;
        //}

        bool CertificateValidationCallBack(
        object sender,
        System.Security.Cryptography.X509Certificates.X509Certificate certificate,
        System.Security.Cryptography.X509Certificates.X509Chain chain,
        System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
            {
                return true;
            }

            if ((sslPolicyErrors & System.Net.Security.SslPolicyErrors.RemoteCertificateChainErrors) != 0)
            {
                if (chain != null && chain.ChainStatus != null)
                {
                    foreach (System.Security.Cryptography.X509Certificates.X509ChainStatus status in chain.ChainStatus)
                    {
                        if ((certificate.Subject == certificate.Issuer) &&
                           (status.Status == System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.UntrustedRoot))
                        {
                            continue;
                        }
                        else
                        {
                            if (status.Status != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.NoError)
                            {
                                return false;
                            }
                        }
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

using System;
using System.IO;
using System.Linq;

using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace MyExpenses.UITests
{
	public class AppInitializer
	{
		public static IApp StartApp (Platform platform)
		{
			if (platform == Platform.Android) {
				return ConfigureApp
					.Android
//					.ApkFile ("../../../Droid/bin/Debug/com.michaelwatson.myexpenses-Signed.apk")
					.StartApp (Xamarin.UITest.Configuration.AppDataMode.Clear);
			}

			return ConfigureApp
				.iOS
//				.AppBundle ("../../../iOS/bin/iPhoneSimulator/Debug/MyExpensesiOS.app")
//				.DeviceIdentifier ("001DA256-27C4-4B89-ABB5-9E8E9226E3A4")
//				.DeviceIdentifier ("XTC API Key")
//				.InstalledApp ("com.michaelwatson.myexpenses")
				.EnableLocalScreenshots ()
				.StartApp ();
		}
	}
}
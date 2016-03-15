using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace MyExpenses.iOS
{
	public class Application
	{
		// This is the main entry point of the application.
		static void Main (string[] args)
		{
			Xamarin.Insights.HasPendingCrashReport += (sender, isStartupCrash) => {
				if (isStartupCrash)
					Xamarin.Insights.PurgePendingCrashReports ().Wait ();	
			};

			#if DEBUG
			Xamarin.Insights.Initialize ("57537a140ed91deb4f55af4bf8a1b43225b9f60e");
			#else 
			Xamarin.Insights.Initialize ("e71401a46c7717ba1e6de93c0e3abda24c46b516");
			#endif
			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			UIApplication.Main (args, null, "AppDelegate");
		}
	}
}
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
			Xamarin.Insights.Initialize ("Debug API key");
			#else 
			Xamarin.Insights.Initialize ("Release API key");
			#endif

			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			UIApplication.Main (args, null, "AppDelegate");
		}
	}
}
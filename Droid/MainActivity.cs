using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Akavache;
using Xamarin.Forms.Platform.Android;

namespace MyExpenses.Droid
{
	[Activity (Label = "MyExpenses.Droid", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			Xamarin.Insights.HasPendingCrashReport += (sender, isStartupCrash) =>
			{
				if (isStartupCrash) 
					Xamarin.Insights.PurgePendingCrashReports().Wait();	
			};

			#if DEBUG
			Xamarin.Insights.Initialize ("57537a140ed91deb4f55af4bf8a1b43225b9f60e", ApplicationContext);
			#else 
			Xamarin.Insights.Initialize ("e71401a46c7717ba1e6de93c0e3abda24c46b516", ApplicationContext);
			#endif

//			global::Xamarin.Forms.Forms.Init (this, bundle);

			Xamarin.Forms.Forms.ViewInitialized += (object sender, Xamarin.Forms.ViewInitializedEventArgs e) => {
				if (!string.IsNullOrWhiteSpace(e.View.StyleId)) {
					e.NativeView.ContentDescription = e.View.StyleId;
				}
			};

//			BlobCache.ApplicationName = "MyExpenses";
//			BlobCache.EnsureInitialized();

			int densityDpi = (int)(Resources.DisplayMetrics.Density * 160f);
			Console.WriteLine ("DPI: " + densityDpi);

			//Need https://bugzilla.xamarin.com/show_bug.cgi?id=36907 to be resolved for AppCompatActivityjason
			FormsAppCompatActivity.ToolbarResource = Resource.Layout.toolbar;
			FormsAppCompatActivity.TabLayoutResource = Resource.Layout.tabs;


			LoadApplication (new App ());
		}
	}
}
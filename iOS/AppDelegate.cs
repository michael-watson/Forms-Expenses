using System;
using System.Collections.Generic;
using System.Linq;

using UIKit;
using Foundation;

using Xamarin.Forms;

using Plugin.Media;
using Plugin.Media.Abstractions;

using MyLoginUI.iOS;

using MyExpenses.iOS;
using MyExpenses.Models;
using MyExpenses.Databases;

namespace MyExpenses.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		ReportDatabase reportDatabase;
		App formsApp;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();//d

			Forms.ViewInitialized += (object sender, ViewInitializedEventArgs e) => {
				if (null != e.View.AutomationId)
					e.NativeView.AccessibilityIdentifier = e.View.AutomationId;	
			};

			reportDatabase = new ReportDatabase ();

			#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start ();
			#endif

			LoginUIExtension.Init ();
			CrossMedia.Current.Initialize ();

			formsApp = new App ();

			LoadApplication (formsApp);

			return base.FinishedLaunching (app, options);
		}

		public UIApplicationShortcutItem LaunchedShortcutItem { get; set; }

		public bool HandleShortcutItem (UIApplicationShortcutItem shortcutItem)
		{
			var handled = false;

			// Anything to process?
			if (shortcutItem == null)
				return false;

			// Take action based on the shortcut type
			switch (shortcutItem.Type) {
			//TODO: Need to handle force touch menu options
			case ShortcutIdentifier.NewReport:
				//Handle New Report Force Touch
				Console.WriteLine ("New Report Handle");
				handled = true;
				break;

			case ShortcutIdentifier.NewExpense:
				//Handle New Report Force Touch
				Console.WriteLine ("New Expense Handle"); 
				handled = true;
				break;
			}

			// Return results
			return handled;
		}

		public override void OnActivated (UIApplication application)
		{
			// Handle any shortcut item being selected
			HandleShortcutItem (LaunchedShortcutItem);

			// Clear shortcut after it's been handled
			LaunchedShortcutItem = null;
		}

		public override void PerformActionForShortcutItem (UIApplication application, UIApplicationShortcutItem shortcutItem, UIOperationHandler completionHandler)
		{
			// Perform action
			completionHandler (HandleShortcutItem (shortcutItem));
		}

		public override bool ContinueUserActivity (UIApplication application,
		                                           NSUserActivity userActivity, UIApplicationRestorationHandler completionHandler)
		{
			var dictionary = userActivity.UserInfo;
			var id = (int)Convert.ToInt32 (dictionary.Values [0].Description);
			// Take action based on the activity type
			switch (userActivity.ActivityType) {
			case "com.xamarin.platform":
				break;
			case "com.apple.corespotlightitem":
				formsApp.HandleSearch = reportDatabase.GetReport (id);
				break;
			}

			return true;
		}

		#region Xamarin Test Cloud Back Door Methods

		#if DEBUG
		[Export ("getReportName:")] // notice the colon at the end of the method name
		public NSString GetReportName (NSString reportName)
		{
			string _reportName = reportName.ToString ();
			_reportName = _reportName.Insert (0, "\'");
			_reportName = _reportName.Insert (_reportName.Length, "\'");
			var pendingSubmission = new List<ExpenseReport> (reportDatabase.GetReportByReportName (_reportName));
			ExpenseReport reportSaved = pendingSubmission [0];

			if (reportSaved == null)
				return new NSString ("");

			return new NSString (reportSaved.ReportName);
		}

		[Export ("getReportExpensesCount:")] // notice the colon at the end of the method name
		public NSString GetReportExpensesCount (NSString reportName)
		{
			string _reportName = reportName.ToString ();
			_reportName = _reportName.Insert (0, "\'");
			_reportName = _reportName.Insert (_reportName.Length, "\'");
			var pendingSubmission = new List<ExpenseReport> (reportDatabase.GetReportByReportName (_reportName));
			ExpenseReport reportSaved = pendingSubmission [0];

			if (reportSaved == null)
				return new NSString ("");

			var expenses = (List<ExpenseModel>)new MyExpensesDatabase ().GetExpensesForReport (reportSaved.ExpenseReportIdentifier);

			return new NSString (expenses.Count.ToString ());
		}

		[Export ("xtcAgent:")]
		public NSString TurnOffTouchId (NSString nothing)
		{
			App.XTCAgent = true;
			return new NSString ();
		}

		[Export ("clearKeyChain:")]
		public NSString ClearKeychain (NSString nothing)
		{
			NSUserDefaults.StandardUserDefaults.RemoveObject ("username");
			KeychainHelpers.DeletePasswordForUsername ("Michael", "XamarinExpenses", true);
			return new NSString ();
		}

		[Export ("getReportsCount:")]
		public NSString GetNumberOfReports (NSString nothing)
		{
			List<ExpenseReport> reports = (List<ExpenseReport>)reportDatabase.GetReports ();
			return new NSString (reports.Count.ToString ());
		}

		[Export ("createLoginBackdoor:")]
		public NSString CreateLogin (NSString nothing)
		{
			KeychainHelpers.SetPasswordForUsername ("admin", "password", "XamarinExpenses", Security.SecAccessible.Always, true);
			NSUserDefaults.StandardUserDefaults.SetString ("admin", "username");
			NSUserDefaults.StandardUserDefaults.Synchronize ();
			return new NSString ();
		}

		[Export ("addPhotosToGallery:")]
		public NSString AddPhotosToGallery (NSString filename)
		{
			for (var i = 1; i < 3; i++)
				UIImage.FromFile ("image" + i + ".jpg").SaveToPhotosAlbum ((image, error) => {
//					var o = image as UIImage;
					Console.WriteLine ("error:" + error);
				});
			return new NSString ();
		}
		#endif
		#endregion
	}
}
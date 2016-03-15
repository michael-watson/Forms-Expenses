using System;

using Android.App;
using Android.OS;
using Android.Runtime;
using Plugin.CurrentActivity;
using Akavache;
using MyExpenses.Models;
using System.Collections.Generic;
using MyExpenses.Databases;
using Android.Content;

namespace MyExpenses.Droid
{
	//You can specify additional application information in this attribute
    [Application]
    public class MainApplication : Application, Application.IActivityLifecycleCallbacks
    {
		ReportDatabase reportDatabase;

        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          :base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            RegisterActivityLifecycleCallbacks(this);
            //A great place to initialize Xamarin.Insights and Dependency Services!
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
			global::Xamarin.Forms.Forms.Init (activity, savedInstanceState);
			BlobCache.ApplicationName = "MyExpenses";
			BlobCache.EnsureInitialized();
			reportDatabase = new ReportDatabase ();
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
        }

		[Java.Interop.Export("CreateLoginBackdoor")]
		public void CreateLoginBackdoor ()
		{
			BlobCache.UserAccount.InsertObject<string> ("username", "admin");
			BlobCache.UserAccount.InsertObject<string> ("password", "password");
		}

		[Java.Interop.Export ("getReportName")] // notice the colon at the end of the method name
		public string GetReportName (string reportName)
		{
			string _reportName = reportName.ToString ();
			_reportName = _reportName.Insert (0, "\'");
			_reportName = _reportName.Insert (_reportName.Length, "\'");
			var pendingSubmission = new List<ExpenseReport> (reportDatabase.GetReportByReportName (_reportName));
			ExpenseReport reportSaved = pendingSubmission [0];

			if (reportSaved == null)
				return "";

			return reportSaved.ReportName;
		}

		[Java.Interop.Export ("getReportExpensesCount")] // notice the colon at the end of the method name
		public string getReportExpensesCount (string reportName)
		{
			string _reportName = reportName.ToString ();
			_reportName = _reportName.Insert (0, "\'");
			_reportName = _reportName.Insert (_reportName.Length, "\'");
			var pendingSubmission = new List<ExpenseReport> (reportDatabase.GetReportByReportName (_reportName));
			ExpenseReport reportSaved = pendingSubmission [0];

			if (reportSaved == null)
				return "";

			var expenses = (List<ExpenseModel>)new MyExpensesDatabase ().GetExpensesForReport (reportSaved.ExpenseReportIdentifier);

			return expenses.Count.ToString ();
		}

		[Java.Interop.Export ("xtcAgent")]
		public string TurnOffTouchId (string nothing)
		{
			App.XTCAgent = true;
			return "";
		}

		[Java.Interop.Export ("clearKeyChain")]
		public string ClearKeychain (string nothing)
		{
			return "";
		}

		[Java.Interop.Export ("getReportsCount")]
		public string getReportsCount (string nothing)
		{
			List<ExpenseReport> reports = (List<ExpenseReport>)reportDatabase.GetReports ();
			return reports.Count.ToString ();
		}

		[Java.Interop.Export ("addPhotosToGallery")]
		public void addPhotosToGallery ()
		{
			for (var i = 1; i < 3; i++) {
				Intent mediaScanIntent = new Intent (Intent.ActionMediaScannerScanFile);

				Android.Net.Uri contentUri = Android.Net.Uri.Parse("file:///android_asset/image" + i + ".jpg");
				mediaScanIntent.SetData (contentUri);
				SendBroadcast (mediaScanIntent);
			}
		}
    }
}
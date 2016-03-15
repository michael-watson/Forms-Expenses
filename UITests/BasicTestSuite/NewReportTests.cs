using System;
using NUnit.Framework;
using Xamarin.UITest;

namespace MyExpenses.UITests.BasicTestSuite
{
	[TestFixture (Platform.Android)]
	[TestFixture (Platform.iOS)]
	[Category ("Basic")]
	public class NewReportTests
	{
		public NewReportTests (Platform platform)
		{
			this.platform = platform;
		}

		IApp app;
		Platform platform;

		[SetUp]
		public void BeforeEachTest ()
		{
			app = AppInitializer.StartApp (platform);

			if (app is Xamarin.UITest.iOS.iOSApp) {
				app.Invoke ("xtcAgent:", "");
				app.Invoke ("clearKeyChain:", "");
				app.Invoke ("createLoginBackdoor:", "");
			} else {
				app.Invoke ("CreateLoginBackdoor");
			}
		}

		[Test]
		[Category ("Basic")]
		public void CancelNewReportCreation()
		{
			app.Screenshot ("Application Start");

			string initialReportCount = GetReportsCount ();

			Login ();
			StartNewReport ();

			app.WaitForThenTap (x => x.Marked ("cancelButton"), "Pressed 'Cancel'");
			app.WaitForThenTap (x => x.Marked ("Yes"), "Yes we are sure");

			string finalReportCount = GetReportsCount ();

			Assert.AreEqual (initialReportCount, finalReportCount);
			Assert.AreEqual (1, app.Query (x => x.Marked ("reportsPage")).Length);
		}

	[Test]
		[Category ("Basic")]
		public void CreateNewReportWithExpenses()
		{
			if (app is Xamarin.UITest.iOS.iOSApp)
				app.Invoke ("addPhotosToGallery:", "");
			else 
				app.Invoke ("addPhotosToGallery");
			

			app.Screenshot ("Application Start");
			if (app is Xamarin.UITest.iOS.iOSApp)
				app.WaitForThenTap (x => x.Marked ("Ok"), "Yes you can access my photos");

			Login ();

			string reportName = "Evolve 2014 Expenses";

			//Add New Report
			if (app is Xamarin.UITest.iOS.iOSApp)
				app.WaitForThenTap (x => x.Marked ("Plus_Add.png"), "Begin New Report");
			else 
				app.WaitForThenTap (x => x.Class ("ActionMenuItemView").Index(0), "Begin New Report");
			
			app.WaitForThenEnterText (x => x.Marked ("reportNameEntry"), reportName, "Changed Report Name to: " + reportName);

			//Add 1st New Expense to Report
			app.WaitForThenTap (x => x.Class ("UINavigationButton"), "Pressed 'Add Expense'");
			app.WaitForThenEnterText (x => x.Marked ("expenseNameEntry"), "Evolve Expense 1", "Changed report name to: Evolve Expense 1");
			app.WaitForThenEnterText (x => x.Marked ("priceEntry"), "5.56", "Changed Price to: $ 5.56");
			app.WaitForThenEnterText (x => x.Marked ("shortDescriptionEntry"), "Evolve was so much fun!!", "Changed short description to: Evolve was so much fun!!");

			var date = DateTime.Now.Subtract (TimeSpan.FromDays (40));
			app.WaitForThenTap (x => x.Marked ("expenseDatePicker"));

			if (app is Xamarin.UITest.iOS.iOSApp) {
				app.Query (x => x.Class ("UIPickerView").Invoke ("selectRow", date.Month - 1 , "inComponent", 0, "animated", true));
				app.Query (x => x.Class ("UIPickerView").Invoke ("selectRow", date.Day - 1, "inComponent", 1, "animated", true));
				app.Query (x => x.Class ("UIPickerView").Invoke ("selectRow", date.Year - 1, "inComponent", 2, "animated", true));
			} else {
				app.Query(x => x.Id("datePicker").Invoke("updateDate", date.Year, date.Month, date.Day));
			}

			app.WaitForThenTap (x => x.Text ("Done"));
			app.WaitForThenTap (x => x.Marked ("addReceiptButton"), "Click on photo button");
			app.WaitForThenTap (x => x.Marked ("Choose From Gallery"), "Choose From Gallery");
			app.WaitForThenTap (x => x.Marked ("Camera Roll"), "Go to Camera Roll");

			//TODO: Need to figure out solution for Android, currently only works for iOS because we don't have class of Android photos
			var pics = app.Query (x => x.Class ("PUPhotoView"));
			var rand = new Random ().Next (1, pics.Length);

			app.WaitForThenTap (x => x.Class ("PUPhotoView").Index (rand));
			app.WaitForThenTap (x => x.Marked ("saveExpenseButton"), "Pressed 'Save' Button");

			//Add 1st New Expense to Report
			app.WaitForThenTap (x => x.Class ("UINavigationButton"), "Pressed 'Add Expense'");
			app.WaitForThenEnterText (x => x.Marked ("expenseNameEntry"), "Evolve Expense 2", "Changed report name to: Evolve Expense 2");
			app.WaitForThenEnterText (x => x.Marked ("priceEntry"), "836.52", "Changed Price to: $ 836.52");
			app.WaitForThenEnterText (x => x.Marked ("shortDescriptionEntry"), "Evolve was so much fun!!", "Changed short description to: Evolve was so much fun!!");

			date = DateTime.Now.Subtract (TimeSpan.FromDays (42));
			app.WaitForThenTap (x => x.Marked ("expenseDatePicker"));

			if (app is Xamarin.UITest.iOS.iOSApp) {
				app.Query (x => x.Class ("UIPickerView").Invoke ("selectRow", date.Month - 1 , "inComponent", 0, "animated", true));
				app.Query (x => x.Class ("UIPickerView").Invoke ("selectRow", date.Day - 1, "inComponent", 1, "animated", true));
				app.Query (x => x.Class ("UIPickerView").Invoke ("selectRow", date.Year - 1, "inComponent", 2, "animated", true));
			} else {
				app.Query(x => x.Id("datePicker").Invoke("updateDate", date.Year, date.Month, date.Day));
			}

			app.WaitForThenTap (x => x.Text ("Done"));
			app.WaitForThenTap (x => x.Marked ("addReceiptButton"), "Click on photo button");
			app.WaitForThenTap (x => x.Marked ("Choose From Gallery"), "Choose From Gallery");
			app.WaitForThenTap (x => x.Marked ("Camera Roll"), "Go to Camera Roll");

			app.Repl ();

			//TODO: Need to figure out solution for Android, currently only works for iOS because we don't have class of Android photos
			pics = app.Query (x => x.Class ("PUPhotoView"));
			rand = new Random ().Next (1, pics.Length);

			app.WaitForThenTap (x => x.Class ("PUPhotoView").Index (rand));
			app.WaitForThenTap (x => x.Marked ("saveExpenseButton"), "Pressed 'Save' Button");

			app.WaitForThenTap (x => x.Marked ("saveReportButton"), "Pressed 'Save Report'");

			var reportNameFromDatabase = app.Invoke ("getReportName:", reportName);
			Assert.AreEqual (reportName, reportNameFromDatabase.ToString ());

			var expenseCount = app.Invoke ("getReportExpensesCount:", reportName);
			Assert.AreEqual ("2", expenseCount.ToString ());
		}

		[Test]
		[Category ("Basic")]
		public void CreateNewReportWithNoExpenses()
		{
			app.Screenshot ("Application Start");
//			CreateNewUserAndLogin ();

			string reportName = "Evolve 2014 Expenses";

			app.WaitForThenTap (x => x.Marked ("Plus_Add.png"), "Begin New Report");
			app.WaitForThenEnterText (x => x.Marked ("reportNameEntry"), reportName, "Changed Report Name to: " + reportName);
			app.DismissKeyboard ();
			app.WaitForThenTap (x => x.Marked ("saveReportButton"), "Pressed 'Save Report'");

			var token = app.Invoke ("getReportName:", reportName).ToString ();
			Assert.AreEqual (reportName, token);
			Assert.AreEqual (1, app.Query (x => x.Marked ("reportsPage")).Length);
		}

		string username = "admin";
		string password = "password";

		void Login()
		{
			app.WaitThenEnterText (x => x.Marked ("usernameEntry"), username, "Enter username: " + username);
			app.DismissKeyboard ();
			app.WaitThenEnterText (x => x.Marked ("passwordEntry"), password, "Enter password: " + password);
			app.DismissKeyboard ();
			app.WaitForThenTap (x => x.Marked ("loginButton"), "Pressed 'Log In' Button");
		}
		void StartNewReport()
		{
			if (app is Xamarin.UITest.iOS.iOSApp)
				app.WaitForThenTap (x => x.Marked ("Plus_Add.png"), "Begin New Report");
			else 
				app.WaitForThenTap (x => x.Class ("ActionMenuItemView").Index(0), "Begin New Report");
		}

		string GetReportsCount()
		{
			if (app is Xamarin.UITest.iOS.iOSApp)
				return app.Invoke ("getReportsCount:", "").ToString ();
			else 
				return app.Invoke ("getReportsCount", "").ToString ();
		}

		void CreateNewUserAndLogin()
		{
			app.WaitForThenTap (x => x.Marked ("newUserButton"), "Pressed 'Sign-up' Button");
			app.WaitForThenEnterText (x => x.Marked ("usernameEntry"), username, "Enter '" + username + "' as username");
			app.DismissKeyboard ();
			app.WaitForThenEnterText (x => x.Marked ("passwordEntry"), password, "Enter'" + password + "' as password");
			app.DismissKeyboard ();
			app.WaitForThenTap (x => x.Marked ("saveUsernameButton"), "Save New User");

			app.WaitThenEnterText (x => x.Marked ("usernameEntry"), username, "Enter username: " + username);
			app.DismissKeyboard ();
			app.WaitThenEnterText (x => x.Marked ("passwordEntry"), password, "Enter password: " + password);
			app.DismissKeyboard ();
			app.WaitForThenTap (x => x.Marked ("loginButton"), "Pressed 'Log In' Button");
		}
	} 
}
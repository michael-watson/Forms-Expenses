using System;
using System.Threading;
using System.Collections.Generic;

using Xamarin.UITest;

using NUnit.Framework;

using MyExpenses.UITests.PageObject.Pages;

namespace MyExpenses.UITests.PageObject.Tests
{
	[Category ("PageObject")]
	public class NewReportTests : AbstractSetup
	{
		public NewReportTests (Platform platform)
			: base (platform)
		{
		}

		public override void BeforeEachTest ()
		{
			base.BeforeEachTest ();

			if (app is Xamarin.UITest.iOS.iOSApp) {
				app.Invoke ("xtcAgent:", "");
				app.Invoke ("clearKeyChain:", "");
				app.Invoke ("createLoginBackdoor:", "");
			} 



			var loginPage = new LoginPage (app, platform);
			loginPage.LoginWithUsernamePassword ("admin", "password");
		}

		[Test]
		[Category ("PageObject")]
		public void CancelNewReportCreation ()
		{
			var initialReportCount = app.Invoke ("getReportsCount:", "").ToString ();

			new ReportsPage (app, platform).BeginNewReport ();
			;
			new NewReportPage (app, platform).PressCancelReportButton (true);

			var finalReportCount = app.Invoke ("getReportsCount:", "").ToString ();

			Assert.AreEqual (initialReportCount, finalReportCount);
			Assert.AreEqual (1, app.Query (x => x.Marked ("reportsPage")).Length);
		}

		[Test]
		[Category ("PageObject")]
		public void CreateNewReportWithExpenses ()
		{
			app.Invoke ("addPhotosToGallery:", "");

			string reportName = "Evolve 2014 Expenses";

			new ReportsPage (app, platform).BeginNewReport ();

			var reportDetailPage = new ReportDetailPage (app, platform);
			reportDetailPage.EnterNewReportName (reportName);
			reportDetailPage.AddExpenseToReport ();

			var newExpenseReportPage = new ExpenseActionPage (app, platform);
			newExpenseReportPage.EnterExpenseName ("Evolve Expense 1");
			newExpenseReportPage.ChangeExpensePrice (5.56);
			//Figure out how to change date
			newExpenseReportPage.EnterShortDescription ("Evolve was so much fun!!");

			var date = DateTime.Now.Subtract (TimeSpan.FromDays (40));

			newExpenseReportPage.ChangeExpenseDate (date);
			newExpenseReportPage.PickReceiptForExpense ();
			newExpenseReportPage.PressSaveExpenseButton ();

			reportDetailPage.AddExpenseToReport ();

			newExpenseReportPage.EnterExpenseName ("Evolve Expense 2");
			newExpenseReportPage.ChangeExpensePrice (105234.35);
			//Figure out how to change date
			newExpenseReportPage.EnterShortDescription ("Evolve was so much fun!!");
			newExpenseReportPage.PickReceiptForExpense ();
			newExpenseReportPage.PressSaveExpenseButton ();

			reportDetailPage.PressSaveReportButton ();

			var reportNameFromDatabase = app.Invoke ("getReportName:", reportName);
			Assert.AreEqual (reportName, reportNameFromDatabase.ToString ());

			var expenseCount = app.Invoke ("getReportExpensesCount:", reportName);
			Assert.AreEqual ("2", expenseCount.ToString ());
		}

		[Test]
		[Category ("PageObject")]
		public void CreateNewReportWithNoExpenses ()
		{
			string reportName = "Evolve 2014 Expenses";

			new ReportsPage (app, platform).BeginNewReport ();

			var reportDetailPage = new ReportDetailPage (app, platform);
			reportDetailPage.EnterNewReportName (reportName);
			reportDetailPage.PressSaveReportButton ();

			var token = app.Invoke ("getReportName:", reportName).ToString ();
			Assert.AreEqual (reportName, token);
			Assert.AreEqual (1, app.Query (x => x.Marked ("reportsPage")).Length);
		}
	}
}
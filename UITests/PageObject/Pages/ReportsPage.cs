using System;

using Xamarin.UITest;

using Expenses.UITests.Enums;

namespace MyExpenses.UITests.PageObject.Pages
{
	public class ReportsPage : BasePage
	{
		public ReportsPage (IApp app, Platform platform)
			: base (app, platform)
		{
		}

		public void BeginNewReport ()
		{
			app.WaitForThenTap (x => x.Marked ("Plus_Add.png"), "Begin New Report");
		}

		public void FilterReports (Status status)
		{
			app.WaitForThenTap (x => x.Marked ("filterReports"), "Filter Reports");

			switch (status) {
			case Status.Approved:
				app.Tap (x => x.Text ("Approved"), "Filter Approved");
				break;
			case Status.PendingApproval:
				app.Tap (x => x.Text ("Pending Approval"), "Filter Pending Approval");
				break;
			case Status.PendingSubmission:
				app.Tap (x => x.Text ("Pending Submission"), "Filter Pending Submission");
				break;
			case Status.All:
				app.Tap (x => x.Text ("All"), "Show All Reports");
				break;
			}
		}
	}
}
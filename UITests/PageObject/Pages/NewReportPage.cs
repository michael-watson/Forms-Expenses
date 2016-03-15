using System;

using Xamarin.UITest;

namespace MyExpenses.UITests.PageObject.Pages
{
	public class NewReportPage : BasePage
	{
		public NewReportPage (IApp app, Platform platform)
			: base (app, platform)
		{
		}

		public void EnterReportName (string reportName)
		{
			app.WaitFor (x => x.Marked ("reportNameEntry"));
			app.EnterText (x => x.Marked ("reportNameEntry"), reportName);
			app.Screenshot ("Changed Report Name to: " + reportName);
		}

		public void PressSaveReportButton ()
		{
			app.WaitForThenTap (x => x.Marked ("saveReportButton"), "Pressed 'Save Report'");
		}

		public void PressCancelReportButton (bool actuallyCancel)
		{
			app.WaitForThenTap (x => x.Marked ("cancelButton"), "Pressed 'Cancel'");
			if (actuallyCancel)
				app.WaitForThenTap (x => x.Marked ("Yes"), "Yes we are sure");
			else
				app.WaitForThenTap (x => x.Marked ("No"), "Nevermind");
		}

		public void AddExpenseToReport ()
		{
			app.WaitForThenTap (x => x.Marked ("addExpense"), "Pressed 'Add Expense'");
		}
	}
}
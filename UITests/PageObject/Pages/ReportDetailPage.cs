using System;

using Xamarin.UITest;

namespace MyExpenses.UITests.PageObject.Pages
{
	public class ReportDetailPage : BasePage
	{
		public ReportDetailPage (IApp app, Platform platform)
			: base (app, platform)
		{
		}

		/// <summary>
		/// Enters text in Report Name. 
		/// </summary>
		/// <param name="textToEnter">The text to enter into the report name.</param>
		/// /// <param name="deletePreviousText">A value of true will clear the text in Report Name before entering the next text. Default is false. </param>
		public void EnterReportName (string textToEnter, bool deletePreviousText)
		{
			app.WaitForThenTap (x => x.Marked ("editNameButton"), "Click edit button");

			if (deletePreviousText) {
				var charsToDelete = app.Query (x => x.Marked ("editNameButton")) [0].Text.Length;
				var stringToEnter = "";

				for (var i = 0; i < charsToDelete; i++)
					stringToEnter += "\b";
				
				app.EnterText (x => x.Marked ("reportNameEntry"), stringToEnter);
			}

			app.EnterText (x => x.Marked ("reportNameEntry"), textToEnter);
			app.Screenshot ("Changed Report Name to: " + textToEnter);
			app.DismissKeyboard ();
		}

		public void EnterNewReportName (string textToEnter)
		{
			app.WaitForThenTap (x => x.Marked ("reportNameEntry"), "Tap on Report Name field");
			app.EnterText (x => x.Marked ("reportNameEntry"), textToEnter);
			app.Screenshot ("Changed Report Name to: " + textToEnter);
			app.DismissKeyboard ();
		}

		public void PressSaveReportButton ()
		{
			app.WaitForThenTap (x => x.Marked ("saveReportButton"), "Pressed 'Save Report'");
		}

		public void PressCancelReportButton ()
		{
			app.WaitForThenTap (x => x.Marked ("cancelButton"), "Pressed 'Cancel'");
		}

		public void AddExpenseToReport ()
		{
			if (app is Xamarin.UITest.iOS.iOSApp) {
				app.WaitForThenTap (x => x.Class ("UINavigationButton"), "Pressed 'Add Expense'");
			} else {
				//TODO: Figure ouit Android NavBar button query
			}

		}
	}
}
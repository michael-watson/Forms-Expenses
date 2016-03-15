using System;

using Xamarin.UITest;
using System.Globalization;

namespace MyExpenses.UITests.PageObject.Pages
{
	public class ExpenseActionPage : BasePage
	{
		public ExpenseActionPage (IApp app, Platform platform)
			: base (app, platform)
		{
		}

		public void PickReceiptForExpense ()
		{
			//TODO: Need to figure out solution for Android, currently only works for iOS because we don't have class of Android photos
			app.WaitForThenTap (x => x.Marked ("addReceiptButton"), "Click on photo button");
			app.WaitForThenTap (x => x.Marked ("Choose From Gallery"), "Choose From Gallery");
			app.WaitForThenTap (x => x.Marked ("Camera Roll"), "Go to Camera Roll");

			var pics = app.Query (x => x.Class ("PUPhotoView"));
			var rand = new Random ().Next (1, pics.Length);

			app.WaitForThenTap (x => x.Class ("PUPhotoView").Index (rand));
		}

		public void ChangeExpenseDate (DateTime date)
		{
			app.WaitForThenTap (x => x.Marked ("expenseDatePicker"));

			if (app is Xamarin.UITest.iOS.iOSApp) {
				app.Query (x => x.Class ("UIPickerView").Invoke ("selectRow", date.Month - 1 , "inComponent", 0, "animated", true));
				app.Query (x => x.Class ("UIPickerView").Invoke ("selectRow", date.Day - 1, "inComponent", 1, "animated", true));
				app.Query (x => x.Class ("UIPickerView").Invoke ("selectRow", date.Year - 1, "inComponent", 2, "animated", true));
			} else {
				app.Query(x => x.Id("datePicker").Invoke("updateDate", date.Year, date.Month, date.Day));
			}

			app.WaitForThenTap (x => x.Text ("Done"));
		}

		public void ChangeExpensePrice (double price)
		{
			app.WaitFor (x => x.Marked ("priceEntry"));
			var currentPriceText = app.Query (x => x.Marked ("priceEntry")) [0].Text;
			var clearString = "";

			for (var i = 0; i < currentPriceText.Length; i++)
				clearString += "\b";

			if (currentPriceText != "$ 0.00")
				app.EnterText (x => x.Marked ("priceEntry"), clearString);
			app.EnterText (x => x.Marked ("priceEntry"), price.ToString ());
			app.Screenshot ("Changed Price to: " + price.ToString ("C"));
		
		}

		public void EnterShortDescription (string shortDescription)
		{
			app.WaitFor (x => x.Marked ("shortDescriptionEntry"));
			app.EnterText (x => x.Marked ("shortDescriptionEntry"), shortDescription);
			app.Screenshot ("Changed short description to: " + shortDescription);
		}

		public void EnterExpenseName (string name)
		{
			app.WaitFor (x => x.Marked ("expenseNameEntry"));
			app.EnterText (x => x.Marked ("expenseNameEntry"), name);
			app.Screenshot ("Changed report name to: " + name);
		}

		public void PressSaveExpenseButton ()
		{
			app.WaitForThenTap (x => x.Marked ("saveExpenseButton"), "Pressed 'Save' Button");
		}

		public void PressCancelExpenseButton ()
		{
			app.WaitForThenTap (x => x.Marked ("cancelExpenseButton"), "Pressed 'Cancel' Button");
		}

		public void PressDeleteExpenseButton ()
		{
			app.WaitForThenTap (x => x.Marked ("expenseDeleteButton"), "Pressed 'Delete' Button");
		}
	}
}
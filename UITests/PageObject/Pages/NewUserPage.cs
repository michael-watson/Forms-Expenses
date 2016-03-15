using System;

using Xamarin.UITest;

namespace MyExpenses.UITests.PageObject.Pages
{
	public class NewUserPage : BasePage
	{
		public NewUserPage (IApp app, Platform platform)
			: base (app, platform)
		{
		}

		public void CreateNewUserWithPassword (string username, string password)
		{
			EnterUsername (username);
			EnterPassword (password);
			SaveNewUser ();
		}

		public void EnterUsername (string username)
		{
			app.WaitForThenEnterText (x => x.Marked ("newUsernameEntry"), username, "Enter '" + username + "' as the username");
			app.DismissKeyboard ();
		}

		public void EnterPassword (string password)
		{
			app.WaitForThenEnterText (x => x.Marked ("newPasswordEntry"), password, "Enter '" + password + "' as the password");
			app.DismissKeyboard ();
		}

		public void SaveNewUser ()
		{
			app.WaitForThenTap (x => x.Marked ("saveUsernameButton"), "Save New User");
		}

		public void Cancel ()
		{
			app.WaitForThenTap (x => x.Marked ("cancelButton"), "Cancel new user");
		}
	}
}
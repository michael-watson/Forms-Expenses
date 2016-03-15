using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xamarin.UITest;

using NUnit.Framework;

namespace MyExpenses.UITests.PageObject.Pages
{
	public class LoginPage : BasePage
	{
		public LoginPage (IApp app, Platform platform)
			: base (app, platform)
		{
		}

		public void LoginWithUsernamePassword (string username, string password)
		{
			if (String.IsNullOrEmpty (app.Query (x => x.Marked ("usernameEntry")) [0].Text))
				EnterUsername (username);
			
			EnterPassword (password);
			app.WaitForThenTap (x => x.Marked ("loginButton"), "Pressed 'Log In' Button");
		}

		public void EnterUsername (string username)
		{
			if (String.IsNullOrEmpty (app.Query (x => x.Marked ("usernameEntry")) [0].Text)) {
				app.WaitThenEnterText (x => x.Marked ("usernameEntry"), username, "Enter username: " + username);
				app.DismissKeyboard ();
			}
		}

		public void EnterPassword (string password)
		{
			app.WaitThenEnterText (x => x.Marked ("passwordEntry"), password, "Enter password: " + password);
			app.DismissKeyboard ();
		}

		public void ClearUsername ()
		{
			app.ClearText ("usernameEntry");
		}

		public void PressNewUserButton ()
		{
			app.WaitForThenTap (x => x.Marked ("newUserButton"), "Pressed 'Sign-up' Button");
		}

		public void PressForgotPasswordButton ()
		{
			app.WaitForThenTap (x => x.Marked ("forgotPasswordButton"), "Pressed 'Forgot Password?' Button");
		}

		public void PressLoginButton ()
		{
			app.WaitForThenTap (x => x.Marked ("loginButton"), "Pressed 'Login' Button");
		}

		public void SignUpNewUserFromDialog ()
		{
			app.WaitForThenTap (x => x.Marked ("Sign-up"));
		}

		public void TryPasswordAgainDialog ()
		{
			app.WaitForThenTap (x => x.Marked ("Try again"));
		}
	}
}
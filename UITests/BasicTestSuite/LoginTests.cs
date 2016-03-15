using System;
using Xamarin.UITest;
using NUnit.Framework;
using System.Threading;

namespace MyExpenses.UITests.BasicTestSuite
{
	[TestFixture (Platform.Android)]
	[TestFixture (Platform.iOS)]
	[Category ("Basic")]
	public class LoginTests
	{
		IApp app;
		Platform platform;

		public LoginTests (Platform platform)
		{
			this.platform = platform;
		}

		[SetUp]
		public void BeforeEachTest ()
		{
			app = AppInitializer.StartApp (platform);

			if (app is Xamarin.UITest.iOS.iOSApp) {
				app.Invoke ("xtcAgent:", "");
				app.Invoke ("clearKeyChain:", "");
			}
		}

		string username = "Michael";
		string password = "xamarin";

		[Test]
		[Category ("Basic")]
		public void CreateNewUserAndLogin ()
		{
			app.Screenshot ("Application Start");
			CreateNewUser ();

			app.WaitFor (x => x.Marked ("usernameEntry"));
			app.ClearText ("usernameEntry");

			app.WaitThenEnterText (x => x.Marked ("usernameEntry"), username, "Enter username: " + username);
			app.DismissKeyboard ();
			app.WaitThenEnterText (x => x.Marked ("passwordEntry"), password, "Enter password: " + password);
			app.DismissKeyboard ();
			app.WaitForThenTap (x => x.Marked ("loginButton"), "Pressed 'Log In' Button");

			Assert.AreEqual (0,app.Query (x => x.Marked ("loginPage")).Length);
		}
		[Test]
		public void NewTest ()
		{
			app.Tap(x => x.Id("newUserButton"));
			app.Screenshot("Tapped on view UIButton with ID: 'newUserButton'");
			app.Tap(x => x.Marked("Username"));
			app.Screenshot("Tapped on view UITextFieldLabel with Text: 'Username'");
			app.EnterText(x => x.Id("newUsernameEntry"), "Michael");
			app.Screenshot("Entered 'Michael' into view UITextField with ID: 'newUsernameEntry'");
			app.Tap(x => x.Marked("Password"));
			app.Screenshot("Tapped on view UITextFieldLabel with Text: 'Password'");
			app.EnterText(x => x.Id("newPasswordEntry"), "passwrod");
			app.Screenshot("Entered 'passwrod' into view UITextField with ID: 'newPasswordEntry'");
			app.Tap(x => x.Text("Save Username"));
			app.Screenshot("Tapped on view UIButtonLabel with Text: 'Save Username'");
			app.Tap(x => x.Marked("Username"));
			app.Screenshot("Tapped on view UITextFieldLabel with Text: 'Username'");
			app.EnterText(x => x.Id("usernameEntry"), "Michael");
			app.Screenshot("Entered 'Michael' into view UITextField with ID: 'usernameEntry'");
			app.Tap(x => x.Marked("Password"));
			app.Screenshot("Tapped on view UITextFieldLabel with Text: 'Password'");
			app.EnterText(x => x.Id("passwordEntry"), "password");
			app.Screenshot("Entered 'password' into view UITextField with ID: 'passwordEntry'");
			app.Tap(x => x.Id("loginButton"));
			app.Screenshot("Tapped on view UIButton with ID: 'loginButton'");
			app.Tap(x => x.Marked("Try again"));
			app.Screenshot("Tapped on view _UIAlertControllerActionView");
			app.ClearText(x => x.Id("passwordEntry"));
			app.EnterText(x => x.Id("passwordEntry"), "passworpassword");
			app.Tap(x => x.Text("Login"));
			app.Screenshot("Tapped on view UIButtonLabel with Text: 'Login'");
			app.Tap(x => x.Marked("Sorry, we didn't recoginize the username or password. Feel free to sign up for free if you haven't!"));
			app.Screenshot("Tapped on view UILabel with Text: 'Sorry, we didn't recoginize the username or password. Feel free to sign up for free if you haven't!'");
		}

		[Test]
		[Category ("Basic")]
		public void CreateNewUserAndUnsuccessfullyLogin ()
		{
			app.Screenshot ("Application Start");
			CreateNewUser ();

			app.WaitFor (x => x.Marked ("usernameEntry"));
			app.ClearText ("usernameEntry");

			//Login with new user credentials but wrong password
			app.WaitThenEnterText (x => x.Marked ("usernameEntry"), username, "Enter username: " + username);
			app.DismissKeyboard ();
			app.WaitThenEnterText (x => x.Marked ("passwordEntry"), "incorrect", "Enter password: 'incorrect'");
			app.DismissKeyboard ();
			app.WaitForThenTap (x => x.Marked ("loginButton"), "Pressed 'Log In' Button");

			Assert.AreEqual (app.Query (x => x.Marked ("loginPage")).Length, 1);
		}

		[Test]
		[Category ("Basic")]
		public void CreateNewUserFromPromptAndLogin ()
		{
			//Try invalid login
			app.Screenshot ("Application Start");
			app.WaitThenEnterText (x => x.Marked ("usernameEntry"), username, "Enter username: " + username);
			app.DismissKeyboard ();
			app.WaitThenEnterText (x => x.Marked ("passwordEntry"), password, "Enter password: " + password);
			app.DismissKeyboard ();
			app.WaitForThenTap (x => x.Marked ("loginButton"), "Pressed 'Log In' Button");

			//Click on prompt to sign up new user
			app.WaitForThenTap (x => x.Marked ("Sign-up"), "Press 'Sign-up' in dialog");

			//Create new user
			app.WaitForThenEnterText (x => x.Marked ("newUsernameEntry"), username, "After Entering'" + username + "'");
			app.DismissKeyboard ();
			app.WaitForThenEnterText (x => x.Marked ("newPasswordEntry"), password, "After Entering'" + password + "'");
			app.DismissKeyboard ();
			app.WaitForThenTap (x => x.Marked ("saveUsernameButton"), "Save New User");
			app.WaitFor (x => x.Marked ("loginPage"));
			app.ClearText ("usernameEntry");
			app.ClearText ("passwordEntry");
			//Login with new user credentials
			app.WaitThenEnterText (x => x.Marked ("usernameEntry"), username, "Enter username: " + username);
			app.DismissKeyboard ();
			app.WaitThenEnterText (x => x.Marked ("passwordEntry"), password, "Enter password: " + password);
			app.DismissKeyboard ();
			app.WaitForThenTap (x => x.Marked ("loginButton"), "Pressed 'Log In' Button");

			Assert.AreEqual (app.Query (x => x.Marked ("loginPage")).Length, 0);
		}

		[Test]
		[Category ("Basic")]
		public void LoginWithCorrectUsernameAndPassword ()
		{
			app.Screenshot ("Application Start");
			CreateNewUser ();

			app.WaitFor (x => x.Marked ("usernameEntry"));
			app.ClearText ("usernameEntry");

			app.WaitThenEnterText (x => x.Marked ("usernameEntry"), username, "Enter username: " + username);
			app.DismissKeyboard ();
			app.WaitThenEnterText (x => x.Marked ("passwordEntry"), password, "Enter password: " + password);
			app.DismissKeyboard ();
			app.WaitForThenTap (x => x.Marked ("loginButton"), "Pressed 'Log In' Button");

			Thread.Sleep (500);
			Assert.AreEqual (1, app.Query (x => x.Marked ("reportsPage")).Length);
		}

		[Test]
		[Category ("Basic")]
		public void TryLoginWithNoPasswordEntered ()
		{
			app.WaitThenEnterText (x => x.Marked ("usernameEntry"), username, "Enter username: " + username);
			app.DismissKeyboard ();
			app.WaitForThenTap (x => x.Marked ("loginButton"), "Pressed 'Log In' Button");

			Assert.AreEqual (1, app.Query (x => x.Marked ("loginPage")).Length);
		}

		[Test]
		[Category ("Basic")]
		public void TryLoginWithNoUsernameEntered ()
		{
			app.WaitThenEnterText (x => x.Marked ("passwordEntry"), password, "Enter password: " + password);
			app.DismissKeyboard ();
			app.WaitForThenTap (x => x.Marked ("loginButton"), "Pressed 'Log In' Button");

			Assert.AreEqual (1, app.Query (x => x.Marked ("loginPage")).Length);
		}

		void CreateNewUser()
		{
			app.WaitForThenTap (x => x.Marked ("newUserButton"), "Pressed 'Sign-up' Button");
			app.WaitForThenEnterText (x => x.Marked ("newUsernameEntry"), username, "Enter '" + username + "' as the username");
			app.DismissKeyboard ();
			app.WaitForThenEnterText (x => x.Marked ("newPasswordEntry"), password, "Enter '" + password + "' as the password");
			app.DismissKeyboard ();
			app.WaitForThenTap (x => x.Marked ("saveUsernameButton"), "Save New User");
		}
	}
}
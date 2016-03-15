using System;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.UITest;

using NUnit.Framework;

using MyExpenses.UITests.PageObject.Pages;

namespace MyExpenses.UITests.PageObject.Tests
{
	[Category ("PageObject")]
	public class LoginTests : AbstractSetup
	{
		public LoginTests (Platform platform)
			: base (platform)
		{
		}

		public override void BeforeEachTest ()
		{
			base.BeforeEachTest ();

			if (app is Xamarin.UITest.iOS.iOSApp) {
				app.Invoke ("xtcAgent:", "");
				app.Invoke ("clearKeyChain:", "");
			}
		}

		[Test]
		[Category ("PageObject")]
		public void CreateNewUserAndLogin ()
		{
			var username = "Michael";
			var password = "test";

			app.Screenshot ("Application Start");

			var loginPage = new LoginPage (app, platform);
			loginPage.PressNewUserButton ();

			var newUserPage = new NewUserPage (app, platform);
			newUserPage.CreateNewUserWithPassword (username, password);

			loginPage.ClearUsername ();
			loginPage.LoginWithUsernamePassword (username, password);

			Assert.IsNotNull (app.Query (x => x.Marked ("loginPage")));
		}

		[Test]
		[Category ("PageObject")]
		public void CreateNewUserAndUnsuccessfullyLogin ()
		{
			var username = "Michael";
			var password = "test";

			app.Screenshot ("Application Start");

			var loginPage = new LoginPage (app, platform);
			loginPage.PressNewUserButton ();

			var newUserPage = new NewUserPage (app, platform);
			newUserPage.CreateNewUserWithPassword (username, password);

			loginPage.ClearUsername ();
			loginPage.LoginWithUsernamePassword (username, "incorrect");

			Assert.IsNotNull (app.Query (x => x.Marked ("loginPage")));
		}

		[Test]
		[Category ("PageObject")]
		public void CreateNewUserFromPromptAndLogin ()
		{
			var username = "Michael";
			var password = "test";

			app.Screenshot ("Application Start");

			var loginPage = new LoginPage (app, platform);
			loginPage.LoginWithUsernamePassword (username, password);
			loginPage.SignUpNewUserFromDialog ();

			var newUserPage = new NewUserPage (app, platform);
			newUserPage.CreateNewUserWithPassword (username, password);

			loginPage.ClearUsername ();
			loginPage.LoginWithUsernamePassword (username, password);
			Console.WriteLine("test");
			Assert.AreEqual (app.Query (x => x.Marked ("loginPage")).Length, 0);
		}

		[Test]
		[Category ("PageObject")]
		public void LoginWithCorrectUsernameAndPassword ()
		{
			var loginPage = new LoginPage (app, platform);
			loginPage.PressNewUserButton ();
			new NewUserPage (app, platform).CreateNewUserWithPassword ("michaelwatson", "xamarin");
			loginPage.LoginWithUsernamePassword ("michaelwatson", "xamarin");

			var reportsPage = app.Query (x => x.Marked ("reportsPage"));
			Assert.AreEqual (1, reportsPage.Length);
		}

		[Test]
		[Category ("PageObject")]
		public void TryLoginWithNoPasswordEntered ()
		{
			var loginPage = new LoginPage (app, platform);
			loginPage.EnterUsername ("michaelwatson");
			loginPage.PressLoginButton ();
			Assert.AreEqual (1, app.Query (x => x.Marked ("loginPage")).Length);
		}

		[Test]
		[Category ("PageObject")]
		public void TryLoginWithNoUsernameEntered ()
		{
			var loginPage = new LoginPage (app, platform);
			loginPage.EnterPassword ("xamarin");
			loginPage.PressLoginButton ();
			Thread.Sleep (500);
			Assert.AreEqual (1, app.Query (x => x.Marked ("loginPage")).Length);
		}
	}
}
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xamarin.Forms;

using MyLoginUI.Views;

namespace MyLoginUI.Pages
{
	public class ReusableLoginPage : ContentPage
	{
		#region LoginPage Properties

		string logoFileImageSource;

		public string LogoFileImageSource
		{
			get { return logoFileImageSource; }
			set
			{
				if (logoFileImageSource == value)
					return;
				logoFileImageSource = value;
				logo.Source = ImageSource.FromFile(logoFileImageSource);
			}
		}

		#endregion

		#region Internal Global References

		Image logo;
		RelativeLayout layout;
		LoginButton loginButton, newUserSignUpButton, forgotPasswordButton;
		LoginEntry loginEntry, passwordEntry;
		Label logoSlogan, rememberMe;
		Switch saveUsername;

		bool isInitialized = false;

		#endregion

		public ReusableLoginPage()
		{
			BackgroundColor = Color.FromHex("#3498db");
			Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
			layout = new RelativeLayout();

			CreateGlobalChildren();
			AddConstraintsToChildren();

			Content = layout;
		}

		#region UI Construction Methods

		void CreateGlobalChildren()
		{
			logo = new Image();
			logoSlogan = new Label
			{
				Opacity = 0,
				Text = "Delighting Developers.",
				TextColor = Color.White,
				FontFamily = Device.OnPlatform(
					iOS: "AppleSDGothicNeo-Light",
					Android: "Droid Sans Mono",
					WinPhone: "Comic Sans MS"),
				FontSize = 14
			};
			loginEntry = new LoginEntry
			{
				AutomationId = "usernameEntry",
				Placeholder = "Username",
			};
			passwordEntry = new LoginEntry
			{
				AutomationId = "passwordEntry",
				Placeholder = "Password",
				IsPassword = true,
			};
			loginButton = new LoginButton(Borders.Thin)
			{
				AutomationId = "loginButton",
				Text = "Login",
			};
			newUserSignUpButton = new LoginButton(Borders.None)
			{
				AutomationId = "newUserButton",
				Text = "Sign-up",
			};
			forgotPasswordButton = new LoginButton(Borders.None)
			{
				AutomationId = "forgotPasswordButton",
				Text = "Forgot Password?",
			};
			rememberMe = new Label
			{
				Opacity = 0,
				Text = "Remember Me",
				TextColor = Color.White,
				FontFamily = Device.OnPlatform(
					iOS: "AppleSDGothicNeo-Light",
					Android: "Droid Sans Mono",
					WinPhone: "Comic Sans MS"),
			};
			saveUsername = new Switch
			{
				AutomationId = "saveUsernameSwitch",
				IsToggled = true,
				Opacity = 0
			};

			loginButton.Clicked += (object sender, EventArgs e) =>
			{
				if (String.IsNullOrEmpty(loginEntry.Text) || String.IsNullOrEmpty(passwordEntry.Text))
				{
					DisplayAlert("Error", "You must enter a username and password.", "Okay");
					return;
				}

				Login(loginEntry.Text, passwordEntry.Text, saveUsername.IsToggled);
			};
			newUserSignUpButton.Clicked += (object sender, EventArgs e) =>
			{
				NewUserSignUp();
			};
			forgotPasswordButton.Clicked += (object sender, EventArgs e) =>
			{
				ForgotPassword();
			};
		}

		void AddConstraintsToChildren()
		{
			Func<RelativeLayout, double> getNewUserButtonWidth = (p) => newUserSignUpButton.GetSizeRequest(layout.Width, layout.Height).Request.Width;
			Func<RelativeLayout, double> getForgotButtonWidth = (p) => forgotPasswordButton.GetSizeRequest(layout.Width, layout.Height).Request.Width;
			Func<RelativeLayout, double> getLogoSloganWidth = (p) => logoSlogan.GetSizeRequest(layout.Width, layout.Height).Request.Width;
			Func<RelativeLayout, double> getRememberMeWidth = (p) => rememberMe.GetSizeRequest(layout.Width, layout.Height).Request.Width;
			Func<RelativeLayout, double> getRememberMeHeight = (p) => rememberMe.GetSizeRequest(layout.Width, layout.Height).Request.Height;
			Func<RelativeLayout, double> getSwitchWidth = (p) => saveUsername.GetSizeRequest(layout.Width, layout.Height).Request.Width;

			layout.Children.Add(
				logo,
				xConstraint: Constraint.Constant(100),
				yConstraint: Constraint.Constant(250),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - 200)
			);

			layout.Children.Add(
				logoSlogan,
				xConstraint: Constraint.RelativeToParent(p => (p.Width / 2) - (getLogoSloganWidth(p) / 2)),
				yConstraint: Constraint.RelativeToView(logo, (p, v) => 250 - (p.Height * 0.3) + v.Height)
			);

			layout.Children.Add(
				loginEntry,
				xConstraint: Constraint.Constant(40),
				yConstraint: Constraint.RelativeToParent(p => p.Height * 0.4),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - 80)
			);
			layout.Children.Add(
				passwordEntry,
				xConstraint: Constraint.Constant(40),
				yConstraint: Constraint.RelativeToView(loginEntry, (p, v) => v.Y + v.Height + 10),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - 80)
			);

			layout.Children.Add(
				rememberMe,
				xConstraint: Constraint.RelativeToParent(p => p.Width - 40 - getSwitchWidth(p) - getRememberMeWidth(p) - 20),
				yConstraint: Constraint.RelativeToView(passwordEntry, (p, v) => v.Y + v.Height + 25 + getRememberMeHeight(p) / 2)
			);
			layout.Children.Add(
				saveUsername,
				xConstraint: Constraint.RelativeToParent(p => p.Width - 40 - getSwitchWidth(p)),
				yConstraint: Constraint.RelativeToView(passwordEntry, (p, v) => v.Y + v.Height + 25)
			);

			layout.Children.Add(
				loginButton,
				xConstraint: Constraint.Constant(40),
				yConstraint: Constraint.RelativeToView(saveUsername, (p, v) => v.Y + v.Height + 25),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - 80)
			);
			layout.Children.Add(
				forgotPasswordButton,
				xConstraint: Constraint.RelativeToParent(p => (p.Width / 2) - (getForgotButtonWidth(p) / 2)),
				yConstraint: Constraint.RelativeToParent(p => p.Height - 50)
			);
			layout.Children.Add(
				newUserSignUpButton,
				xConstraint: Constraint.RelativeToParent(p => (p.Width / 2) - (getNewUserButtonWidth(p) / 2)),
				yConstraint: Constraint.RelativeToView(forgotPasswordButton, (p, v) => v.Y - v.Height)
			);
		}

		#endregion

		#region Virual Methods to Expose Override Methods

		public virtual void RunAfterAnimation()
		{
		}

		public virtual void Login(string userName, string passWord, bool saveUserName)
		{
		}

		public virtual void NewUserSignUp()
		{
		}

		public virtual void ForgotPassword()
		{
		}

		#endregion

		#region Page Overrides

		protected override void OnAppearing()
		{
			base.OnAppearing();

			if (String.IsNullOrEmpty(LogoFileImageSource))
				throw new Exception("You must set the LogoFileImageSource property to specify the logo");

			logo.Source = LogoFileImageSource;

			List<Task> animationTaskList;

			if (!isInitialized)
			{
				Device.BeginInvokeOnMainThread(async () =>
				{
					await Task.Delay(500);
					await logo?.TranslateTo(0, -layout.Height * 0.3 - 10, 250);
					await logo?.TranslateTo(0, -layout.Height * 0.3 + 5, 100);
					await logo?.TranslateTo(0, -layout.Height * 0.3, 50);

					await logo?.TranslateTo(0, -200 + 5, 100);
					await logo?.TranslateTo(0, -200, 50);

					var logoSloginAnimationTask = logoSlogan?.FadeTo(1, 5);
					var newUserSignUpButtonAnimationTask = newUserSignUpButton?.FadeTo(1, 250);
					var forgotPasswordButtonAnimationTask = forgotPasswordButton?.FadeTo(1, 250);
					var loginEntryAnimationTask = loginEntry?.FadeTo(1, 250);
					var passwordEntryAnimationTask = passwordEntry?.FadeTo(1, 250);
					var saveUsernameAnimationTask = saveUsername?.FadeTo(1, 250);
					var rememberMeAnimationTask = rememberMe?.FadeTo(1, 250);
					var loginButtonAnimationTask = loginButton?.FadeTo(1, 249);

					animationTaskList = new List<Task>
					{
						logoSloginAnimationTask,
						newUserSignUpButtonAnimationTask,
						forgotPasswordButtonAnimationTask,
						loginEntryAnimationTask,
						passwordEntryAnimationTask,
						saveUsernameAnimationTask,
						rememberMeAnimationTask,
						loginButtonAnimationTask
					};

					await Task.WhenAll(animationTaskList);

					isInitialized = true;
					RunAfterAnimation();
				});
			}
		}

		#endregion

		#region Extension Methods

		public void SetUsernameEntry(string password)
		{
			if (!String.IsNullOrEmpty(password))
				loginEntry.Text = password;
		}

		#endregion
	}
}
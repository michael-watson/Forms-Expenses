using System;

using Xamarin.Forms;

using MyLoginUI.Views;

using MyExpenses.Views;
using MyExpenses.Interfaces;

namespace MyExpenses.Pages
{
	public class NewUserSignUpPage : BasePage
	{
		Button saveUsernameButton, cancelButton;
		Entry usernameEntry, passwordEntry;
		StackLayout layout;

		public override void ConstructUI ()
		{
			base.ConstructUI ();
			//
			layout = new StackLayout {
				Padding = new Thickness (20, 50, 20, 20),
				VerticalOptions = LayoutOptions.FillAndExpand,
			};

			usernameEntry = new Entry {
				Style = (Style)App.Current.Resources ["underlinedEntry"],
				StyleId = "newUsernameEntry", 
				Placeholder = "Username",
				HorizontalOptions = LayoutOptions.Fill,
				HorizontalTextAlignment = TextAlignment.End
			};

			passwordEntry = new Entry {
				Style = (Style)App.Current.Resources ["underlinedEntry"],
				StyleId = "newPasswordEntry", 
				Placeholder = "Password",
				IsPassword = true,
				HorizontalOptions = LayoutOptions.Fill,
				HorizontalTextAlignment = TextAlignment.End,
				VerticalOptions = LayoutOptions.Fill
			};

			saveUsernameButton = new Button {
				Style = (Style)App.Current.Resources ["borderedButton"],
				StyleId = "saveUsernameButton",
				Text = "Save Username",
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.EndAndExpand
			};
			cancelButton = new Button {
				Style = (Style)App.Current.Resources ["borderedButton"],
				StyleId = "cancelButton",
				Text = "Cancel",
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.End
			};

			saveUsernameButton.Clicked += async (object sender, EventArgs e) => {
				var success = await DependencyService.Get<ITouchId> ().SetPasswordForUsername (usernameEntry.Text, passwordEntry.Text);
				if (success)
					Navigation.PopModalAsync ();
				else
					DisplayAlert ("Error", "You must enter a username and a password", "Okay");
			};

			cancelButton.Clicked += (object sender, EventArgs e) => {
				Navigation.PopModalAsync ();
			};
		}

		public override void AddChildrenToParentLayout ()
		{
			base.AddChildrenToParentLayout ();

			layout.Children.Add (
				new Label { 
					Style = (Style)App.Current.Resources ["whiteTextLabel"],
					Text = "Please enter username", 
					HorizontalOptions = LayoutOptions.Start 
				}
			);
			layout.Children.Add (usernameEntry);
			layout.Children.Add (
				new Label { 
					Style = (Style)App.Current.Resources ["whiteTextLabel"],
					Text = "Please enter password", 
					HorizontalOptions = LayoutOptions.Start 
				}
			);
			layout.Children.Add (passwordEntry);
			layout.Children.Add (saveUsernameButton);
			layout.Children.Add (cancelButton);

			Content = layout;
		}

		protected override void LayoutChildren (double x, double y, double width, double height)
		{
			cancelButton.WidthRequest = width - 40;
			saveUsernameButton.WidthRequest = width - 40;

			base.LayoutChildren (x, y, width, height);
		}
	}
}
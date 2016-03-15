using System;
using System.Collections.Generic;

using Xamarin.Forms;

using MyLoginUI.Views;

using MyExpenses.Pages;
using MyExpenses.Models;
using MyExpenses.Interfaces;

namespace MyExpenses
{
	public partial class App : Application
	{
		public static bool XTCAgent;
		public static bool IsLoggedIn;
		public static string UserName;
		public static ExpenseReport ProcessAfterLogin;

		public NavigationPage Navigation;

		public ExpenseReport HandleSearch {
			set {
				if (IsLoggedIn && value != null)
					Navigation.PushAsync (new ReportDetailPage (value));
				else {
					ProcessAfterLogin = value;
					Navigation.CurrentPage.Navigation.InsertPageBefore (new LoginPage (), Navigation.CurrentPage);
					Navigation.PopToRootAsync ();
				}
			}
		}

		public App ()
		{
			InitializeComponent ();

			var page = new LoginPage { LogoFileImageSource = "xamarin_logo.png" };
			NavigationPage.SetHasNavigationBar (page, false);
			Navigation = new NavigationPage (page) { 
				BarBackgroundColor = Color.FromHex ("#3498db"),
				BarTextColor = Color.White,
			};

			MainPage = Navigation;
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
			CheckForUsername ();
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
			CheckForUsername ();
			Xamarin.Insights.Track ("ApplicationResume");
		}

		async void CheckForUsername ()
		{
			var username = await DependencyService.Get<ITouchId> ().GetSavedUsername ();
			if (!String.IsNullOrEmpty (username) && username != "Not Found")
				App.UserName = username;
		}
	}
}
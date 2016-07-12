using System;

using Xamarin.Forms;

using Plugin.Media;
using Plugin.Media.Abstractions;

using MyLoginUI.Views;

using MyExpenses.Views;
using MyExpenses.Models;
using MyExpenses.Interfaces;
using MyExpenses.ViewModels;

namespace MyExpenses.Pages
{
	public class ExpenseActionPage : BasePage
	{
		ExpenseActionViewModel ViewModel;
		Label nameLabel, shortDescriptionLabel, priceLabel, dateLabel;
		Entry nameEntry, shortDescriptionEntry, priceEntry;
		Button saveButton, cancelButton;
		RelativeLayout formLayout;
		ExpenseDatePicker date;

		Image addReceipt;

		public ExpenseActionPage(ExpenseModel model = null, bool editable = true)
		{
			NavigationPage.SetTitleIcon(this, "icon.png");
			ViewModel = new ExpenseActionViewModel(editable);
			BindingContext = ViewModel;
			Title = "New Expense";

			if (model != null)
				ViewModel.Expense = model;
			//			else
			//				ViewModel.Expense.ReceiptLocation = "ic_insert_photo_white_48dp.png";

			AddConditionalUI();

			Content = formLayout;
			Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);

			#region Set Bindings And Event Handlers
			nameEntry.SetBinding(Entry.TextProperty, "Name");
			shortDescriptionEntry.SetBinding(Entry.TextProperty, "ShortDescription");
			priceEntry.SetBinding(Entry.TextProperty, "Price", BindingMode.TwoWay, null, "$ {0}");
			date.SetBinding(DatePicker.DateProperty, "Date");

			saveButton.Clicked += (object sender, EventArgs e) =>
			{
				var success = ViewModel.Save();

				if (success)
					Navigation.PopModalAsync();
				else
					DisplayAlert("Error", "You must enter a Vendor Name, Price and Short Description", "Ok");
			};
			cancelButton.Clicked += (object sender, EventArgs e) =>
			{
				Navigation.PopModalAsync();
			};

			TapGestureRecognizer tap = new TapGestureRecognizer();
			tap.Tapped += async (object sender, EventArgs e) =>
			{
				var result = await DisplayActionSheet("Media Source", "Cancel", null, "Take Picture", "Choose From Gallery");

				switch (result)
				{
					case "Cancel":
						return;
					case "Take Picture":
						if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
						{
							DisplayAlert("No Camera", "The camera is unavailable.", "OK");
							return;
						}
						else {
							var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
							{
								SaveToAlbum = true,
								Directory = "Pictures",
								Name = ViewModel.Expense.ExpenseId + ".png"
							});
							if (file == null)
								return;

							var source = ImageSource.FromStream(() => file.GetStream());
							addReceipt.Source = source;
							DependencyService.Get<IPhoto>().SavePictureToDisk(source, ViewModel.Expense.ExpenseId);

							file.Dispose();
						}
						break;
					case "Choose From Gallery":
						if (CrossMedia.Current.IsPickPhotoSupported)
						{
							var file = await CrossMedia.Current.PickPhotoAsync();

							if (file == null)
								return;

							var source = ImageSource.FromStream(() => file.GetStream());
							DependencyService.Get<IPhoto>().SavePictureToDisk(source, ViewModel.Expense.ExpenseId);
							addReceipt.Source = source;

							file.Dispose();
						}
						else {
							DisplayAlert("Sorry", "I Can't seem to find the photo gallery, try just taking a picture", "Dang, Ok");
							return;
						}
						break;
				}
			};
			addReceipt.GestureRecognizers.Add(tap);
			#endregion
		}

		#region Create UI

		public override void ConstructUI()
		{
			base.ConstructUI();

			nameLabel = new Label
			{
				Style = (Style)App.Current.Resources["whiteTextLabel"],
				Text = "Name",
				HorizontalOptions = LayoutOptions.Start
			};
			shortDescriptionLabel = new Label
			{
				Style = (Style)App.Current.Resources["whiteTextLabel"],
				Text = "Short Description",
				HorizontalOptions = LayoutOptions.Start
			};
			priceLabel = new Label
			{
				Style = (Style)App.Current.Resources["whiteTextLabel"],
				Text = "Price",
				HorizontalOptions = LayoutOptions.Start
			};
			dateLabel = new Label
			{
				Style = (Style)App.Current.Resources["whiteTextLabel"],
				Text = "Date of Expense",
				HorizontalOptions = LayoutOptions.Start
			};

			nameEntry = new Entry
			{
				Style = (Style)App.Current.Resources["underlinedEntry"],
				AutomationId = "expenseNameEntry",
				HorizontalTextAlignment = TextAlignment.End,
				PlaceholderColor = Color.FromHex("#E8E8E8"),
				Placeholder = "Vendor Name"
			};
			shortDescriptionEntry = new Entry
			{
				Style = (Style)App.Current.Resources["underlinedEntry"],
				AutomationId = "shortDescriptionEntry",
				HorizontalTextAlignment = TextAlignment.End
			};
			priceEntry = new Entry
			{
				Style = (Style)App.Current.Resources["underlinedEntry"],
				AutomationId = "priceEntry",
				HorizontalTextAlignment = TextAlignment.End,
				Keyboard = Keyboard.Numeric,
				PlaceholderColor = Color.FromHex("#E8E8E8"),
				Placeholder = "$0.00"
			};
			date = new ExpenseDatePicker
			{
				AutomationId = "expenseDatePicker",
				HorizontalOptions = LayoutOptions.End
			};

			saveButton = new Button
			{
				Style = (Style)App.Current.Resources["borderedButton"],
				AutomationId = "saveExpenseButton",
				Text = "Save",
				VerticalOptions = LayoutOptions.End
			};
			cancelButton = new Button
			{
				Style = (Style)App.Current.Resources["borderedButton"],
				AutomationId = "cancelExpenseButton",
				Text = "Cancel",
				VerticalOptions = LayoutOptions.EndAndExpand
			};
			addReceipt = new Image
			{
				AutomationId = "addReceiptButton",
				Source = "ic_insert_photo_white_48dp.png"
			};

			formLayout = new RelativeLayout();
		}

		public override void AddChildrenToParentLayout()
		{
			base.AddChildrenToParentLayout();

			Func<RelativeLayout, double> getCancelButtonHeight = (p) => cancelButton.Measure(formLayout.Width, formLayout.Height).Request.Height;

			formLayout.Children.Add(
				nameLabel,
				xConstraint: Constraint.Constant(20),
				yConstraint: Constraint.Constant(20)
			);
			formLayout.Children.Add(
				nameEntry,
				xConstraint: Constraint.Constant(20),
				yConstraint: Constraint.RelativeToView(nameLabel, (p, v) => v.Y + v.Height - 10),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - 40)
			);
			formLayout.Children.Add(
				priceLabel,
				xConstraint: Constraint.Constant(20),
				yConstraint: Constraint.RelativeToView(nameEntry, (p, v) => v.Y + v.Height + 10)
			);
			formLayout.Children.Add(
				priceEntry,
				xConstraint: Constraint.Constant(20),
				yConstraint: Constraint.RelativeToView(priceLabel, (p, v) => v.Y + v.Height - 10),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - 40)
			);
			formLayout.Children.Add(
				shortDescriptionLabel,
				xConstraint: Constraint.Constant(20),
				yConstraint: Constraint.RelativeToView(priceEntry, (p, v) => v.Y + v.Height + 10)
			);
			formLayout.Children.Add(
				shortDescriptionEntry,
				xConstraint: Constraint.Constant(20),
				yConstraint: Constraint.RelativeToView(shortDescriptionLabel, (p, v) => v.Y + v.Height),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - 40)
			);
			formLayout.Children.Add(
				dateLabel,
				xConstraint: Constraint.Constant(20),
				yConstraint: Constraint.RelativeToView(shortDescriptionEntry, (p, v) => v.Y + v.Height + 10),
				widthConstraint: (Constraint.RelativeToParent(p => p.Width - 40))
			);
			formLayout.Children.Add(
				date,
				xConstraint: Constraint.Constant(20),
				yConstraint: Constraint.RelativeToView(dateLabel, (p, v) => v.Y + v.Height),
				widthConstraint: (Constraint.RelativeToParent(p => p.Width - 40))
			);
			formLayout.Children.Add(
				addReceipt,
				xConstraint: (Constraint.Constant(20)),
				yConstraint: Constraint.RelativeToView(date, (p, v) => v.Y + v.Height + 5),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - 40),
				heightConstraint: Constraint.RelativeToView(date, (p, v) => p.Height - v.Y - v.Height - 3 * getCancelButtonHeight(p) - 30)
			);

			formLayout.Children.Add(
				cancelButton,
				xConstraint: Constraint.Constant(10),
				yConstraint: Constraint.RelativeToParent(p => p.Height - getCancelButtonHeight(p) - 10),
				widthConstraint: Constraint.RelativeToParent(p => p.Width - 20)
			);
		}

		public override void AddConditionalUI()
		{
			base.AddConditionalUI();

			if (ViewModel.Expense != null)
			{
				if (ViewModel.IsEditable)
				{
					Button deleteButton = new Button
					{
						Style = (Style)App.Current.Resources["borderedButton"],
						AutomationId = "expenseDeleteButton",
						Text = "Delete"
					};

					formLayout.Children.Add(deleteButton,
						xConstraint: Constraint.Constant(10),
						yConstraint: Constraint.RelativeToView(cancelButton, (p, v) => v.Y - 50),
						widthConstraint: Constraint.RelativeToParent(p => p.Width - 20)
					);

					formLayout.Children.Add(saveButton,
						xConstraint: Constraint.Constant(10),
						yConstraint: Constraint.RelativeToView(deleteButton, (p, v) => v.Y - 50),
						widthConstraint: Constraint.RelativeToParent(p => p.Width - 20)
					);

					deleteButton.Clicked += (object sender, EventArgs e) =>
					{
						ViewModel.Delete();
						Navigation.PopModalAsync();
					};
				}
				else {
					nameEntry.IsEnabled = false;
					priceEntry.IsEnabled = false;
					shortDescriptionEntry.IsEnabled = false;
					date.IsEnabled = false;
					addReceipt.IsEnabled = false;
				}

			}
			else {
				formLayout.Children.Add(saveButton,
					xConstraint: Constraint.Constant(10),
					yConstraint: Constraint.RelativeToView(cancelButton, (p, v) => v.Y - 50),
					widthConstraint: Constraint.RelativeToParent(p => p.Width - 20)
				);
			}
		}

		#endregion

		protected override void OnAppearing()
		{
			base.OnAppearing();

			if (!String.IsNullOrEmpty(ViewModel.Expense.ExpenseId))
			{
				var pictureFileName = DependencyService.Get<IPhoto>().GetPictureFromDisk(ViewModel.Expense.ExpenseId);
				var exists = DependencyService.Get<IPhoto>().CheckIfExists(pictureFileName);
				if (!String.IsNullOrEmpty(pictureFileName) && exists)
					addReceipt.Source = ImageSource.FromFile(pictureFileName);
			}
		}
	}
}
using System;

using Xamarin.Forms;

using MyExpenses.Pages;

namespace MyExpenses.Views
{
	public class ReportViewCell : ViewCell
	{
		public ReportViewCell ()
		{
			Label nameLabel = new Label { Style = (Style)App.Current.Resources ["whiteTextLabel"] };
			Label statusLabel = new Label { Style = (Style)App.Current.Resources ["whiteTextLabel"], Font = Font.SystemFontOfSize(NamedSize.Small), TextColor = Color.FromRgb(200,200,200) };
			Label totlaPriceLabel = new Label { Style = (Style)App.Current.Resources ["whiteTextLabel"] };
			Label createdOnLabel = new Label { Style = (Style)App.Current.Resources ["whiteTextLabel"], Font = Font.SystemFontOfSize(NamedSize.Micro), TextColor = Color.FromRgb(200,200,200) };

			AbsoluteLayout layout = new AbsoluteLayout { Padding = new Thickness (20,5,0,0), HeightRequest = 50 };
			layout.Children.Add(nameLabel,new Rectangle(0.05,0.05,0.8,0.5));
			layout.Children.Add(statusLabel,new Rectangle(0.05,0.95,0.55,0.5));
			layout.Children.Add(totlaPriceLabel,new Rectangle(1,0.05,0.25,0.6));
			layout.Children.Add(createdOnLabel,new Rectangle(1,0.95,0.45,0.4));

			AbsoluteLayout.SetLayoutFlags(nameLabel,AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutFlags(statusLabel,AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutFlags(totlaPriceLabel,AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutFlags(createdOnLabel,AbsoluteLayoutFlags.All);

			nameLabel.SetBinding(Label.TextProperty,"ReportName");
			statusLabel.SetBinding(Label.TextProperty, "StatusString");
			totlaPriceLabel.SetBinding(Label.TextProperty, "Total",BindingMode.TwoWay,null,"$ {0}"); 
			createdOnLabel.SetBinding(Label.TextProperty, "CreatedOnString");

			var deleteItem = new MenuItem { Text = "Delete", IsDestructive = true };
			deleteItem.SetBinding (MenuItem.CommandProperty, ".");
			deleteItem.SetBinding (MenuItem.CommandParameterProperty,"ID");
			deleteItem.Clicked += OnDelete;

			ContextActions.Add (deleteItem);
			View = layout;
		}

		void OnDelete (object sender, EventArgs e) {
			var page = this.Parent.Parent as ReportsPage;
			var item = (MenuItem)sender;

			page.ViewModel.DeleteItem ((int)item.CommandParameter);
		}
	}
}
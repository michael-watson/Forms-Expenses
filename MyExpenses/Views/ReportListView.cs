using System;

using Xamarin.Forms;

namespace MyExpenses.Views
{
	public class ReportListView : ListView
	{
		public ReportListView ()
		{
			BackgroundColor = Color.Transparent;
			HasUnevenRows = true;
			ItemTemplate = new DataTemplate (typeof(ReportViewCell));
		}
	}
}
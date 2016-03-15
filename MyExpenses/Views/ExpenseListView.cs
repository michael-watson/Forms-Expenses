using System;
using System.Collections.Generic;

using Xamarin.Forms;

using MyExpenses.Models;

namespace MyExpenses.Views
{
	public class ExpenseListView : ListView
	{
		public ExpenseListView ()
		{
			BackgroundColor = Color.Transparent;
			HasUnevenRows = true;
			ItemTemplate = new DataTemplate(typeof(ExpenseViewCell));
		}
	}
}
using System;

using Xamarin.Forms;

namespace MyExpenses.Views
{
	public class ExpenseDatePicker : DatePicker
	{
		public ExpenseDatePicker ()
		{
			Format = "M/d/yyyy";
			HeightRequest = 40;
		}
	}
}
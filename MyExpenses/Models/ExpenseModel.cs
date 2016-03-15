using System;

using SQLite;

using MyExpenses.Databases;

namespace MyExpenses.Models
{
	public class ExpenseModel
	{
		public ExpenseModel ()
		{
			Date = DateTime.Now;
		}

		public ExpenseModel (string id)
		{
			ExpenseReportIdentifier = id;
			Date = DateTime.Now;
		}

		public ExpenseModel (string name, string description, double price, DateTime date)
		{
			Name = name;
			ShortDescription = description;
			Price = price;
			Date = date;
		}

		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }

		public string ExpenseId { get; set; }

		public string Name { get; set; }

		public double Price { get; set; }

		public string ShortDescription { get; set; }

		public string ExpenseReportIdentifier { get; set; }

		public DateTime Date { get; set; }

		public string ReceiptLocation { get; set; }

		#region Formatted Properties

		public string FormattedDate {
			get {
				return String.Format ("{0:M/d/yyyy}", Date);
			}
		}

		public string FormattedPrice { 
			get {
				return "$ " + Math.Round (Price, 2).ToString ("#.00");
			}
		}

		#endregion
	}
}
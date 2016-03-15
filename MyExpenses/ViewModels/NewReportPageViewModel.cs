using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin;
using Xamarin.Forms;

using MyExpenses.Models;
using MyExpenses.Databases;
using MyExpenses.Interfaces;

namespace MyExpenses.ViewModels
{
	public class NewReportPageViewModel : BaseViewModel
	{
		ReportDatabase reportDatabase;
		MyExpensesDatabase expenseDatabase;

		public ExpenseReport Report { get; set; }

		public NewReportPageViewModel ()
		{
			reportDatabase = new ReportDatabase ();
			expenseDatabase = new MyExpensesDatabase ();
			Report = new ExpenseReport {
				CreatedOn = String.Format ("{0:M/d/yyyy}", DateTime.Now),
				Total = "0.00",
				Status = Status.PendingSubmission,
			};
			Report.Expenses = new List<ExpenseModel> ();
			CreateReportID ();
		}

		public string ReportName {
			get { return Report.ReportName; }
			set {
				if (Report.ReportName == value)
					return;
				Report.ReportName = value;
				OnPropertyChanged ("ReportName");
			}
		}

		public string ReportTotal {
			get { 
				if (Report.Total.Contains ("$"))
					return Report.Total.Substring (2, Report.Total.Length - 2);
				return Report.Total;
			}
		}

		public string ReportStatus {
			get { 
				switch (Report.Status) {
				case Status.Approved:
					return "Approved";
				case Status.PendingApproval:
					return "Pending Approval";
				case Status.PendingSubmission:
					return "Draft";
				}
				return "";
			}
		}

		public ObservableCollection<ExpenseModel> Expenses {
			get { return new ObservableCollection<ExpenseModel> (Report.Expenses); }
			set {
				if (Report.Expenses == value)
					return;
				Report.Expenses = value;
				OnPropertyChanged ("Expenses");
			}
		}

		public void CancelReport ()
		{
			expenseDatabase.DeleteExpensesForReport (Report.ExpenseReportIdentifier);
			reportDatabase.DeleteReport (Report.ID);
			Insights.Track ("CancelReport");
		}

		public bool Save ()
		{
			if (String.IsNullOrEmpty (ReportName))
				return false;
			
			reportDatabase.SaveReport (Report);

			Insights.Track ("ExpenseReportCreated", new Dictionary<string, string> {
				{ "ID", Report.ExpenseReportIdentifier },
				{ "ExpenseReportName", Report.ReportName },
				{ "Status", Report.StatusString },
				{ "TotalPrice", Report.Total },
				{ "CreatedOn", Report.CreatedOnString }
			});

			if (Device.OS == TargetPlatform.iOS)
				DependencyService.Get<ISearchable> ().InsertOrUpdateReport (Report);
			
			return true;
		}

		void CreateReportID ()
		{

			string uniqueKey = "";
			bool success = false;

			while (!success) {
				var rand = new Random ();
				for (var i = 0; i < 10; i++)
					uniqueKey += rand.Next (0, 9);

				success = reportDatabase.CheckIdentifier (uniqueKey);
			}

			Report.ExpenseReportIdentifier = uniqueKey;

			Insights.Track ("NewReportCreated", new Dictionary<string, string> {
				{ "ExpenseReportIdenifier", Report.ExpenseReportIdentifier },
			});
		}

		public void GetReportExpenses ()
		{
			Expenses = new ObservableCollection<ExpenseModel> (expenseDatabase.GetExpensesForReport (Report.ExpenseReportIdentifier));

			double total = 0;
			foreach (var expense in Expenses) {
				total += expense.Price;
			}

			var split = total.ToString ().Split (new char[]{ '.' }, 2);

			if (split.Length == 1)
				Report.Total = total.ToString () + ".00";
			else {
				var decimals = split [1];
				if (decimals.Length == 1)
					Report.Total = total.ToString () + "0";
				else
					Report.Total = total.ToString ();
			}

			if (Expenses.Count != 0) {
				OnPropertyChanged ("ReportTotal");
				reportDatabase.UpdateReport (Report);
			}
		}
	}
}
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin;
using Xamarin.Forms;

using MyExpenses.Interfaces;
using MyExpenses.Models;
using MyExpenses.Databases;

namespace MyExpenses.ViewModels
{
	public class ReportDetailViewModel : BaseViewModel
	{
		ReportDatabase reportDatabase;
		MyExpensesDatabase expenseDatabase;

		public ReportDetailViewModel (ExpenseReport report)
		{
			nameEditImageSource = "ic_mode_edit_white_48pt.png";
			reportDatabase = new ReportDatabase ();
			expenseDatabase = new MyExpensesDatabase ();
			Report = report;
			Report.Expenses = new List<ExpenseModel> ();

		}

		public ExpenseReport Report { get; set; }

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

		string nameEditImageSource;

		public string NameEditImageSource {
			get { return nameEditImageSource; }
			set {
				if (nameEditImageSource == value)
					return;
				nameEditImageSource = value;
				OnPropertyChanged ("NameEditImageSource");
			}
		}

		bool isEditing;

		public bool IsEditing {
			get { return isEditing; }
			set {
				if (isEditing == value)
					return;
				isEditing = value;
				OnPropertyChanged ("IsEditing");
				OnPropertyChanged ("NameLabelTextColor");
			}
		}

		public void ToggleEdit (object sender, EventArgs e)
		{
			if (IsEditing) {
				IsEditing = false;
				NameEditImageSource = "ic_mode_edit_white_48pt.png";
			} else {
				IsEditing = true;
				NameEditImageSource = "ic_check_white_48pt.png";
			}
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

		public ObservableCollection<ExpenseModel> Expenses {
			get { return new ObservableCollection<ExpenseModel> (Report.Expenses); }
			set {
				if (Report.Expenses == value)
					return;
				Report.Expenses = value;
				OnPropertyChanged ("Expenses");
			}
		}
		//Expose observable collection for list

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

			OnPropertyChanged ("ReportTotal");
			reportDatabase.UpdateReport (Report);
		}

		public void SubmitReport ()
		{
			Report.Approver = "Ian Leatherbury";
			Report.Status = Status.PendingApproval;
			reportDatabase.UpdateReport (Report);

			Insights.Track ("ReportSubmitted", new Dictionary<string, string> {
				{ "ID", Report.ExpenseReportIdentifier },
				{ "ExpenseReportName", Report.ReportName },
				{ "Approver", Report.Approver },
				{ "Status", Report.StatusString },
				{ "TotalPrice", Report.Total },
				{ "CreatedOn", Report.CreatedOnString }
			});

			if (Device.OS == TargetPlatform.iOS)
				DependencyService.Get<ISearchable> ().InsertOrUpdateReport (Report);
		}

		public void RevokeSubmission ()
		{
			Report.Approver = "";
			Report.Status = Status.PendingSubmission;
			reportDatabase.UpdateReport (Report);
			OnPropertyChanged ("ReportStatus");

			Insights.Track ("ReportUnsubmitted", new Dictionary<string, string> {
				{ "ID", Report.ExpenseReportIdentifier },
				{ "ExpenseReportName", Report.ReportName },
				{ "Approver", Report.Approver },
				{ "Status", Report.StatusString },
				{ "TotalPrice", Report.Total },
				{ "CreatedOn", Report.CreatedOnString }
			});
		}

		public void DeleteReport ()
		{
			reportDatabase.DeleteReport (Report.ID);
			Insights.Track ("ReportDeleted", new Dictionary<string, string> {
				{ "ID", Report.ExpenseReportIdentifier },
				{ "ExpenseReportName", Report.ReportName },
				{ "Approver", Report.Approver },
				{ "Status", Report.StatusString },
				{ "TotalPrice", Report.Total },
				{ "CreatedOn", Report.CreatedOnString }
			});

			if (Device.OS == TargetPlatform.iOS)
				DependencyService.Get<ISearchable> ().RemoveReport (Report);
		}

		public void DeleteExpense (int id)
		{
			var expense = expenseDatabase.GetExpense (id);
			expenseDatabase.DeleteExpense (id);
			Insights.Track ("ExpenseReportDeleted", new Dictionary<string, string> {
				{ "ExpenseId", expense.ExpenseId },
				{ "ExpenseReportId", expense.ExpenseReportIdentifier },
				{ "ExpenseName", expense.Name },
				{ "Price", expense.FormattedPrice },
				{ "ShortDescription", expense.ShortDescription }
			});

			GetReportExpenses ();
		}
	}
}
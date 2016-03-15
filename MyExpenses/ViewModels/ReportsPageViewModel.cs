using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin;
using Xamarin.Forms;

using MyExpenses.Models;
using MyExpenses.Databases;
using MyExpenses.Interfaces;

namespace MyExpenses.ViewModels
{
	public class ReportsPageViewModel : BaseViewModel
	{
		ReportDatabase database;

		public ReportsPageViewModel ()
		{
			database = new ReportDatabase ();
		}

		ObservableCollection<ExpenseReport> reports;

		public ObservableCollection<ExpenseReport> Reports { 
			get { return reports; } 
			set {
				if (reports == value)
					return;
				reports = value;
				OnPropertyChanged ("Reports");
			}
		}

		public void FilterData (string filterType)
		{
			switch (filterType) {
			case "All":
				Reports = new ObservableCollection<ExpenseReport> (database.GetReports ());
				break;
			case "Pending Approval":
				Reports = new ObservableCollection<ExpenseReport> (database.GetPendingAprrovalReports ());
				break;
			case "Approved":
				Reports = new ObservableCollection<ExpenseReport> (database.GetApprovedReports ());
				break;
			case "Pending Submission":
				Reports = new ObservableCollection<ExpenseReport> (database.GetPendingSubmissionReports ());
				break;
			}
			Insights.Track ("ReportsFiltered", new Dictionary<string, string> {
				{ "FilterType", filterType },
			});
		}

		public void RefreshData ()
		{
			var source = (List<ExpenseReport>)database.GetReports ();

			//Update CoreSpotlight with non-approved reports
			if (Device.OS == TargetPlatform.iOS)
				foreach (var report in source)
					if (report.Status != Status.Approved)
						DependencyService.Get<ISearchable> ().InsertOrUpdateReport (report);
			
			Reports = new ObservableCollection<ExpenseReport> (source);
			Insights.Track ("DataRefresh");
		}

		public void DeleteItem (int id)
		{
			var Report = database.GetReport (id);

			Insights.Track ("ReportDeleted", new Dictionary<string, string> {
				{ "ID", Report.ExpenseReportIdentifier },
				{ "ExpenseReportName", Report.ReportName },
				{ "Approver", Report.Approver },
				{ "Status", Report.StatusString },
				{ "TotalPrice", Report.Total },
				{ "CreatedOn", Report.CreatedOnString }
			});
			database.DeleteReport (id);

			if (Device.OS == TargetPlatform.iOS)
				DependencyService.Get<ISearchable> ().RemoveReport (Report);
			
			RefreshData ();
		}
	}
}
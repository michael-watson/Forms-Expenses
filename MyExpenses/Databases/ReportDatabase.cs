using System;
using SQLite;
using Xamarin.Forms;
using MyExpenses.Models;
using MyExpenses.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace MyExpenses.Databases
{
	public class ReportDatabase
	{
		SQLiteConnection database;
		public ReportDatabase ()
		{
			database = DependencyService.Get<ISQLite> ().GetReportsConnection ();
			database.CreateTable<ExpenseReport>();
		}
		public bool CheckIdentifier(string id){
			var source = new List<ExpenseReport> (GetReports ());

			foreach (var report in source) 
				if (report.ExpenseReportIdentifier == id)
					return false;

			return true;
		}
		public IEnumerable<ExpenseReport> GetReports () {
			return (from i in database.Table<ExpenseReport>() select i).ToList();
		}
		public IEnumerable<ExpenseReport> GetApprovedReports()
		{
			return database.Query<ExpenseReport> ("SELECT * FROM [ExpenseReport] Where [Status] = 2");
		}
		public IEnumerable<ExpenseReport> GetPendingAprrovalReports()
		{
			return database.Query<ExpenseReport> ("SELECT * FROM [ExpenseReport] Where [Status] = 0");
		}
		public IEnumerable<ExpenseReport> GetPendingSubmissionReports()
		{
			return database.Query<ExpenseReport> ("SELECT * FROM [ExpenseReport] Where [Status] = 1");
		}
		public IEnumerable<ExpenseReport> GetReportByReportName(string reportName)
		{
			return database.Query<ExpenseReport> ("SELECT * FROM [ExpenseReport] Where [ReportName] = " + reportName);
		}
		public ExpenseReport GetReport (int id)
		{
			return database.Table<ExpenseReport>().FirstOrDefault(x => x.ID == id);
		}
		public int DeleteReport(int id)
		{
			return database.Delete<ExpenseReport>(id);
		}

		public void SaveReport(ExpenseReport expense)
		{
			database.Insert (expense);
		}
		public void UpdateReport(ExpenseReport expense)
		{
			database.InsertOrReplace (expense);
		}
	}
}
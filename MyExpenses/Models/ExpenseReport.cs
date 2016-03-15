using System;
using System.Collections.Generic;

using SQLite;

namespace MyExpenses.Models
{
	public enum Status
	{
		PendingApproval = 0,
		PendingSubmission = 1,
		Approved = 2,
	}

	public class ExpenseReport
	{
		public ExpenseReport ()
		{
		}

		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }

		public string ExpenseReportIdentifier { get; set; }

		[Ignore]
		public IEnumerable<ExpenseModel> Expenses { get; set; }

		public string CreatedOn { get; set; }

		public string ReportName { get; set; }

		public Status Status { get; set; }

		public string Approver { get; set; }

		public string Total { get; set; }

		#region Formatted Properties

		public string StatusString {
			get {
				return Status.ToString ();
			}
		}

		public string CreatedOnString {
			get { return "Created On: " + CreatedOn; }
		}

		#endregion
	}
}
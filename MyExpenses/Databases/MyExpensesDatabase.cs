using System;
using System.Linq;
using System.Collections.Generic;

using SQLite;

using Xamarin.Forms;

using MyExpenses.Models;
using MyExpenses.Interfaces;

namespace MyExpenses.Databases
{
	public class MyExpensesDatabase
	{
		SQLiteConnection database;

		public MyExpensesDatabase ()
		{
			database = DependencyService.Get<ISQLite> ().GetExpenseConnection ();
			database.CreateTable<ExpenseModel>();
		}
		public bool CheckIfIdentifierAvailable(string id){
			var source = new List<ExpenseModel> (GetExpenses ());

			foreach (var report in source) 
				if (report.ExpenseId == id)
					return false;

			return true;
		}
		public IEnumerable<ExpenseModel> GetExpensesForReport(string id) {
			var allExpenses = new List<ExpenseModel> (GetExpenses ());
			List<ExpenseModel> results = new List<ExpenseModel> ();

			foreach (var expense in allExpenses)
				if (expense.ExpenseReportIdentifier == id)
					results.Add (expense);
			
			return results;
		}

		public IEnumerable<ExpenseModel> GetExpenses () {
			return (from i in database.Table<ExpenseModel>() select i).ToList();
		}
		public ExpenseModel GetExpense (int id)
		{
			return database.Table<ExpenseModel>().FirstOrDefault(x => x.ID == id);
		}

		public void DeleteExpensesForReport(string id){
			var allExpenses = new List<ExpenseModel> (GetExpenses ());
			List<ExpenseModel> results = new List<ExpenseModel> ();

			foreach (var expense in allExpenses)
				if (expense.ExpenseReportIdentifier == id)
					results.Add (expense);

			if (results.Count > 0)
				foreach (var expense in results) {
					database.Delete<ExpenseModel> (expense.ID);
				}
		}

		public int DeleteExpense(int id)
		{
			return database.Delete<ExpenseModel>(id);
		}

		public void SaveExpense(ExpenseModel expense)
		{
			database.Insert (expense);
		}
		public void UpdateExpense(ExpenseModel expense)
		{
			database.InsertOrReplace (expense);
		}
	}
}
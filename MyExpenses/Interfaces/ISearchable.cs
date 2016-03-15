using System;
using MyExpenses.Models;

namespace MyExpenses.Interfaces
{
	public interface ISearchable
	{
		void InsertOrUpdateReport (ExpenseReport item);
		void RemoveReport (ExpenseReport item);
	}
}

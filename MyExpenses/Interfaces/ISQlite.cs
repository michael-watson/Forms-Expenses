using System;

using SQLite;

namespace MyExpenses.Interfaces
{
	public interface ISQLite
	{
		SQLiteConnection GetExpenseConnection ();
		SQLiteConnection GetReportsConnection ();
	}
}
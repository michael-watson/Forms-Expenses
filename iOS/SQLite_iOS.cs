using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Xamarin.Forms;

using Foundation;

using MyExpenses.iOS;
using MyExpenses.Models;
using MyExpenses.Interfaces;

[assembly: Dependency (typeof(SQLite_iOS))]

namespace MyExpenses.iOS
{
	public class SQLite_iOS : ISQLite
	{
		public SQLite.SQLiteConnection GetExpenseConnection ()
		{
			var sqliteFilename = "MyExpenses.db3";
			string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
			string libraryPath = Path.Combine (documentsPath, "..", "Library"); // Library folder
			var path = Path.Combine (libraryPath, sqliteFilename);

			if (!File.Exists (path)) {
				var existingDb = NSBundle.MainBundle.PathForResource ("MyExpenses", "db3");
				File.Copy (existingDb, path);
			}
			// Create the connection
			var conn = new SQLite.SQLiteConnection (path);
			// Return the database connection
			return conn;
		}

		public SQLite.SQLiteConnection GetReportsConnection ()
		{
			var sqliteFilename = "Reports.db3";
			string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
			string libraryPath = Path.Combine (documentsPath, "..", "Library"); // Library folder
			var path = Path.Combine (libraryPath, sqliteFilename);

			if (!File.Exists (path)) {
				var existingDb = NSBundle.MainBundle.PathForResource ("Reports", "db3");
				File.Copy (existingDb, path);
			}
			// Create the connection
			var conn = new SQLite.SQLiteConnection (path);
			// Return the database connection
			return conn;
		}
	}
}
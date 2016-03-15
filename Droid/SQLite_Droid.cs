using System;
using Xamarin.Forms;
using MyExpenses.Droid;
using MyExpenses.Interfaces;
using System.IO;
using Android.App;

[assembly: Dependency (typeof (SQLite_Droid))]

namespace MyExpenses.Droid
{
	public class SQLite_Droid : ISQLite
	{
		public SQLite_Droid ()
		{
		}

		public SQLite.SQLiteConnection GetExpenseConnection ()
		{
			var sqliteFilename = "MyExpenses.db3";
			string path = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			string dbPath = Path.Combine (path, sqliteFilename);

			if (!File.Exists (dbPath))
				CopyDatabaseIfNotExists (dbPath, sqliteFilename);
			// Create the connection
			var conn = new SQLite.SQLiteConnection(dbPath);
			// Return the database connection
			return conn;
		}

		public SQLite.SQLiteConnection GetReportsConnection ()
		{
			var sqliteFilename = "Reports.db3";
			string path = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			string dbPath = Path.Combine (path, sqliteFilename);

			if (!File.Exists (dbPath))
				CopyDatabaseIfNotExists (dbPath, sqliteFilename);

			// Create the connection
			var conn = new SQLite.SQLiteConnection(dbPath);
			// Return the database connection
			return conn;
		}

		private static void CopyDatabaseIfNotExists (string dbPath, string sqliteFilename)
		{
			using (var br = new BinaryReader (Android.App.Application.Context.Assets.Open (sqliteFilename))) {
				using (var bw = new BinaryWriter (new FileStream (dbPath, FileMode.Create))) {
					byte[] buffer = new byte[2048];
					int length = 0;
					while ((length = br.Read (buffer, 0, buffer.Length)) > 0) {
						bw.Write (buffer, 0, length);
					}
				}
			}
		}
	}
}
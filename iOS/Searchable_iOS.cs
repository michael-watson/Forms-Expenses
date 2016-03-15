using System;

using Xamarin.Forms;

using UIKit;
using CoreSpotlight;

using MyLoginUI.iOS;

using MyExpenses.iOS;
using MyExpenses.Models;
using MyExpenses.Interfaces;

[assembly:Dependency (typeof(Searchable_iOS))]

namespace MyExpenses.iOS
{
	public class Searchable_iOS : ISearchable
	{
		public void InsertOrUpdateReport (ExpenseReport item)
		{
			//Need to check if iOS 9 for CoreSpotlight
			if (UIDevice.CurrentDevice.CheckSystemVersion (9, 0)) {
				var attributes = new CSSearchableItemAttributeSet (itemContentType: "");
				attributes.Title = "Expense Report: " + item.ReportName;
				attributes.ContentDescription = 
				"Status: " + item.StatusString
				+ "\nTotal: " + item.Total
				+ "\nCreated On:  " + item.CreatedOnString;
			
				// Create item
				var itemToAdd = new CSSearchableItem (item.ID.ToString (), item.ExpenseReportIdentifier, attributes);

				// Index item
				CSSearchableIndex.DefaultSearchableIndex.Index (new CSSearchableItem[]{ itemToAdd }, (error) => {
					// Successful?
					if (error != null) {
						Console.WriteLine (error.LocalizedDescription);
					}
				});
			}
		}

		public void RemoveReport (ExpenseReport item)
		{ 
			//Need to check if iOS 9 for CoreSpotlight
			if (UIDevice.CurrentDevice.CheckSystemVersion (9, 0)) {
				CSSearchableIndex.DefaultSearchableIndex.Delete (new string[]{ item.ID.ToString (), item.ExpenseReportIdentifier }, (error) => {
					// Successful?
					if (error != null) {
						Console.WriteLine (error.LocalizedDescription);
					}
				});
			}
		}
	}
}
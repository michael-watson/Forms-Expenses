using System;

using Xamarin.Forms;

using MyLoginUI.Views;

using MyExpenses.ViewModels;
using MyExpenses.Views;
using MyExpenses.Models;

namespace MyExpenses.Pages
{
	public class NewReportPage : BasePage
	{
		Entry reportName;
		Label reportTotal, status, listHeader;
		Button saveReportButton, cancelReportButton;
		RelativeLayout dashboard;
		ExpenseListView expenseList;
		NewReportPageViewModel ViewModel;
		ToolbarItem addExpense;

		public NewReportPage ()
		{
			NavigationPage.SetTitleIcon (this, "icon.png");
			ViewModel = new NewReportPageViewModel ();
			BindingContext = ViewModel;
			Title = "New Report";

			Content = new StackLayout {
				Padding = new Thickness (0,0,0,10),
				Children = {
					dashboard,
					expenseList,
					saveReportButton,
					cancelReportButton
				}
			};

			#region Set Event Handlers and Bindings
			saveReportButton.Clicked += HandleSaveReport;
			cancelReportButton.Clicked += HandleCancelReport;
			addExpense.Clicked += HandleAddExpense;
			expenseList.ItemSelected += HandleItemSelected;

			expenseList.SetBinding (ListView.ItemsSourceProperty, "Expenses");
			reportName.SetBinding (Entry.TextProperty, "ReportName", BindingMode.TwoWay);
			reportTotal.SetBinding (Label.TextProperty, "ReportTotal",BindingMode.OneWay,null,"$ {0}");
			status.SetBinding (Label.TextProperty, "ReportStatus");
			#endregion
		}

		#region Create UI
		public override void ConstructUI (){
			dashboard = new RelativeLayout ();
			reportName = new Entry {
				Style = (Style)App.Current.Resources["underlinedEntry"],
				AutomationId = "reportNameEntry",
				FontSize = 20,
				Placeholder = "Report Name"
			};
			reportTotal = new Label { Style = (Style)App.Current.Resources ["whiteTextLabel"] };
			status = new Label { Style = (Style)App.Current.Resources ["whiteTextLabel"] };

			saveReportButton = new Button { 
				Style = (Style)App.Current.Resources["borderedButton"],
				AutomationId = "saveReportButton", 
				Text = "Save Report", 
				HorizontalOptions = LayoutOptions.CenterAndExpand 
			};
			cancelReportButton = new Button { 
				Style = (Style)App.Current.Resources["borderedButton"],
				AutomationId = "cancelButton", 
				Text = "Cancel", 
				HorizontalOptions = LayoutOptions.CenterAndExpand 
			};

			expenseList = new ExpenseListView { AutomationId = "expenseListView" };

			listHeader = new Label { Style = (Style)App.Current.Resources["whiteTextLabel"], Text = "Expenses" };
			expenseList.Header = new ContentView {
				Content = listHeader,
				Padding = new Thickness (10, 0, 0, 0)
			};

			addExpense = new ToolbarItem { AutomationId = "addExpense", Text = "Add Expense" };
		}

		public override void AddChildrenToParentLayout ()
		{
			base.AddChildrenToParentLayout ();

			Func<RelativeLayout, double> getReportTotalWidth = (p) => reportTotal.Measure(dashboard.Width, dashboard.Height).Request.Width;
			Func<RelativeLayout, double> getStatusWidth = (p) => status.Measure(dashboard.Width, dashboard.Height).Request.Width;

			dashboard.Children.Add(
				reportName,
				xConstraint: Constraint.Constant(10),
				yConstraint: Constraint.Constant(10),
				widthConstraint: Constraint.RelativeToParent(p=>p.Width * 0.6 - 10)
			);
			dashboard.Children.Add(
				reportTotal,
				xConstraint: Constraint.RelativeToParent(p=>p.Width - getReportTotalWidth(p) - 10),
				yConstraint: Constraint.Constant(10)
			);
			dashboard.Children.Add(
				status,
				xConstraint: Constraint.RelativeToParent(p=>p.Width - getStatusWidth(p) - 10),
				yConstraint: Constraint.RelativeToView (reportTotal,(p,v)=>v.Y + v.Height + 5)
			);

			ToolbarItems.Add (addExpense);
		}
		#endregion

		#region Event Handlers
		void HandleItemSelected(object sender, SelectedItemChangedEventArgs e) {
			var expense = e.SelectedItem as ExpenseModel;
			var editable = ViewModel.Report.Status == Status.PendingSubmission;
			Navigation.PushModalAsync(new ExpenseActionPage(expense,editable));
		}
		void HandleAddExpense(object sender, EventArgs e) {
			Navigation.PushModalAsync (new ExpenseActionPage (new ExpenseModel (ViewModel.Report.ExpenseReportIdentifier)));
		}
		async void HandleCancelReport(object sender, EventArgs e) {
			var delete = await DisplayAlert("Confirm", "Are you sure you want to delete this report? This can't be undone.","Yes","No");
			if(delete) {
				ViewModel.CancelReport();
				Navigation.PopAsync();
			}
		}
		async void HandleSaveReport(object sender, EventArgs e) {
			//Need to perform this check because of iOS auto-correct
			ViewModel.ReportName = reportName.Text;
			//Saves two reports for some reason, one blank. Created check in OnApeparing for ReportsPage to check for null reports and delete
			ViewModel.Save();
			Navigation.PopAsync ();
		}
		#endregion

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			ViewModel.GetReportExpenses ();
		}

		protected override void LayoutChildren (double x, double y, double width, double height)
		{
			saveReportButton.WidthRequest = width - 20;
			cancelReportButton.WidthRequest = width - 20;
			base.LayoutChildren (x, y, width, height);
		}
	}
}
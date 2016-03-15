using System;

namespace MyExpenses.ViewModels
{
	public class LoginPageViewModel : BaseViewModel
	{
		public LoginPageViewModel ()
		{
			touchIdSuccess = false;
		}

		bool touchIdSuccess;

		public bool TouchIdSuccess {
			get {
				return touchIdSuccess;
			}
			set {
				if (touchIdSuccess)
				if (touchIdSuccess == value)
					return;
				touchIdSuccess = value;
				OnPropertyChanged ("Test");
			}
		}
	}
}
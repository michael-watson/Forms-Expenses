using System;
using System.Threading.Tasks;

using MyExpenses.Pages;

namespace MyExpenses.Interfaces
{
	public interface ITouchId
	{
		void AuthenticateWithTouchId (LoginPage page);
		Task<bool> SetPasswordForUsername (string username, string password);
		Task<bool> CheckLogin (string username, string password);
		Task SaveUsername (string username);
		Task<string> GetSavedUsername ();
	}
}
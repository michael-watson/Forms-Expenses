using System;
using Xamarin.Forms;
using MyExpenses.Droid;
using MyExpenses.Interfaces;
using MyExpenses.Pages;
using Akavache;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Collections.Generic;

[assembly: Dependency (typeof(ITouchId_Droid))]

namespace MyExpenses.Droid
{
	public class ITouchId_Droid : ITouchId
	{
		public ITouchId_Droid ()
		{
		}

		public void AuthenticateWithTouchId (LoginPage page)
		{
		}

		public async Task<bool> SetPasswordForUsername (string username, string password)
		{
//			byte[] bytes = new byte[username.Length * sizeof(char)];
//			System.Buffer.BlockCopy(username.ToCharArray(), 0, bytes, 0, bytes.Length);
			await BlobCache.UserAccount.InsertObject ("username", username);

//			bytes = new byte[password.Length * sizeof(char)];
//			System.Buffer.BlockCopy(password.ToCharArray(), 0, bytes, 0, bytes.Length);
			await BlobCache.UserAccount.InsertObject ("password", password);

			return true;
		}

		public async Task<bool> CheckLogin (string username, string password)
		{
			string _username = null;
			string _password = null;

			try{
				_username = await BlobCache.UserAccount.GetObject<string>("username");
			} catch (Exception e) {
				Console.WriteLine (e.Message);
				Console.WriteLine ("No Username Stored");
				return false;
			}

			try{
				_password = await BlobCache.UserAccount.GetObject<string>("password");
			} catch (Exception e) {
				Console.WriteLine (e.Message);
				Console.WriteLine ("No Password Stored");
				return false;
			}

			try {
				IEnumerable<string> test = await BlobCache.UserAccount.GetAllKeys();
				var test1 = test;
			} catch (Exception e){

			}

			if (_username == null || _password == null)
				return false;

			if (password == _password &&
				username == _username.ToString())
			{
				return true;
			}

			return false;
		}

		public async Task SaveUsername (string username)
		{
			await BlobCache.UserAccount.InsertObject<string> ("username", username);
		}

		public async Task<string> GetSavedUsername ()
		{
			string username = null;
			try {
				username = await BlobCache.UserAccount.GetObject<string> ("username");
			} catch (Exception e){
				Console.WriteLine (e.Message);
				username = "Not Found";
			}

			return username;
		}
	}
}
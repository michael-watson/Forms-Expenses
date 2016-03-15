using System;

using Xamarin.Forms;

namespace MyExpenses.Views
{
	public class WhiteLabel : Label
	{
		public WhiteLabel ()
		{
			TextColor = Color.White;
			FontFamily = Device.OnPlatform (
				iOS: "AppleSDGothicNeo-Light",
				Android: "Droid Sans Mono",
				WinPhone: "Comic Sans MS");
		}
	}
}


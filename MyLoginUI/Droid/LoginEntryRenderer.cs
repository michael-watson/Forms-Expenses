using System;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using MyLoginUI.Views;
using MyLoginUI.Droid.Renderers;

using Android.Widget;
using Android.Graphics;

[assembly:ExportRenderer(typeof(LoginEntry),typeof(LoginEntryRenderer))]

namespace MyLoginUI.Droid.Renderers
{
	public class LoginEntryRenderer : EntryRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged (e);

			if (e.NewElement != null) {
				var droidEditText = Control as EditText;
				droidEditText.SetHintTextColor (Xamarin.Forms.Color.White.ToAndroid ());

                Typeface font = Typeface.Create("Droid Sans Mono", TypefaceStyle.Normal);
                droidEditText.Typeface = font;
            }
		}
	}
}
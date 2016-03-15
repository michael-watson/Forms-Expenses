using System;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using MyExpenses.Views;
using MyExpenses.iOS.Renderers;

using UIKit;
using CoreAnimation;
using CoreGraphics;

[assembly:ExportRenderer (typeof(ExpenseDatePicker), typeof(ExpenseDatePickerRenderer))]

namespace MyExpenses.iOS.Renderers
{
	public class ExpenseDatePickerRenderer : DatePickerRenderer
	{
		UITextField nativeElement;
		CALayer bottomBorder;
		bool isInitialized;

		protected override void OnElementChanged (ElementChangedEventArgs<DatePicker> e)
		{
			base.OnElementChanged (e);

			if (e.NewElement != null && !isInitialized) {
				nativeElement = Control as UITextField;
				nativeElement.Font = UIFont.FromName ("AppleSDGothicNeo-Light", 18);
				nativeElement.BorderStyle = UITextBorderStyle.None;
				nativeElement.TextColor = Color.White.ToUIColor ();
				nativeElement.TextAlignment = UITextAlignment.Right;

				bottomBorder = new CALayer ();
				bottomBorder.BackgroundColor = UIColor.White.CGColor;
				nativeElement.Layer.AddSublayer (bottomBorder);

				isInitialized = true;
			}
		}

		public override CoreGraphics.CGSize SizeThatFits (CoreGraphics.CGSize size)
		{
			bottomBorder.Frame = new CGRect (0, 40 - 1, size.Width, 1);
			return base.SizeThatFits (size);
		}
	}
}
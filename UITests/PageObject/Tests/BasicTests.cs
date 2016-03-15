using NUnit.Framework;

using Xamarin.UITest;

namespace MyExpenses.UITests.PageObject.Tests
{
	public class BasicTests : AbstractSetup
	{
		public BasicTests (Platform platform)
			: base (platform)
		{
		}

		public override void BeforeEachTest ()
		{
			base.BeforeEachTest ();

			if (app is Xamarin.UITest.iOS.iOSApp) {
				app.Invoke ("xtcAgent:", "");
				app.Invoke ("clearKeyChain:", "");
			}
		}

		[Test]
		public void Repl ()
		{
			app.Repl ();
		}
	}
}
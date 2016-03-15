using System;

namespace MyExpenses.UITests
{
	public class SystemDialogButton
	{
		public El el { get; set; }

		public HitPoint hitPoint { get; set; }

		public Rect rect { get; set; }

		public string name { get; set; }

		public string label { get; set; }

	}

	public class El
	{
	}

	public class HitPoint
	{
		public double x { get; set; }

		public double y { get; set; }
	}

	public class Rect
	{
		public double x { get; set; }

		public double y { get; set; }

		public double width { get; set; }

		public double height { get; set; }
	}
}
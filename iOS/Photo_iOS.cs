using System;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using Foundation;

using Plugin.Media.Abstractions;

using MyExpenses.iOS;
using MyExpenses.Interfaces;

[assembly:Dependency (typeof(Photo_iOS))]

namespace MyExpenses.iOS
{
	public class Photo_iOS : IPhoto
	{
		public bool CheckIfExists (string file)
		{
			Console.WriteLine ("File Exists? " + File.Exists (file) + "\n" + file);
			return File.Exists (file);
		}

		public async void SavePictureToDisk (ImageSource imgSrc, string id)
		{
			var renderer = new StreamImagesourceHandler ();
			var photo = await renderer.LoadImageAsync (imgSrc);
			var documentsDirectory = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			string jpgFilename = System.IO.Path.Combine (documentsDirectory, id + ".jpg");
			NSData imgData = photo.AsJPEG ();
			NSError err = null;

			if (imgData.Save (jpgFilename, false, out err))
				Console.WriteLine ("saved as " + jpgFilename);
			else
				Console.WriteLine ("NOT saved as " + jpgFilename + " because" + err.LocalizedDescription);
		}

		public string GetPictureFromDisk (string id)
		{
			var documentsDirectory = Environment.GetFolderPath
				(Environment.SpecialFolder.Personal);
			string jpgFilename = System.IO.Path.Combine (documentsDirectory, id + ".jpg");
			return jpgFilename;
		}
	}
}
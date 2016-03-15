using System;
using System.IO;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace MyExpenses.Interfaces
{
	public interface IPhoto
	{
		bool CheckIfExists (string file);
		void SavePictureToDisk (ImageSource imgSrc, string Id);
		string GetPictureFromDisk (string id);
	}
}
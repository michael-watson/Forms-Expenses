##MyExpenses

For access to this application, please contact michael.watson@xamarin.com to be added.

MyExpenses is a Xamarin.Forms application that was designed to display a wide range of mobile techniques that create a end-to-end mobile solution. The following peices are leveraged in the application:

* Xamarin.Forms - iOS and Android applications available
	* iOS 9 features integrated into iOS applicaiton
		* [Force Touch shortcut](https://github.com/michael-watson/Forms-Expenses/blob/master/iOS/AppDelegate.cs#L54)
		* [Corespotlight](https://github.com/michael-watson/Forms-Expenses/blob/master/iOS/		AppDelegate.cs#L97)
		* [TouchId](https://github.com/michael-watson/Forms-Expenses/blob/master/iOS/TouchId_iOS.cs#L20)
	* [Media Plugin](https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/Media) is utilized for images
	* [Application Resources](https://github.com/michael-watson/Forms-Expenses/blob/master/Forms-Expenses/App.xaml) are utilized for base styling
* Xamarin.UITest
	* [PageObject Testing Suite](https://github.com/michael-watson/Forms-Expenses/tree/master/UITests/PageObject)
    * [Basic Test Suite](https://github.com/michael-watson/Forms-Expenses/tree/master/UITests/BasicTestSuite)
    * [AppInitializer](https://github.com/michael-watson/Forms-Expenses/blob/master/UITests/AppInitializer.cs) to enable cross-platform testing suite
    * [ExtensionMethods](https://github.com/michael-watson/Forms-Expenses/blob/master/UITests/ExtensionMethods.cs) utilized to help write tests 
    * [Reusable UI component](https://github.com/michael-watson/MyExpenses/tree/master/MyLoginUI) for LoginPage
        * [LoginPage](https://github.com/michael-watson/MyExpenses/blob/master/MyLoginUI/MyLoginUI/ReusableLoginPage.cs)
        * The idea behind this is that an organization can create standard login procedures and distribute the package to different development projects. This will keep the branding uniform and can potentially create a login component that only has to be maintained.


####Author
Michael Watson  
Xamarin.Forms Ninja  
##MyExpenses

For access to this application, please contact michael.watson@xamarin.com to be added.

MyExpenses is a Xamarin.Forms application that was designed to display a wide range of mobile techniques that create a end-to-end mobile solution. The following peices are leveraged in the application:

* Xamarin.Forms - iOS and Android applications available
	* iOS 9 features integrated into iOS applicaiton
		* [Force Touch shortcut](https://github.com/michael-watson/MyExpenses/blob/master/iOS/AppDelegate.cs#L54)
		* [Corespotlight](https://github.com/michael-watson/MyExpenses/blob/master/iOS/		AppDelegate.cs#L97)
		* [TouchId](https://github.com/michael-watson/MyExpenses/blob/master/iOS/TouchId_iOS.cs#L20)
	* [Media Plugin](https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/Media) is utilized for images
	* [Application Resources](https://github.com/michael-watson/MyExpenses/blob/master/MyExpenses/App.xaml) are utilized for base styling
* Xamarin.UITest
	* [PageObject Testing Suite](https://github.com/michael-watson/MyExpenses/tree/master/UITests/PageObject)
    * [Basic Test Suite](https://github.com/michael-watson/MyExpenses/tree/master/UITests/BasicTestSuite)
    * [AppInitializer](https://github.com/michael-watson/MyExpenses/blob/master/UITests/AppInitializer.cs) to enable cross-platform testing suite
    * [ExtensionMethods](https://github.com/michael-watson/MyExpenses/blob/master/UITests/ExtensionMethods.cs) utilized to help write tests
    * **Fixes found through Xamarin Test Cloud**
    	* [Initial Test Results](https://testcloud.xamarin.com/test/myexpenses_a6042d85-cb29-42bf-ba02-6c7a4bb1b3ee/)
    		* Notice the dashboard "Failures by category" hints that the issue is affecting only devices that are not iOS 9.
    		* [GitHub Issue for failures](https://github.com/michael-watson/MyExpenses/issues/3)
    		* [Fix 1](https://github.com/michael-watson/MyExpenses/blob/master/iOS/Searchable_iOS.cs#L22)
    		* [Fix 2](https://github.com/michael-watson/MyExpenses/blob/master/iOS/Searchable_iOS.cs#L46)
    		* [Test Results showing problem resolved](https://testcloud.xamarin.com/test/myexpenses_13957e5d-d0c4-4ffd-b89d-a83a9681eedc/)

* Xamarin.Insights	
	* [User identification](https://github.com/michael-watson/MyExpenses/blob/master/MyExpenses/Pages/LoginPage.cs#L51)
	* [Event Tracking Example](https://github.com/michael-watson/MyExpenses/blob/master/MyExpenses/ViewModels/ExpenseActionViewModel.cs#L127)
	* **Fixes found through Insights**
		*  [addExpense null exception](https://github.com/michael-watson/MyExpenses/blob/master/MyExpenses/Pages/ReportDetailPage.cs#L149)
			* [Insights Report](https://insights.xamarin.com/app/expenses-production/issues/41)
			* [GitHub Issue](https://github.com/michael-watson/MyExpenses/issues/2)
		* [LayoutChildren on ReportDetailPage null exception](https://github.com/michael-watson/MyExpenses/blob/master/MyExpenses/Pages/ReportDetailPage.cs#L222)
			* [Insights Report](https://insights.xamarin.com/app/expenses-production/issues/40)
			* [GitHub Issue](https://github.com/michael-watson/MyExpenses/issues/1)
* Continuous Integration through Bitrise
	* All code commits synched to GitHub will kick off a build and run both testing suites on a sub set of devices.
	* Currently working on master suite
	* **Android UITest inegration is to be completed**
* [Reusable UI component](https://github.com/michael-watson/MyExpenses/tree/master/MyLoginUI) for LoginPage
    * [LoginPage](https://github.com/michael-watson/MyExpenses/blob/master/MyLoginUI/MyLoginUI/ReusableLoginPage.cs)
    * The idea behind this is that an organization can create standard login procedures and distribute the package to different development projects. This will keep the branding uniform and can potentially create a login component that only has to be maintained.

The application was designed to be re-used and white-labeled for any company. This was originally done for Sage, but can be done for any company. All that needs to be done is changing the organization logo on the login page and the overall theme colors. Below are the steps to white-label application after forking from this branch:

All fonts are set to White and can be changed in the [application resources](https://github.com/michael-watson/MyExpenses/blob/master/MyExpenses/App.xaml)

* iOS
    * Remove 'xamarin_logo.png' from 'Resources' folder and replace with company image. You should include @1x, @2x and @3x images.
        * 1x size - 162 x 37
        * 2x size - 324 x 74
        * 3x size - 486 x 111
        * All images have a transparent background
    * [Modify 'LaunchScreen.xib' to change the image source](https://github.com/michael-watson/MyExpenses/blob/master/iOS/Resources/LaunchScreen.xib#L14) to your new image. 
        * Don't change the positioning of the image!!!
        * All constraints are aligned to match splash screen image location to the Xamarin.Forms login page
    * [Modify background color of 'LaunchScreen.xib'](https://github.com/michael-watson/MyExpenses/blob/master/iOS/Resources/LaunchScreen.xib#L18)    
    * [Change 'BackgroundColor' of BasePage](https://github.com/michael-watson/MyExpenses/blob/master/MyExpenses/Pages/BasePage.cs#L13)
    * Change images in AppIconSet
* Android
    * **To be completed**


####Author
Michael Watson  
Customer Success Engineer  
Xamarin.Forms Ninja  
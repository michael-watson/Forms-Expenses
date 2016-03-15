##MyExpenses Page Object Testing Suite
[Page Object Testing Documentation](https://developer.xamarin.com/guides/testcloud/calabash/xplat-best-practices/#Page_Objects)

Suite is split into 'Pages' and 'Tests' folders. This separates the Page Objects from the tests that incorporate them. The idea behind a Page Object suite is that each page and all items that can be interacted with are exposed through the object.

* All Page Objects will ingerit from [BasePage](https://github.com/michael-watson/MyExpenses/blob/master/UITests/PageObject/Pages/BasePage.cs)
 	* This provides a mechanism for determining whether the platoform is iOS or Android.
 	* This also includes a secondary constructor for traits
 * Using [LoginPage](https://github.com/michael-watson/MyExpenses/blob/master/UITests/PageObject/Pages/LoginPage.cs) as an example:
 	* Methods exposed interact with elements on the page
 		* [EnterUsername(string)](https://github.com/michael-watson/MyExpenses/blob/master/UITests/PageObject/Pages/LoginPage.cs#L32) will enter text into the username Entry
 		* [PressLoginButton()](https://github.com/michael-watson/MyExpenses/blob/master/UITests/PageObject/Pages/LoginPage.cs#L61) will press the login button on the page
 		* [LoginWithUsernamePassword(string,string)](https://github.com/michael-watson/MyExpenses/blob/master/UITests/PageObject/Pages/LoginPage.cs#L19) will utilize methods EnterUsername(string), EnterPassword(string) and PressLoginButton().
 			* It is best practice to design your objects in a some heirarchy 
 * Using [LoginTests](https://github.com/michael-watson/MyExpenses/blob/master/UITests/PageObject/Tests/LoginTests.cs) as an example:
 	* [Before each test](https://github.com/michael-watson/MyExpenses/blob/master/UITests/PageObject/Tests/LoginTests.cs#L25), backdoor methods ([iOS](https://github.com/michael-watson/MyExpenses/blob/master/iOS/AppDelegate.cs#L114), Android to be completed) are ran to disable TouchId and clear the keychain.
 		* All tests are categorized as "PageObject" so they can be uniquely identified in BitRise (CI setup)
 		* From here, our tests become easily readable and can be programmed by anyone with some knowledge of how the application should work. Below is an example of [logging in](https://github.com/michael-watson/MyExpenses/blob/master/UITests/PageObject/Tests/LoginTests.cs#L33):
 		
```
[Test]
[Category ("PageObject")]
public void CreateNewUserAndLogin ()
{
	var username = "Michael";
	var password = "test";
	
	app.Screenshot ("Application Start");
	
	var loginPage = new LoginPage (app, platform);
	loginPage.PressNewUserButton ();
	
	var newUserPage = new NewUserPage (app, platform);
	newUserPage.CreateNewUserWithPassword (username, password);
	
	loginPage.ClearUsername ();
	loginPage.LoginWithUsernamePassword (username, password);
	
	Assert.IsNotNull (app.Query (x => x.Marked ("loginPage")));
}
```

Doesn't that code look super clean???? 

It's so easy to read and it makes more sense to the average eye. I can see that I'm pressing a new user button and then creating a new user and a password that I define. We don't have to worry about the code it takes to perform that action because our developers have already created the objects for us. This is one of the better models to use when you have separate QA and development teams. 

Pros: 
 
1. Easily maintainable between separate QA and Development teams
2. Cross-platforms tests are easier to create because you can point to general page objects that represent one screen
3. Easily re-use code and can be maintained by developers without affecting QA tests developed
	* You would modify the page object as a developer, but the tests would still perform the same actions. This might need to be done if a control is being changed for appearance or architecture reasons.
 

Cons:  

1. More complicated setup that takes up-front time to architecture
2. Changes to tests will require multiple changes throughout the basic testing suite since code is duplicated
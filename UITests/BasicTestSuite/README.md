##MyExpenses Basic Testing Suite
The purpose of this testing suite is to show a contrast to the [Page Object model](https://github.com/michael-watson/MyExpenses/tree/master/UITests/PageObject). 

Pros: 
 
1. Easiest to setup
2. Directly correlates to tests built by Xamarin Test Recorder or developed through REPL
	* Page Object suite requires you to abstract out the code generated from these techniques into the Page Objects 
 

Cons:  

1. Code is repeated through tests
2. Changes to tests will require multiple changes throughout the basic testing suite since code is duplicated
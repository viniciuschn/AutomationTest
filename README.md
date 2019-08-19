# AutomationTest

This script is an automation test requested by DB Server to Vinicius Lopes.

## How to run this script

First, you will need to install Selenium tools on the machine:

1. [Selenium server](https://bit.ly/2TlkRyu)
2. [Firefox](https://github.com/mozilla/geckodriver/) and [Chrome](https://sites.google.com/a/chromium.org/chromedriver/) plugins
3. Firefox and Chrome browsers installed on the machine
4. [Visual Studio](https://visualstudio.microsoft.com/downloads/) (or any other .NET code editor/IDE)

After installing the needed tools, clone this repository to a local folder.

Open the [AppSettings.json](AutomationTest.Tests/AppSettings.json) file, and fill the necessary information for the tests. The location of the Firefox and Chrome drivers must be correctly defined before running tests. Also, fill the remote site URL which the tests will be running.

After that, do a dotnet restore and dotnet build to build the solution.

After building the solution, run tests on the test folder.

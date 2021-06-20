# TestProject OpenSDK For C#

[TestProject](https://testproject.io) is a **Free** Test Automation platform for Web, Mobile and API testing.
To get familiar with TestProject, visit our main [documentation](https://docs.testproject.io/) website.

The TestProject OpenSDK is a single, integrated interface to scripting with the most popular open source test automation frameworks.

From now on, you can effortlessly execute Selenium and Appium native tests using a single automation platform that already takes care of all the complex setup, maintenance and configs.

With one unified SDK available across multiple languages, developers and testers receive a go-to toolset, solving some of the greatest challenges in open source test automation.

With the TestProject OpenSDK, users save a bunch of time and enjoy the following benefits out of the box:

* 100% open source and available as a [NuGet](https://www.nuget.org/packages/TestProject.OpenSDK/) dependency.
* 5-minute simple Selenium and Appium setup with a single [Agent](https://docs.testproject.io/testproject-agents) deployment.
* Automatic test reports in HTML/PDF format (including screenshots). 
* Collaborative reporting dashboards with execution history and RESTful API support.
* Automatic distribution and deployment of test artifacts in case uploaded to the platform.
* Always up-to-date with the latest and stable Selenium driver version.
* A simplified, familiar syntax for both web and mobile applications.
* Complete test runner capabilities for both local and remote executions, anywhere.
* Cross platform support for Mac, Windows, Linux and Docker.
* Ability to store and execute tests locally on any source control tool, such as Git.

# Getting Started

To get started, you need to complete the following prerequisites checklist:

* Login to your account at https://app.testproject.io/ or [register](https://app.testproject.io/signup/) a new one.
* [Download](https://app.testproject.io/#/download) and install an Agent for your operating system or pull a container from [Docker Hub](https://hub.docker.com/r/testproject/agent).
* Run the Agent and [register](https://docs.testproject.io/getting-started/installation-and-setup#register-the-agent) it with your Account.
* Get a development token from [Integrations / SDK](https://app.testproject.io/#/integrations/sdk) page. 

## Installation

The TestProject C# OpenSDK is [available via NuGet](https://www.nuget.org/packages/TestProject.OpenSDK/).

> This OpenSDK supports .NET Standard 2.0 and newer.

# Test Development

Using a TestProject driver is identical to using a Selenium driver. Changing the import statement is enough in most cases.

> The following examples use the `ChromeDriver`, but they are applicable to all other supported drivers.

Here's an example of how to create a TestProject version of `ChromeDriver`:

```csharp
// using OpenQA.Selenium.Chrome; <-- Replaced
using TestProject.OpenSDK.Drivers.Web;
using ChromeOptions = OpenQA.Selenium.Chrome.ChromeOptions;

...

public class MyTest {
  ChromeDriver driver = new ChromeDriver(chromeOptions: new ChromeOptions());
}
```

# Drivers

The TestProject OpenSDK overrides standard Selenium/Appium drivers with extended functionality. Below is the packages structure containing all supported drivers:

```ascii
TestProject.OpenSDK.Drivers
├── Web
│   ├── ChromeDriver
│   ├── EdgeDriver
│   ├── FirefoxDriver
│   ├── InternetExplorerDriver
│   ├── SafariDriver
│   └── RemoteWebDriver
├── Android
│   └── AndroidDriver
├── iOS
│   └── IOSDriver
├── Generic
│   └── GenericDriver
```

> The GenericDriver can be used to run non-UI tests and still report the results to TestProject.

## Development Token

The OpenSDK uses a development token for communication with the Agent and the TestProject platform. Drivers search the developer token in an environment variable `TP_DEV_TOKEN`. This token can be also provided explicitly using the constructor:

```csharp
ChromeDriver driver = new ChromeDriver(token: "your_token_goes_here");
```

> When a token is provided in both the constructor and an environment variable, the token in the environment variable will be used.

## Remote Agent

By default, drivers communicate with the local Agent listening on http://localhost:8585.

The Agent URL (host and port) can be also provided explicitly using this constructor:

```csharp
ChromeDriver driver = new ChromeDriver(remoteAddress: "your_address_and_port_go_here");
```

It can also be set using the `TP_AGENT_URL` environment variable.

**NOTE:** By default, the agent binds to localhost.
In order to allow the SDK to communicate with agents running on a remote machine (*On the same network*), the agent should bind to an external interface.
For additional documentation on how to achieve such, please refer [here](https://docs.testproject.io/testproject-agents/testproject-agent-cli#start)

## Driver Builder
The SDK provides a generic builder for the drivers - `DriverBuilder`, for example:

```csharp
var caps = new FireFoxOptions();
caps.AddArguments("--headless");
var driver = new DriverBuilder<FirefoxDriver>()
    .WithJobName("DriverBuilder Job")
    .WithProjectName("TestProject C# OpenSDK")
    .WithOptions(caps)
    .Build();
```

# Reports

The TestProject OpenSDK reports all driver commands and their results to the TestProject Cloud. Doing so allows us to present beautifully designed reports and statistics in its dashboards.

Reports can be completely disabled using this constructor:

```csharp
ChromeDriver driver = new ChromeDriver(disableReports: true);
```

## Implicit Project and Job Names

By default, the OpenSDK will attempt to infer Project and Job names when you're using NUnit, MSTest or XUnit as a testing framework.

If any of these unit testing frameworks is detected, the following reporting settings will be inferred:

* The project name will be equal to the final segment of the namespace that your test class is part of. For example, if your test class is in the `TestProject.OpenSDK.Example` namespace, the project name will be equal to `Example`.
* The job name will be equal to the name of your test class.
* The test name will be equal to the name of your test method.
  
Examples of implicit project and job names inferred from annotations:

* [MSTest example](https://github.com/testproject-io/csharp-opensdk/blob/main/TestProject.OpenSDK.Tests/Examples/Frameworks/MSTest/InferredReportTest.cs)
* [NUnit example](https://github.com/testproject-io/csharp-opensdk/blob/main/TestProject.OpenSDK.Tests/Examples/Frameworks/NUnit/InferredReportTest.cs)
* [XUnit example](https://github.com/testproject-io/csharp-opensdk/blob/main/TestProject.OpenSDK.Tests/Examples/Frameworks/XUnit/InferredReportTest.cs)

## Explicit Names

Project and job names can also be specified explicitly using this constructor:

```csharp
ChromeDriver driver = new ChromeDriver(projectName: "your_project_name", jobName: "your_job_name");
```

Examples of explicit project and job name configuration:

* [MSTest example](https://github.com/testproject-io/csharp-opensdk/blob/main/TestProject.OpenSDK.Tests/Examples/Frameworks/MSTest/ExplicitReportTest.cs)
* [NUnit example](https://github.com/testproject-io/csharp-opensdk/blob/main/TestProject.OpenSDK.Tests/Examples/Frameworks/NUnit/ExplicitReportTest.cs)
* [XUnit example](https://github.com/testproject-io/csharp-opensdk/blob/main/TestProject.OpenSDK.Tests/Examples/Frameworks/XUnit/ExplicitReportTest.cs)

## Tests Reports

### Automatic Tests Reporting

Tests are reported automatically when a test **ends** or when driver _quits_. This behavior can be overridden or disabled (see the [Disabling Reports](#disabling-reports) section below).

In order to determine whether a test has ended, the call stack is inspected, searching for the current test method.
When the test name is different from the latest known test name, it is concluded that the execution of the previous test has ended.
This is supported for MSTest, NUnit and XUnit.

### Manual Tests Reporting

To report tests manually, you can use the `driver.Report().Test()` method, for example:

```csharp
ChromeDriver driver = new ChromeDriver(chromeOptions: new ChromeOptions());
driver.Report().Test("My First Test");
```

> It is important to disable automatic tests reporting when using the manual option to avoid any collision.

### Steps

Steps are reported automatically when driver commands are executed. If this feature is disabled, or in addition, manual reports can be performed, for example:

```csharp
ChromeDriver driver = new ChromeDriver(chromeOptions: new ChromeOptions());
driver.Report().Step("User logged in successfully");
```

## Disabling Reports

If reports were **not** disabled when the driver was created, they can be disabled or enabled later. However, if reporting was explicitly disabled when the driver was created, it can **not** be enabled later.

### Disable all reports

This will disable all types of reports:

```csharp
ChromeDriver driver = new ChromeDriver(chromeOptions: new ChromeOptions());
driver.Report().DisableReports(true);
```

### Disable automatic test reports

This will disable automatic test reporting. All steps will end up in a single test report, unless tests are reported manually using `driver.Report().Test()`:

```csharp
ChromeDriver driver = new ChromeDriver(chromeOptions: new ChromeOptions());
driver.Report().DisableAutoTestReports(true);
```

### Disable driver command reports

This will disable driver _command_ reporting. There are three options here:

* `DisableCommandReports(DriverCommandsFilter.All)` disables the reporting of all driver commands.
* `DisableCommandReports(DriverCommandsFilter.Passing)` disables the reporting of passing driver commands. Failing driver commands will still be reported as manual steps, including a screenshot.
* `DisableCommandReports(DriverCommandsFilter.None)` reports all driver commands as normal.

For example, the following:

```csharp
ChromeDriver driver = new ChromeDriver(chromeOptions: new ChromeOptions());
driver.Report().DisableCommandReports(DriverCommandsFilter.All);
```

will result in a report with no steps, unless they are reported manually using `driver.Report().Step()`.

## Cloud and Local Report

By default, the execution report is uploaded to the cloud, and a local report is created, as an HTML file in a temporary folder.

At the end of execution, the report is uploaded to the cloud and SDK outputs to the console/terminal the path for a local report file:

`Execution Report: {temporary_folder}/report.html`

This behavior can be controlled, by requesting only a `LOCAL` or only a `CLOUD` report.

> When the Agent is offline, and only a _cloud_ report is requested, execution will fail with appropriate message.

Via a driver constructor:

```csharp
var driver = new ChromeDriver(chromeOptions: new ChromeOptions(), reportType: ReportType.LOCAL);
```

Via Driver Builder:

```csharp
var driver = new DriverBuilder<FirefoxDriver>()
    .WithJobName("DriverBuilder Job")
    .WithProjectName("TestProject C# OpenSDK")
    .WithReportType(ReportType.LOCAL)
    .Build();
```

## Control Path and Name of Local Reports

By default, the local reports name is the timestamp of the test execution, and the path is the reports directory in the agent data folder.

The SDK provides a way to override the default values of the generated local reports name and path.

Via driver constructor:

```csharp
var driver = new ChromeDriver(chromeOptions: new ChromeOptions(), reportName: "C# Local report", reportPath: "/my_executions/reports);
```

Via Driver Builder:

```csharp
var driver = new DriverBuilder<FirefoxDriver>()
    .WithLocalReportName("C# Local Report")
    .WithLocalReportPath("/my_executions/reports")
    .Build();
```


### Disable command redaction

When reporting driver commands, the OpenSDK performs redaction of sensitive data (values) sent to secured elements. If the element is one of the following:

* Any element with `type` attribute set to `password`
* With XCUITest, on iOS an element type of `XCUIElementTypeSecureTextField`

the values sent to these elements will be converted to three asterisks - `***`. This behavior can be disabled as follows:

```csharp
ChromeDriver driver = new ChromeDriver(chromeOptions: new ChromeOptions());
driver.Report().DisableRedaction(true);
```

# SpecFlow support

The OpenSDK also supports automatic reporting of SpecFlow features, scenarios and steps through the [TestProject OpenSDK SpecFlow plugin](https://www.nuget.org/packages/TestProject.OpenSDK.SpecFlowPlugin/).

After installing the plugin package using NuGet, SpecFlow-based scenarios that use an OpenSDK driver will be automatically reported to TestProject Cloud.

When the plugin detects that SpecFlow is used, it will disable the reporting of driver command and automatic reporting of tests.

Instead, it will report:

* A separate job for every feature file
* A test for every scenario in a feature file
* All steps in a scenario as steps in the corresponding test

Steps are automatically marked as passed or failed, and Scenario Outlines are supported to create comprehensive living documentation from your specifications on TestProject Cloud.

A working example project can be found [here](https://github.com/testproject-io/csharp-opensdk/tree/main/TestProject.OpenSDK.SpecFlowExamples). This project contains:

* [A feature with scenarios using desktop Chrome](https://github.com/testproject-io/csharp-opensdk/blob/main/TestProject.OpenSDK.SpecFlowExamples/Features/SpecFlowExample.feature)
* [A feature with a scenario outline using desktop Chrome](https://github.com/testproject-io/csharp-opensdk/blob/main/TestProject.OpenSDK.SpecFlowExamples/Features/SpecFlowScenarioOutlineExample.feature)
* [A feature with a scenario using a native Android app](https://github.com/testproject-io/csharp-opensdk/blob/main/TestProject.OpenSDK.SpecFlowExamples/Features/SpecFlowMobileExample.feature)

# Package & Upload Tests to TestProject

Tests can be executed locally using the SDK, or triggered remotely from the TestProject platform.  
Before uploading your Tests, they should be packaged into a ZIP file.

### Packaging

Packaging can be done in two ways:

**1. Using Visual Studio:**

* Right-click your solution in the *Solution Explorer* panel and select *Publish...*
* If this is your first time publishing, create a new publishing target:
  * Choose *Folder* as your publish target, and press *Next*.
  * Choose *Folder* again, and press *Next*.
  * Change the folder your code is built in to `out` and press *Finish*.
* Press *Publish* next to the *FolderProfile* target to create this folder.
* Right-click your solution in the *Solution Explorer* panel and select *Open Folder in File Explorer*.
* Zip the newly created `out` folder (The entire folder, not just the contents).

**2. Using terminal/PowerShell:**

* Open a terminal or powershell window in your solution folder.
* Run the following command: `dotnet publish -o out <your-solution-file>`
* Zip the newly created `out` folder (The entire folder, not just the contents).

Now your code is ready to be uploaded to TestProject's platform!

### Supported Frameworks

TestProject platform supports the upload and execution of NUnit, xUnit and MSTest testing frameworks.  
It also supports SpecFlow tests that use [OpenSDK's SpecFlow plugin](#specflow-support).

### Parameterizing Your Tests

TestProject platform supports uploading tests that can use custom parameters.
To do this, we use the new `TestProjectDataProvider` class. It supports both NUnit and xUnit. MSTest is not supported.

Here's some code examples how to create tests with the new `TestProjectDataProvider` class:

<details><summary>Nunit</summary>

```csharp
namespace ParameterizationExamples
{
    using NUnit.Framework;
    using OpenQA.Selenium;
    using TestProject.OpenSDK.DataProviders;
    using TestProject.OpenSDK.Drivers.Web;

    public class NUnitExample
    {
        [TestCaseSource(typeof(TestProjectDataProvider), nameof(TestProjectDataProvider.DataSource))]
        public void ExampleTest(string username, string password)
        {
            var driver = new ChromeDriver();  // Project and job names are inferred.
            driver.Navigate().GoToUrl("https://example.testproject.io");
            driver.FindElement(By.CssSelector("#name")).SendKeys("John Smith");
            driver.FindElement(By.CssSelector("#password")).SendKeys("12345");
            driver.FindElement(By.CssSelector("#login")).Click();

            Assert.IsTrue(driver.FindElement(By.CssSelector("#greetings")).Displayed);
            driver.Quit();
        }
    }
}
```

</details>

<details><summary>xUnit</summary>

```csharp
namespace ParameterizationExamples
{
    using OpenQA.Selenium;
    using TestProject.OpenSDK.DataProviders;
    using TestProject.OpenSDK.Drivers.Web;
    using Xunit;

    public class XUnitExample
    {
        [Theory]
        [ClassData(typeof(TestProjectDataProvider))]
        public void ExampleTest(string username, string password)
        {
            var driver = new ChromeDriver();  // Project and job names are inferred.
            driver.Navigate().GoToUrl("https://example.testproject.io");
            driver.FindElement(By.CssSelector("#name")).SendKeys("John Smith");
            driver.FindElement(By.CssSelector("#password")).SendKeys("12345");
            driver.FindElement(By.CssSelector("#login")).Click();

            Assert.True(driver.FindElement(By.CssSelector("#greetings")).Displayed);
            driver.Quit();
        }
    }
}
```

</details>

## Underlying Selenium Version

TestProject uses the latest Selenium Version *3.141.59* 

# Examples

More usage examples for the OpenSDK can be found [here](https://github.com/testproject-io/csharp-opensdk/tree/main/TestProject.OpenSDK.Tests/Examples):

* Drivers
  * Web
    * [Chrome Test](https://github.com/testproject-io/csharp-opensdk/blob/main/TestProject.OpenSDK.Tests/Examples/Drivers/ChromeDriverTest.cs)
    * [Edge Test](https://github.com/testproject-io/csharp-opensdk/blob/main/TestProject.OpenSDK.Tests/Examples/Drivers/EdgeDriverTest.cs)
    * [Firefox Test](https://github.com/testproject-io/csharp-opensdk/blob/main/TestProject.OpenSDK.Tests/Examples/Drivers/FirefoxDriverTest.cs)
    * [Internet Explorer Test](https://github.com/testproject-io/csharp-opensdk/blob/main/TestProject.OpenSDK.Tests/Examples/Drivers/InternetExplorerDriverTest.cs)
    * [Safari Test](https://github.com/testproject-io/csharp-opensdk/blob/main/TestProject.OpenSDK.Tests/Examples/Drivers/SafariDriverTest.cs)
    * [Remote Web Driver Test](https://github.com/testproject-io/csharp-opensdk/blob/main/TestProject.OpenSDK.Tests/Examples/Drivers/RemoteWebDriverTest.cs)
  * Android
    * [Android native test](https://github.com/testproject-io/csharp-opensdk/blob/main/TestProject.OpenSDK.Tests/Examples/Drivers/AndroidDriverTest.cs)
    * [Android native app](https://github.com/testproject-io/android-demo-app)
    * [Web test on mobile Chrome](https://github.com/testproject-io/csharp-opensdk/blob/main/TestProject.OpenSDK.Tests/Examples/Drivers/AndroidDriverChromeTest.cs)
  * iOS
    * [iOS native test](https://github.com/testproject-io/csharp-opensdk/blob/main/TestProject.OpenSDK.Tests/Examples/Drivers/IOSDriverTest.cs)
    * [iOS native app](https://github.com/testproject-io/ios-demo-app)
    * [Web test on mobile Safari](https://github.com/testproject-io/csharp-opensdk/blob/main/TestProject.OpenSDK.Tests/Examples/Drivers/IOSDriverSafariTest.cs)
  * Generic
    * [Generic Driver Test](https://github.com/testproject-io/csharp-opensdk/blob/main/TestProject.OpenSDK.Tests/Examples/Drivers/GenericDriverTest.cs)
* Frameworks
  * MSTest
    * [Inferred Report](https://github.com/testproject-io/csharp-opensdk/blob/main/TestProject.OpenSDK.Tests/Examples/Frameworks/MSTest/InferredReportTest.cs)
    * [Explicit Report](https://github.com/testproject-io/csharp-opensdk/blob/main/TestProject.OpenSDK.Tests/Examples/Frameworks/MSTest/ExplicitReportTest.cs)
  * NUnit
    * [Inferred Report](https://github.com/testproject-io/csharp-opensdk/blob/main/TestProject.OpenSDK.Tests/Examples/Frameworks/NUnit/InferredReportTest.cs)
    * [Explicit Report](https://github.com/testproject-io/csharp-opensdk/blob/main/TestProject.OpenSDK.Tests/Examples/Frameworks/NUnit/ExplicitReportTest.cs)
  * XUnit
    * [Inferred Report](https://github.com/testproject-io/csharp-opensdk/blob/main/TestProject.OpenSDK.Tests/Examples/Frameworks/XUnit/InferredReportTest.cs)
    * [Explicit Report](https://github.com/testproject-io/csharp-opensdk/blob/main/TestProject.OpenSDK.Tests/Examples/Frameworks/XUnit/ExplicitReportTest.cs)

# License

The TestProject OpenSDK For C# is licensed under the LICENSE file in the root directory of this source tree.

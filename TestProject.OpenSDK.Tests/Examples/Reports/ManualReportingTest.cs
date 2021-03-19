// <copyright file="ManualReportingTest.cs" company="TestProject">
// Copyright 2020 TestProject (https://testproject.io)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

namespace TestProject.OpenSDK.Tests.Examples.Reports
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OpenQA.Selenium;
    using TestProject.OpenSDK.Drivers.Web;
    using TestProject.OpenSDK.Enums;

    /// <summary>
    /// This class contains examples of using the TestProject C# SDK with manual reporting steps.
    /// </summary>
    [TestClass]
    public class ManualReportingTest
    {
        /// <summary>
        /// The TestProject ChromeDriver instance to be used in this test class.
        /// </summary>
        private ChromeDriver driver;

        /// <summary>
        /// Starts a Chrome browser with default ChromeOptions before each test.
        /// </summary>
        [TestInitialize]
        public void StartBrowser()
        {
            this.driver = new ChromeDriver(
                projectName: "Examples",
                jobName: "Manual reporting examples");

            // Disabling the automatic reporting of tests, so only manually reported tests will show up in the report.
            this.driver.Report().DisableAutoTestReports(true);

            // Disabling the reporting of all driver commands (both passed and failed ones),
            // so only manually reported steps will show up in the report.
            this.driver.Report().DisableCommandReports(DriverCommandsFilter.All);
        }

        /// <summary>
        /// An example test logging in to the TestProject demo application with Chome,
        /// and manually logging both a step and a test.
        /// </summary>
        [TestMethod]
        public void ExampleTestUsingChromeDriver()
        {
            this.driver.Navigate().GoToUrl("https://example.testproject.io");
            this.driver.FindElement(By.CssSelector("#name")).SendKeys("John Smith");
            this.driver.FindElement(By.CssSelector("#password")).SendKeys("12345");
            this.driver.FindElement(By.CssSelector("#login")).Click();

            this.driver.Report().Step("A passing step", "This demonstrates reporting a passing step with a screenshot", true, true);

            bool result = this.driver.FindElement(By.CssSelector("#greetings")).Displayed;

            this.driver.Report().Test("Passing test", result, "This demonstrates reporting a passing test");
        }

        /// <summary>
        /// Closes the browser and ends the development session after each test.
        /// </summary>
        [TestCleanup]
        public void CloseBrowser()
        {
            this.driver.Quit();
        }
    }
}

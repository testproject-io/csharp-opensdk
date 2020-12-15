// <copyright file="InternetExplorerDriverTest.cs" company="TestProject">
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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using TestProject.OpenSDK.Drivers.Web;

namespace TestProject.OpenSDK.Tests.Examples.Drivers
{
    /// <summary>
    /// This class contains examples of using the TestProject C# SDK with IE.
    /// </summary>
    [TestClass]
    public class InternetExplorerDriverTest
    {
        /// <summary>
        /// The TestProject InternetExplorerDriver instance to be used in this test class.
        /// </summary>
        private InternetExplorerDriver driver;

        /// <summary>
        /// Starts an IE browser with default InternetExplorerOptions before each test.
        /// </summary>
        [TestInitialize]
        public void StartBrowser()
        {
            OpenQA.Selenium.IE.InternetExplorerOptions internetExplorerOptions = new OpenQA.Selenium.IE.InternetExplorerOptions();
            internetExplorerOptions.PageLoadStrategy = PageLoadStrategy.Normal;
            internetExplorerOptions.UnhandledPromptBehavior = UnhandledPromptBehavior.DismissAndNotify;
            internetExplorerOptions.IntroduceInstabilityByIgnoringProtectedModeSettings = true;

            this.driver = new InternetExplorerDriver(
                internetExplorerOptions: internetExplorerOptions,
                projectName: "Examples",
                jobName: "Internet Explorer examples");
        }

        /// <summary>
        /// An example test logging in to the TestProject demo application with IE.
        /// </summary>
        [TestMethod]
        public void ExampleTestUsingInternetExplorerDriver()
        {
            this.driver.Navigate().GoToUrl("https://example.testproject.io");
            this.driver.FindElement(By.CssSelector("#name")).SendKeys("John Smith");
            this.driver.FindElement(By.CssSelector("#password")).SendKeys("12345");
            this.driver.FindElement(By.CssSelector("#login")).Click();

            Assert.IsTrue(this.driver.FindElement(By.CssSelector("#greetings")).Displayed);
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

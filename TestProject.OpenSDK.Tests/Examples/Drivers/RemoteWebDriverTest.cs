// <copyright file="RemoteWebDriverTest.cs" company="TestProject">
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
    /// This class contains examples of using the TestProject C# SDK with a RemoteWebDriver.
    /// </summary>
    [TestClass]
    public class RemoteWebDriverTest
    {
        /// <summary>
        /// The TestProject RemoteWebDriver instance to be used in this test class.
        /// </summary>
        private RemoteWebDriver driver;

        /// <summary>
        /// An example test logging in to the TestProject demo application with Chrome (using RemoteWebDriver).
        /// </summary>
        [TestMethod]
        public void ExampleTestUsingRemoteWebDriverWithChromeOptions()
        {
            OpenQA.Selenium.Chrome.ChromeOptions chromeOptions = new OpenQA.Selenium.Chrome.ChromeOptions();
            chromeOptions.PageLoadStrategy = PageLoadStrategy.Normal;
            chromeOptions.UnhandledPromptBehavior = UnhandledPromptBehavior.DismissAndNotify;

            this.driver = new RemoteWebDriver(
                driverOptions: chromeOptions,
                projectName: "My project",
                jobName: "My job",
                token: "aqqm_o3T_egvYLkI1eum8LV10IsHu-tKO3cRbJP6qW81");

            this.driver.Navigate().GoToUrl("https://example.testproject.io");
            this.driver.FindElement(By.CssSelector("#name")).SendKeys("John Smith");
            this.driver.FindElement(By.CssSelector("#password")).SendKeys("12345");
            this.driver.FindElement(By.CssSelector("#login")).Click();

            Assert.IsTrue(this.driver.FindElement(By.CssSelector("#greetings")).Displayed);
        }

        /// <summary>
        /// An example test logging in to the TestProject demo application with Firefox (using RemoteWebDriver).
        /// </summary>
        [TestMethod]
        public void ExampleTestUsingRemoteWebDriverWithFirefoxOptions()
        {
            OpenQA.Selenium.Firefox.FirefoxOptions firefoxOptions = new OpenQA.Selenium.Firefox.FirefoxOptions();
            firefoxOptions.PageLoadStrategy = PageLoadStrategy.Normal;
            firefoxOptions.UnhandledPromptBehavior = UnhandledPromptBehavior.DismissAndNotify;

            this.driver = new RemoteWebDriver(
                driverOptions: firefoxOptions,
                projectName: "My project",
                jobName: "My job",
                token: "aqqm_o3T_egvYLkI1eum8LV10IsHu-tKO3cRbJP6qW81");

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

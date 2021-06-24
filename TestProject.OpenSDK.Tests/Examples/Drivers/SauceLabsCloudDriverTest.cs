// <copyright file="SauceLabsCloudDriverTest.cs" company="TestProject">
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

namespace TestProject.OpenSDK.Tests.Examples.Drivers
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Remote;
    using TestProject.OpenSDK.Drivers;
    using ChromeDriver = TestProject.OpenSDK.Drivers.Web.ChromeDriver;
    using ChromeOptions = OpenQA.Selenium.Chrome.ChromeOptions;

    /// <summary>
    /// This class contains examples of using the TestProject C# SDK with BrowserStack.
    /// </summary>
    [TestClass]
    public class SauceLabsCloudDriverTest
    {
        /// <summary>
        /// The TestProject ChromeDriver instance to be used in this test class.
        /// </summary>
        private ChromeDriver driver;

        private string sauceUsername = System.Environment.GetEnvironmentVariable("SAUCE_USERNAME");

        private string sauceAccessKey = System.Environment.GetEnvironmentVariable("SAUCE_PASSWORD");

        /// <summary>
        /// Starts a Chrome browser with a custom cloud:URL capability.
        /// </summary>
        [TestInitialize]
        public void StartBrowser()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddAdditionalCapability(CapabilityType.Version, "latest", true);
            chromeOptions.AddAdditionalCapability(CapabilityType.Platform, "Windows 10", true);
            chromeOptions.AddAdditionalCapability("username", this.sauceUsername, true);
            chromeOptions.AddAdditionalCapability("accessKey", this.sauceAccessKey, true);
            chromeOptions.AddAdditionalCapability(TestProjectCapabilityType.CLOUD_URL, "https://ondemand.saucelabs.com/wd/hub", true);
            this.driver = new ChromeDriver(chromeOptions: chromeOptions);
        }

        /// <summary>
        /// An example test logging in to the TestProject demo application with Chrome.
        /// </summary>
        [TestMethod]
        public void ExampleTestUsingChromeDriver()
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

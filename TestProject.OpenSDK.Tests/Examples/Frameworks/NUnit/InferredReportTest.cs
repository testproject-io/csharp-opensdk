// <copyright file="InferredReportTest.cs" company="TestProject">
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

namespace TestProject.OpenSDK.Tests.Examples.Frameworks.NUnit
{
    using global::NUnit.Framework;
    using OpenQA.Selenium;
    using TestProject.OpenSDK.Drivers.Web;

    /// <summary>
    /// This class contains examples of using the TestProject C# SDK with NUnit and inferred reporting.
    /// </summary>
    [TestFixture]
    public class InferredReportTest
    {
        /// <summary>
        /// The TestProject ChromeDriver instance to be used in this test class.
        /// </summary>
        private ChromeDriver driver;

        /// <summary>
        /// Starts a Chrome browser with default ChromeOptions before each test.
        /// </summary>
        [SetUp]
        public void StartBrowser()
        {
            this.driver = new ChromeDriver();  // Project and job names are inferred.
        }

        /// <summary>
        /// An example test logging in to the TestProject demo application with Chrome.
        /// </summary>
        [Test]
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
        [TearDown]
        public void CloseBrowser()
        {
            this.driver.Quit();
        }
    }
}

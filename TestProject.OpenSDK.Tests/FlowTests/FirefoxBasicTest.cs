// <copyright file="FirefoxBasicTest.cs" company="TestProject">
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

namespace TestProject.OpenSDK.Tests.FlowTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TestProject.OpenSDK.Drivers.Web;
    using TestProject.OpenSDK.Tests.FlowTests.PageObjects;

    /// <summary>
    /// This class contains examples of using the TestProject C# SDK with Firefox.
    /// </summary>
    [TestClass]
    public class FirefoxBasicTest
    {
        /// <summary>
        /// The TestProject FirefoxDriver instance to be used in this test class.
        /// </summary>
        private FirefoxDriver driver;

        /// <summary>
        /// Starts a headless Firefox browser before each test.
        /// </summary>
        [TestInitialize]
        public void StartBrowser()
        {
            OpenQA.Selenium.Firefox.FirefoxOptions firefoxOptions = new OpenQA.Selenium.Firefox.FirefoxOptions();
            firefoxOptions.AddArgument("-headless");
            this.driver = new FirefoxDriver(firefoxOptions: firefoxOptions, projectName: "CI - C#");
        }

        /// <summary>
        /// An example test logging in to the TestProject demo application with Firefox.
        /// </summary>
        [TestMethod]
        public void ExampleTestUsingFirefoxDriver()
        {
            new LoginPage(this.driver).LoginAs("John Smith", "12345");

            Assert.IsTrue(new ProfilePage(this.driver).GreetingsAreDisplayed());
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

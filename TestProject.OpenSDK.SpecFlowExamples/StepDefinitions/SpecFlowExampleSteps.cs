// <copyright file="SpecFlowExampleSteps.cs" company="TestProject">
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

namespace TestProject.OpenSDK.SpecFlowExamples.StepDefinitions
{
    using NUnit.Framework;
    using OpenQA.Selenium;
    using TechTalk.SpecFlow;
    using TestProject.OpenSDK.Drivers.Web;

    /// <summary>
    /// This class contains the step definitions corresponding to the steps in the example scenario.
    /// </summary>
    [Binding]
    public class SpecFlowExampleSteps
    {
        /// <summary>
        /// The TestProject ChromeDriver instance to be used in this test class.
        /// </summary>
        private ChromeDriver driver;

        /// <summary>
        /// Start a new browser session before each scenario.
        /// </summary>
        [Before]
        public void StartBrowser()
        {
            this.driver = new ChromeDriver();
        }

        /// <summary>
        /// Implementation of the 'Given' step.
        /// </summary>
        /// <param name="firstName">The first name of the user wanting to log in to the TestProject demo application.</param>
        [Given(@"(.+) wants to use the TestProject demo application")]
        public void GivenWantsToUseTheTestProjectDemoApplication(string firstName)
        {
            this.driver.Navigate().GoToUrl("https://example.testproject.io");
        }

        /// <summary>
        /// Implementation of the 'When' step.
        /// </summary>
        /// <param name="username">The username to be provided when logging in.</param>
        /// <param name="password">The password to be provided when logging in.</param>
        [When(@"s?he logs in with username (.+) and password (.+)")]
        public void WhenTheyLogInWithUserNameAndPassword(string username, string password)
        {
            this.driver.FindElement(By.CssSelector("#name")).SendKeys(username);
            this.driver.FindElement(By.CssSelector("#password")).SendKeys(password);
            this.driver.FindElement(By.CssSelector("#login")).Click();
        }

        /// <summary>
        /// Implementation of the 'Then' step.
        /// </summary>
        [Then(@"s?he gains access to the secure part of the application")]
        public void ThenTheyGainAccessToTheSecurePartOfTheApplication()
        {
            Assert.IsTrue(this.driver.FindElement(By.CssSelector("#greetings")).Displayed);
        }

        /// <summary>
        /// Close the browser after each scenario.
        /// </summary>
        [After]
        public void CloseBrowser()
        {
            this.driver.Quit();
        }
    }
}

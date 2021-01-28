// <copyright file="LoginPage.cs" company="TestProject">
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

namespace TestProject.OpenSDK.Tests.FlowTests.PageObjects
{
    using OpenQA.Selenium;
    using TestProject.OpenSDK.Drivers;

    /// <summary>
    /// Page Object representing the TestProject demo application login page.
    /// </summary>
    public class LoginPage
    {
        private string loginPageUrl = "https://example.testproject.io";
        private By textfieldUsername = By.CssSelector("#name");
        private By textfieldPassword = By.CssSelector("#password");
        private By buttonDoLogin = By.CssSelector("#login");

        private BaseDriver driver;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPage"/> class.
        /// </summary>
        /// <param name="driver">The current <see cref="BaseDriver"/> instance to use.</param>
        public LoginPage(BaseDriver driver)
        {
            this.driver = driver;
        }

        /// <summary>
        /// Login using the specified credentials.
        /// </summary>
        /// <param name="username">The username to log in with.</param>
        /// <param name="password">The password corresponding to the specified username.</param>
        public void LoginAs(string username, string password)
        {
            this.driver.Navigate().GoToUrl(this.loginPageUrl);
            this.driver.FindElement(this.textfieldUsername).SendKeys(username);
            this.driver.FindElement(this.textfieldPassword).SendKeys(password);
            System.Threading.Thread.Sleep(500);
            this.driver.FindElement(this.buttonDoLogin).Click();
        }
    }
}

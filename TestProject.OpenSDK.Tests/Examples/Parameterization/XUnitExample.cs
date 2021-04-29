// <copyright file="XUnitExample.cs" company="TestProject">
// Copyright 2021 TestProject (https://testproject.io)
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

namespace TestProject.OpenSDK.Tests.Examples.Parameterization
{
    using OpenQA.Selenium;
    using TestProject.OpenSDK.DataProviders;
    using TestProject.OpenSDK.Drivers.Web;
    using Xunit;

    /// <summary>
    /// Examples for XUnit parameterized tests.
    /// </summary>
    public class XUnitExample
    {
        /// <summary>
        /// Example parameterized test.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        [Theory]
        [ClassData(typeof(TestProjectDataProvider))]
        public void ExampleTest(string username, string password)
        {
            var driver = new ChromeDriver(); // Project and job names are inferred.
            driver.Navigate().GoToUrl("https://example.testproject.io");
            driver.FindElement(By.CssSelector("#name")).SendKeys(username);
            driver.FindElement(By.CssSelector("#password")).SendKeys(password);
            driver.FindElement(By.CssSelector("#login")).Click();

            Assert.True(driver.FindElement(By.CssSelector("#greetings")).Displayed);
            driver.Quit();
        }
    }
}

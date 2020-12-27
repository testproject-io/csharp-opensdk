// <copyright file="ExplicitReportTest.cs" company="TestProject">
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

using OpenQA.Selenium;
using TestProject.OpenSDK.Drivers.Web;
using Xunit;

namespace TestProject.OpenSDK.Tests.Examples.Frameworks.XUnit
{
    /// <summary>
    /// This class contains examples of using the TestProject C# SDK with XUnit and explicit reporting.
    /// </summary>
    public class ExplicitReportTest
    {
        /// <summary>
        /// An example test logging in to the TestProject demo application with Chrome.
        /// </summary>
        [Fact]
        public void ExampleTestUsingChromeDriver()
        {
            ChromeDriver driver = new ChromeDriver(
                projectName: "Examples",
                jobName: "XUnit example");

            driver.Navigate().GoToUrl("https://example.testproject.io");
            driver.FindElement(By.CssSelector("#name")).SendKeys("John Smith");
            driver.FindElement(By.CssSelector("#password")).SendKeys("12345");
            driver.FindElement(By.CssSelector("#login")).Click();

            Assert.True(driver.FindElement(By.CssSelector("#greetings")).Displayed);

            driver.Quit();
        }
    }
}

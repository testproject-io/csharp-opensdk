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

namespace TestProject.OpenSDK.Tests.Examples.Frameworks.XUnit
{
    using OpenQA.Selenium;
    using TestProject.OpenSDK.Drivers.Web;
    using TestProject.OpenSDK.XUnitLogger;
    using Xunit;
    using Xunit.Abstractions;

    /// <summary>
    /// This class contains examples of using the TestProject C# SDK with XUnit and inferred reporting.
    /// Note: It is important to inherit from the XUnitLogger base class in order to see the logs in the console.
    /// </summary>
    public class InferredReportTest : XUnitLogger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InferredReportTest"/> class.
        /// Init's the base class which configures the NLoger to work with XUnit.
        /// </summary>
        /// <param name="outputHelper">XUnit's output helper which allows capturing output.</param>
        public InferredReportTest(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        /// <summary>
        /// An example test logging in to the TestProject demo application with Chrome.
        /// </summary>
        [Fact(DisplayName = "XUnit Example Using Chrome Driver")]
        public void ExampleTestUsingChromeDriver()
        {
            ChromeDriver driver = new ChromeDriver();  // Project and job names are inferred.

            driver.Navigate().GoToUrl("https://example.testproject.io");
            driver.FindElement(By.CssSelector("#name")).SendKeys("John Smith");
            driver.FindElement(By.CssSelector("#password")).SendKeys("12345");
            driver.FindElement(By.CssSelector("#login")).Click();

            Assert.True(driver.FindElement(By.CssSelector("#greetings")).Displayed);

            driver.Quit();
        }
    }
}

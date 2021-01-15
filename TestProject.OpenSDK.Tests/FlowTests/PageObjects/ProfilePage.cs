// <copyright file="ProfilePage.cs" company="TestProject">
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
    using TestProject.OpenSDK.Tests.FlowTests.Helpers;

    /// <summary>
    /// Page Object representing the TestProject demo application profile page.
    /// </summary>
    public class ProfilePage
    {
        private readonly By textlabelGreetings = By.CssSelector("#greetings");

        private BaseDriver driver;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfilePage"/> class.
        /// </summary>
        /// <param name="driver">The current <see cref="BaseDriver"/> instance to use.</param>
        public ProfilePage(BaseDriver driver)
        {
            this.driver = driver;
        }

        /// <summary>
        /// Method to check whether the 'greetings' textlabel is displayed.
        /// </summary>
        /// <returns>True if the element is displayed, false otherwise.</returns>
        public bool GreetingsAreDisplayed()
        {
            return WaitHelpers.IsDisplayed(this.driver, this.textlabelGreetings);
        }
    }
}

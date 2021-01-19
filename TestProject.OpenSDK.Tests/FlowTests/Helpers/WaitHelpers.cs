// <copyright file="WaitHelpers.cs" company="TestProject">
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

namespace TestProject.OpenSDK.Tests.FlowTests.Helpers
{
    using System;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using TestProject.OpenSDK.Drivers;

    /// <summary>
    /// Helper methods for proper element synchronization in Selenium.
    /// </summary>
    public static class WaitHelpers
    {
        private static readonly TimeSpan TIMEOUT = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Helper method to determine whether an element is displayed within the given timeout.
        /// </summary>
        /// <param name="driver">The current driver in use.</param>
        /// <param name="element">The element to check.</param>
        /// <returns>True if element is displayed within the default timeout, false otherwise.</returns>
        public static bool IsDisplayed(BaseDriver driver, By element)
        {
            WebDriverWait wait = new WebDriverWait(driver, TIMEOUT);

            return wait.Until(d =>
            {
                try
                {
                    IWebElement tempElement = driver.FindElement(element);
                    return tempElement.Displayed;
                }
                catch
                {
                    return false;
                }
            });
        }
    }
}

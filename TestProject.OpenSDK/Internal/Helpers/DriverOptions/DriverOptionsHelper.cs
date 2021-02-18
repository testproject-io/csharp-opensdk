// <copyright file="DriverOptionsHelper.cs" company="TestProject">
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

namespace TestProject.OpenSDK.Internal.Helpers.DriverOptions
{
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Edge;
    using OpenQA.Selenium.Firefox;
    using OpenQA.Selenium.IE;
    using OpenQA.Selenium.Safari;
    using TestProject.OpenSDK.Exceptions;

    /// <summary>
    /// Provides utility methods to patch <see cref="DriverOptions"/> objects to make them suitable to start a session with the Agent.
    /// </summary>
    public static class DriverOptionsHelper
    {
        /// <summary>
        /// Patches <see cref="DriverOptions"/> objects make them suitable to start a session with the Agent.
        /// </summary>
        /// <param name="originalOptions">The original <see cref="DriverOptions"/> as specified by the user.</param>
        /// <param name="browserType">The type of browser in use (required to return the right type of options).</param>
        /// <returns>A patched <see cref="DriverOptions"/> object suitable to start a session with the Agent.</returns>
        public static OpenQA.Selenium.DriverOptions Patch(OpenQA.Selenium.DriverOptions originalOptions, BrowserType browserType)
        {
            if (originalOptions == null)
            {
                return CreateDefaultOptionsFor(browserType);
            }

            // Patch PageLoadStrategy if necessary, the Agent does not understand the 'Default' value
            if (originalOptions.PageLoadStrategy.Equals(OpenQA.Selenium.PageLoadStrategy.Default))
            {
                originalOptions.PageLoadStrategy = OpenQA.Selenium.PageLoadStrategy.Normal;
            }

            // Patch UnhandledPromptBehavior if necessary, the Agent does not understand the 'Default' value
            if (originalOptions.UnhandledPromptBehavior.Equals(OpenQA.Selenium.UnhandledPromptBehavior.Default))
            {
                originalOptions.UnhandledPromptBehavior = OpenQA.Selenium.UnhandledPromptBehavior.DismissAndNotify;
            }

            return originalOptions;
        }

        /// <summary>
        /// Creates default <see cref="DriverOptions"/> for the specified <see cref="BrowserType"/>.
        /// </summary>
        /// <param name="browserType">The type of browser in use.</param>
        /// <returns>Default <see cref="DriverOptions"/> suitable for starting a new session with the Agent.</returns>
        private static OpenQA.Selenium.DriverOptions CreateDefaultOptionsFor(BrowserType browserType)
        {
            OpenQA.Selenium.DriverOptions options;

            // Create default options for all supported drivers.
            // RemoteWebDriver options will never be null, so they do not have to be created from scratch.
            switch (browserType)
            {
                case BrowserType.Chrome:
                    options = new ChromeOptions();
                    break;
                case BrowserType.Firefox:
                    options = new FirefoxOptions();
                    break;
                case BrowserType.InternetExplorer:
                    options = new InternetExplorerOptions();
                    break;
                case BrowserType.Edge:
                    options = new EdgeOptions();
                    break;
                case BrowserType.Safari:
                    options = new SafariOptions();
                    break;
                default:
                    throw new SdkException($"Unsupported BrowserType {browserType} specified.");
            }

            // Set default values for properties
            options.PageLoadStrategy = OpenQA.Selenium.PageLoadStrategy.Normal;
            options.UnhandledPromptBehavior = OpenQA.Selenium.UnhandledPromptBehavior.DismissAndNotify;

            return options;
        }
    }
}

// <copyright file="DriverOptionsHelperTest.cs" company="TestProject">
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

namespace TestProject.OpenSDK.Tests.UnitTests.Internal.Helpers.DriverOptions
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TestProject.OpenSDK.Internal.Helpers.DriverOptions;

    /// <summary>
    /// Class containing unit tests for the <see cref="DriverOptionsHelper"/> class.
    /// </summary>
    [TestClass]
    public class DriverOptionsHelperTest
    {
        /// <summary>
        /// When the user does not explicitly specify options when creating a driver session,
        /// a default <see cref="DriverOptions"/> object should be created and returned.
        /// </summary>
        /// <param name="browserType">The type of browser for which <see cref="DriverOptions"/> should be created.</param>
        /// <param name="expectedType">The expected object type returned by the Patch method.</param>
        [DataTestMethod]
        [DataRow(BrowserType.Chrome, typeof(OpenQA.Selenium.Chrome.ChromeOptions))]
        [DataRow(BrowserType.Firefox, typeof(OpenQA.Selenium.Firefox.FirefoxOptions))]
        [DataRow(BrowserType.InternetExplorer, typeof(OpenQA.Selenium.IE.InternetExplorerOptions))]
        [DataRow(BrowserType.Edge, typeof(OpenQA.Selenium.Edge.EdgeOptions))]
        [DataRow(BrowserType.Safari, typeof(OpenQA.Selenium.Safari.SafariOptions))]
        public void Patch_WithNullOptions_ShouldReturnCorrectDriverOptionsType(BrowserType browserType, Type expectedType)
        {
            OpenQA.Selenium.DriverOptions options = DriverOptionsHelper.Patch(null, browserType);

            Assert.IsTrue(options.GetType().Equals(expectedType));
            Assert.AreEqual(options.PageLoadStrategy, OpenQA.Selenium.PageLoadStrategy.Normal);
            Assert.AreEqual(options.UnhandledPromptBehavior, OpenQA.Selenium.UnhandledPromptBehavior.DismissAndNotify);
        }

        /// <summary>
        /// Options where PageLoadStrategy and UnhandledPromptBehavior are explicitly set should return unchanged.
        /// </summary>
        [TestMethod]
        public void Patch_WithExistingOptions_ShouldReturnUnchangedOptions()
        {
            OpenQA.Selenium.Chrome.ChromeOptions originalOptions = new OpenQA.Selenium.Chrome.ChromeOptions();
            originalOptions.PageLoadStrategy = OpenQA.Selenium.PageLoadStrategy.Eager;
            originalOptions.UnhandledPromptBehavior = OpenQA.Selenium.UnhandledPromptBehavior.AcceptAndNotify;

            OpenQA.Selenium.DriverOptions patchedOptions = DriverOptionsHelper.Patch(originalOptions, BrowserType.Chrome);

            Assert.AreEqual(originalOptions, patchedOptions);
        }

        /// <summary>
        /// Options where PageLoadStrategy is set to 'Default' should return with PageLoadStrategy 'Normal'.
        /// </summary>
        [TestMethod]
        public void Patch_WithDefaultPageLoadStrategy_ShouldReturnWithPageLoadStrategyNormal()
        {
            OpenQA.Selenium.Chrome.ChromeOptions originalOptions = new OpenQA.Selenium.Chrome.ChromeOptions();
            originalOptions.UnhandledPromptBehavior = OpenQA.Selenium.UnhandledPromptBehavior.DismissAndNotify;

            OpenQA.Selenium.DriverOptions patchedOptions = DriverOptionsHelper.Patch(originalOptions, BrowserType.Chrome);

            Assert.AreEqual(OpenQA.Selenium.PageLoadStrategy.Normal, patchedOptions.PageLoadStrategy);
        }

        /// <summary>
        /// Options where UnhandledPromptBehavior is set to 'Default' should return with UnhandledPromptBehavior 'DismissAndNotify'.
        /// </summary>
        [TestMethod]
        public void Patch_WithUnhandledPromptBehaviorDefault_ShouldReturnWithUnhandledPromptBehaviorDismissAndNotify()
        {
            OpenQA.Selenium.Chrome.ChromeOptions originalOptions = new OpenQA.Selenium.Chrome.ChromeOptions();
            originalOptions.PageLoadStrategy = OpenQA.Selenium.PageLoadStrategy.Normal;

            OpenQA.Selenium.DriverOptions patchedOptions = DriverOptionsHelper.Patch(originalOptions, BrowserType.Chrome);

            Assert.AreEqual(OpenQA.Selenium.UnhandledPromptBehavior.DismissAndNotify, patchedOptions.UnhandledPromptBehavior);
        }
    }
}

// <copyright file="IOSDriverTest.cs" company="TestProject">
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

namespace TestProject.OpenSDK.Tests.Examples.Drivers
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Appium;
    using OpenQA.Selenium.Appium.Enums;
    using OpenQA.Selenium.Remote;
    using TestProject.OpenSDK.Drivers.IOS;
    using TestProject.OpenSDK.Exceptions;

    /// <summary>
    /// This class contains examples of using the TestProject C# SDK with a native iOS app.
    /// </summary>
    [TestClass]
    public class IOSDriverTest
    {
        private readonly string deviceId = Environment.GetEnvironmentVariable("TP_IOS_DUT_UDID");

        private readonly string deviceName = Environment.GetEnvironmentVariable("TP_IOS_DUT_NAME");

        private readonly string bundleId = Environment.GetEnvironmentVariable("TP_IOS_AUT_BUNDLE_ID");

        /// <summary>
        /// The TestProject IOSDriver instance to be used in this test class.
        /// </summary>
        private IOSDriver<AppiumWebElement> driver;

        /// <summary>
        /// Starts an Android driver with required AppiumOptions before each test.
        /// </summary>
        [TestInitialize]
        public void StartDriver()
        {
            if (this.deviceId == null || this.deviceName == null || this.bundleId == null)
            {
                throw new SdkException("Not all required environment variables were set before the start of the test.");
            }

            AppiumOptions appiumOptions = new AppiumOptions();

            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, MobilePlatform.IOS);
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.Udid, this.deviceId);

            appiumOptions.AddAdditionalCapability(CapabilityType.BrowserName, string.Empty);
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, this.deviceName);
            appiumOptions.AddAdditionalCapability(IOSMobileCapabilityType.BundleId, this.bundleId);

            this.driver = new IOSDriver<AppiumWebElement>(appiumOptions: appiumOptions);
        }

        /// <summary>
        /// An example test logging in to the TestProject native demo application on iOS.
        /// </summary>
        [TestMethod]
        public void ExampleTestUsingIOSDriver()
        {
            this.driver.FindElement(By.Id("name")).SendKeys("John Smith");
            this.driver.FindElement(By.Id("password")).SendKeys("12345");
            this.driver.FindElement(By.Id("login")).Click();

            Assert.IsTrue(this.driver.FindElement(By.Id("greetings")).Displayed);
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

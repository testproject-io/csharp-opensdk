// <copyright file="SpecFlowExampleMobileSteps.cs" company="TestProject">
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
    using System;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Appium;
    using OpenQA.Selenium.Appium.Enums;
    using OpenQA.Selenium.Remote;
    using TechTalk.SpecFlow;
    using TestProject.OpenSDK.Drivers.Android;
    using TestProject.OpenSDK.Exceptions;

    /// <summary>
    /// This class contains the step definitions corresponding to the steps in the example mobile scenario.
    /// </summary>
    [Binding]
    public class SpecFlowExampleMobileSteps
    {
        private readonly string dutUdid = Environment.GetEnvironmentVariable("TP_ANDROID_DUT_UDID");

        private readonly string autPackageName = Environment.GetEnvironmentVariable("TP_ANDROID_AUT_PACKAGE");

        private readonly string autActivityName = Environment.GetEnvironmentVariable("TP_ANDROID_AUT_ACTIVITY");

        /// <summary>
        /// The TestProject driver instance to be used in this test class.
        /// </summary>
        private AndroidDriver<AppiumWebElement> driver;

        /// <summary>
        /// Start a new Android browser session before each mobile scenario.
        /// </summary>
        [Before("mobile")]
        public void StartMobileBrowser()
        {
            if (this.autActivityName == null || this.autPackageName == null || this.dutUdid == null)
            {
                throw new SdkException("Not all required environment variables were set before the start of the test.");
            }

            AppiumOptions appiumOptions = new AppiumOptions();

            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, MobilePlatform.Android);
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.Udid, this.dutUdid);

            appiumOptions.AddAdditionalCapability(CapabilityType.BrowserName, string.Empty);
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, "Pixel_3a_API_29_x86 [emulator-5554]");
            appiumOptions.AddAdditionalCapability(AndroidMobileCapabilityType.AppPackage, this.autPackageName);
            appiumOptions.AddAdditionalCapability(AndroidMobileCapabilityType.AppActivity, this.autActivityName);

            this.driver = new AndroidDriver<AppiumWebElement>(appiumOptions: appiumOptions);
        }

        /// <summary>
        /// Implementation of the 'Given' step.
        /// </summary>
        /// <param name="firstName">The first name of the user wanting to log in to the TestProject demo application.</param>
        [Given(@"(.+) wants to use the TestProject demo application")]
        [Scope(Tag = "mobile")]
        public void GivenWantsToUseTheTestProjectDemoApplication(string firstName)
        {
            this.driver.StartActivity(appPackage: this.autPackageName, appActivity: this.autActivityName);
        }

        /// <summary>
        /// Implementation of the 'When' step.
        /// </summary>
        /// <param name="username">The username to be provided when logging in.</param>
        /// <param name="password">The password to be provided when logging in.</param>
        [When(@"s?he logs in with username (.+) and password (.+)")]
        [Scope(Tag = "mobile")]
        public void WhenTheyLogInWithUserNameAndPassword(string username, string password)
        {
            this.driver.FindElement(By.Id("name")).SendKeys(username);
            this.driver.FindElement(By.Id("password")).SendKeys(password);
            this.driver.FindElement(By.Id("login")).Click();
        }

        /// <summary>
        /// Implementation of the 'Then' step.
        /// </summary>
        [Then(@"s?he gains access to the secure part of the application")]
        [Scope(Tag = "mobile")]
        public void ThenTheyGainAccessToTheSecurePartOfTheApplication()
        {
            Assert.IsTrue(this.driver.FindElement(By.Id("greetings")).Displayed);
        }

        /// <summary>
        /// Close the browser after each scenario.
        /// </summary>
        [After("mobile")]
        public void CloseBrowser()
        {
            this.driver?.Quit();
        }
    }
}

// <copyright file="AssertionHelper.cs" company="TestProject">
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

namespace TestProject.OpenSDK.Internal.Helpers.Assertions
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NLog;
    using NUnit.Framework;
    using OpenQA.Selenium.Appium;
    using PostSharp.Aspects;
    using PostSharp.Serialization;
    using TestProject.OpenSDK.Drivers;
    using TestProject.OpenSDK.Drivers.Android;
    using TestProject.OpenSDK.Drivers.Generic;
    using TestProject.OpenSDK.Drivers.IOS;
    using TestProject.OpenSDK.Exceptions;
    using TestProject.OpenSDK.Internal.Reporting;
    using Xunit.Sdk;

    /// <summary>
    /// Extension method which provides an annotation for capturing assertion failures and reporting them.
    /// </summary>
    [PSerializable]
    public sealed class AssertionHelper : OnExceptionAspect
    {
        /// <summary>
        /// Screenshot flag, should a screenshot be taken or not.
        /// </summary>
        public bool Screenshot { get; set; } = true;

        /// <summary>
        /// Logger instance for this class.
        /// </summary>
        private static Logger Logger { get; set; } = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Instance of the reporter from the active driver.
        /// </summary>
        private Reporter Reporter { get; set; }

        /// <summary>
        /// Extenstion method which is invoked if the following annotation is on an class or method.
        /// <param name="eventArgs">The arguments from the invoking exception</param>
        /// </summary>
        public override void OnException(MethodExecutionArgs eventArgs)
        {
            Type exceptionType = eventArgs.Exception.GetType().BaseType;
            Logger.Debug($"Catched exception: {exceptionType}");

            // Only report the exception if its a failed assertion
            if (exceptionType == typeof(AssertActualExpectedException)
                || exceptionType == typeof(ResultStateException)
                || exceptionType == typeof(UnitTestAssertException))
            {
                Logger.Info($"Assertion Failure with type '{exceptionType}' catched");
                this.GetActiveDriverReporter();
                string message = eventArgs.Exception.Message;
                this.Reporter.Step(
                    description: "Assertion Failed",
                    message: $"Assertion Failed with message: {message}",
                    passed: false,
                    screenshot: this.Screenshot);
            }

            // Rethrow the original exception so the test will fail as required
            eventArgs.FlowBehavior = FlowBehavior.RethrowException;
        }

        /// <summary>
        /// Helper Method which scans all possible active drivers.
        /// </summary>
        private void GetActiveDriverReporter()
        {
            List<object> drivers = new List<object>
            {
                BaseDriver.GetInstance(),
                GenericDriver.GetInstance(),
                AndroidDriver<AppiumWebElement>.GetInstance(),
                IOSDriver<AppiumWebElement>.GetInstance(),
            };

            foreach (var driver in drivers)
            {
                if (driver != null)
                {
                    if (driver is IWebDriver currentDriver)
                    {
                        this.Reporter = currentDriver.Report();
                    }
                    else
                    {
                        this.Reporter = ((GenericDriver)driver).Report();
                    }

                    return;
                }
            }

            // If driver is null, there is no active driver session
            throw new SdkException("No active driver instance found for reporting");
        }
    }
}
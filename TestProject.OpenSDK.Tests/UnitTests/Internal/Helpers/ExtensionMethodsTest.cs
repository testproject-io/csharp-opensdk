// <copyright file="ExtensionMethodsTest.cs" company="TestProject">
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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Remote;
using TestProject.OpenSDK.Internal.Helpers;

namespace TestProject.OpenSDK.Tests.UnitTests.Internal.Helpers
{
    /// <summary>
    /// Class containing unit tests for the <see cref="ExtensionMethods"/> class.
    /// </summary>
    [TestClass]
    public class ExtensionMethodsTest
    {
        /// <summary>
        /// If the WebDriver command equals 'SendKeysToElement', the command should be patched.
        /// </summary>
        [TestMethod]
        public void ShouldBePatched_UsingSendKeysToElementCommand_ShouldReturnTrue()
        {
            string driverCommand = DriverCommand.SendKeysToElement;

            Assert.IsTrue(driverCommand.ShouldBePatched());
        }

        /// <summary>
        /// If the WebDriver command equals 'SendKeysToActiveElement', the command should be patched.
        /// </summary>
        [TestMethod]
        public void ShouldBePatched_UsingSendKeysToActiveCommand_ShouldReturnTrue()
        {
            string driverCommand = DriverCommand.SendKeysToActiveElement;

            Assert.IsTrue(driverCommand.ShouldBePatched());
        }

        /// <summary>
        /// If the WebDriver command equals neither 'SendKeysToElement' nor 'SendKeysToActiveElement',
        /// the command should NOT be patched.
        /// </summary>
        [TestMethod]
        public void ShouldBePatched_UsingClickElementCommand_ShouldReturnFalse()
        {
            string driverCommand = DriverCommand.ClickElement;

            Assert.IsFalse(driverCommand.ShouldBePatched());
        }
    }
}

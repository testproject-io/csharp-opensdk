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

namespace TestProject.OpenSDK.Tests.UnitTests.Internal.Helpers
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OpenQA.Selenium.Remote;
    using TestProject.OpenSDK.Internal.Helpers;

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

        /// <summary>
        /// If the host name of a Uri used to define the Agent remote address contains 'localhost',
        /// this should be updated to '127.0.0.1' to prevent reporting issues due to DNS lookup delays.
        /// </summary>
        [TestMethod]
        public void LocalhostTo127001_WithLocalhostInUri_ShouldReturnUpdatedUri()
        {
            Uri originalUri = new Uri("http://localhost:1234");

            Assert.AreEqual("http://127.0.0.1:1234/", originalUri.LocalhostTo127001().ToString());
        }

        /// <summary>
        /// Checking if the hostname equals 'localhost' should be case insensitive.
        /// </summary>
        [TestMethod]
        public void LocalhostTo127001_WithLocalhostInCaps_ShouldReturnUpdatedUri()
        {
            Uri originalUri = new Uri("https://lOcaLhoST:9876");

            Assert.AreEqual("https://127.0.0.1:9876/", originalUri.LocalhostTo127001().ToString());
        }

        /// <summary>
        /// If the host name of a Uri used to define the Agent remote address does not contain 'localhost',
        /// it should return unchanged.
        /// </summary>
        [TestMethod]
        public void LocalhostTo127001_WithoutLocalhostInUri_ShouldReturnUnchanged()
        {
            Uri originalUri = new Uri("http://my.host.com:5678");

            Assert.AreEqual("http://my.host.com:5678/", originalUri.LocalhostTo127001().ToString());
        }
    }
}

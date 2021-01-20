// <copyright file="SessionRequestTest.cs" company="TestProject">
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

namespace TestProject.OpenSDK.Tests.UnitTests.Internal.Rest.Messages
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OpenQA.Selenium.Chrome;
    using TestProject.OpenSDK.Drivers.Generic;
    using TestProject.OpenSDK.Drivers.Web;
    using TestProject.OpenSDK.Internal.Rest.Messages;

    /// <summary>
    /// Class containing unit tests for the <see cref="SessionRequest"/> class.
    /// </summary>
    [TestClass]
    public class SessionRequestTest
    {
        /// <summary>
        /// Creating a new <see cref="SessionRequest"/> object for a <see cref="GenericDriver"/> should yield expected capabilities.
        /// </summary>
        [TestMethod]
        public void NewSessionRequest_ForGenericDriver_ShouldContainExpectedCapabilities()
        {
            GenericOptions genericOptions = new GenericOptions();

            SessionRequest sessionRequest = new SessionRequest(null, genericOptions);

            Assert.AreEqual(1, sessionRequest.Capabilities.Count);
            Assert.IsTrue(sessionRequest.Capabilities.ContainsKey("PlatformName"));

            sessionRequest.Capabilities.TryGetValue("PlatformName", out object actualPlatformName);

            Assert.AreEqual("ANY", actualPlatformName.ToString());
        }

        /// <summary>
        /// Creating a new <see cref="SessionRequest"/> object for a <see cref="ChromeDriver"/> should yield expected capabilities.
        /// We're only testing this for Chrome, because we're using the Selenium implementation for all drivers other than the <see cref="GenericDriver"/>,
        /// so there isn't much use in writing tests for all other cases.
        /// </summary>
        [TestMethod]
        public void NewSessionRequest_ForChromeDriver_ShouldContainExpectedCapabilities()
        {
            ChromeOptions chromeOptions = new ChromeOptions();

            SessionRequest sessionRequest = new SessionRequest(null, chromeOptions);

            Assert.AreEqual(2, sessionRequest.Capabilities.Count);
            Assert.IsTrue(sessionRequest.Capabilities.ContainsKey("browserName"));
            Assert.IsTrue(sessionRequest.Capabilities.ContainsKey("goog:chromeOptions"));

            sessionRequest.Capabilities.TryGetValue("browserName", out object actualBrowserName);

            Assert.AreEqual("chrome", actualBrowserName.ToString());
        }
    }
}

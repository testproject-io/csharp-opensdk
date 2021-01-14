// <copyright file="GenericDriverTest.cs" company="TestProject">
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
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TestProject.OpenSDK.Drivers.Generic;

    /// <summary>
    /// This class contains examples of using the TestProject C# SDK with the <see cref="GenericDriver"/>.
    /// </summary>
    [TestClass]
    public class GenericDriverTest
    {
        /// <summary>
        /// The TestProject GenericDriver instance to be used in this test class.
        /// </summary>
        private GenericDriver driver;

        /// <summary>
        /// Starts a generic driver session before the start of each test.
        /// </summary>
        [TestInitialize]
        public void StartDriver()
        {
            this.driver = new GenericDriver();
        }

        /// <summary>
        /// An example test logging in to the TestProject demo application with Chrome.
        /// </summary>
        [TestMethod]
        public void ExampleTestUsingGenericDriver()
        {
            this.driver.Report().Step("A generic driver step", "A message that goes with the step", true);
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

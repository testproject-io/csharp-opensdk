// <copyright file="GenericOptions.cs" company="TestProject">
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

namespace TestProject.OpenSDK.Drivers.Generic
{
    using OpenQA.Selenium;

    /// <summary>
    /// Custom class extending <see cref="DriverOptions"/> to allow providing of capabilities for a <see cref="GenericDriver"/>.
    /// </summary>
    public class GenericOptions : DriverOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericOptions"/> class.
        /// </summary>
        public GenericOptions()
        {
            this.PlatformName = "ANY";
        }

        /// <summary>
        /// Provides a means to add additional capabilities not yet added as type safe options
        /// for the specific browser driver.
        /// </summary>
        /// <param name="capabilityName">The name of the capability to add.</param>
        /// <param name="capabilityValue">The value of the capability to add.</param>
        public override void AddAdditionalCapability(string capabilityName, object capabilityValue)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Returns the <see cref="ICapabilities"/> for the specific browser driver with these
        /// options included as capabilities. This does not copy the options. Further
        /// changes will be reflected in the returned capabilities.
        /// </summary>
        /// <returns>The <see cref="ICapabilities"/> for browser driver with these options.</returns>
        public override ICapabilities ToCapabilities()
        {
            throw new System.NotImplementedException();
        }
    }
}

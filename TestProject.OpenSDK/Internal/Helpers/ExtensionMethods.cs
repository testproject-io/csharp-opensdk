// <copyright file="ExtensionMethods.cs" company="TestProject">
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

using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace TestProject.OpenSDK.Internal.Helpers
{
    /// <summary>
    /// This class contains custom extension methods implementing SDK specific business logic.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Determines whether or not a driver command should be patched before sending it to the Agent.
        /// </summary>
        /// <param name="driverCommand">The driver command that should or should not be patched.</param>
        /// <returns>True if the driver command should be patched, false otherwise.</returns>
        public static bool ShouldBePatched(this string driverCommand)
        {
            return driverCommand.Equals(DriverCommand.SendKeysToElement) || driverCommand.Equals(DriverCommand.SendKeysToActiveElement);
        }

        /// <summary>
        /// Determines whether or not a WebDriver command execution is deemed successful.
        /// </summary>
        /// <param name="response">The <see cref="Response"/> to be inspected.</param>
        /// <returns>True if the Response result is interpreted as successful, false otherwise.</returns>
        public static bool IsPassed(this Response response)
        {
            return response.Status.Equals(WebDriverResult.Success);
        }

        /// <summary>
        /// Changes the hostname of a <see cref="Uri"/> to '127.0.0.1' if the hostname is 'localhost'.
        /// </summary>
        /// <param name="originalUri">The original <see cref="Uri"/>.</param>
        /// <returns>An updated Uri where 'localhost' is replaced with '127.0.0.1'.</returns>
        public static Uri LocalhostTo127001(this Uri originalUri)
        {
            if (originalUri.Host.ToLower().Equals("localhost"))
            {
                UriBuilder builder = new UriBuilder(originalUri);
                builder.Host = "127.0.0.1";
                return builder.Uri;
            }
            else
            {
                return originalUri;
            }
        }
    }
}

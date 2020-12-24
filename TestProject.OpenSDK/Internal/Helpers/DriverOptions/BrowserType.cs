// <copyright file="BrowserType.cs" company="TestProject">
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

using TestProject.OpenSDK.Drivers.Web;

namespace TestProject.OpenSDK.Internal.Helpers.DriverOptions
{
    /// <summary>
    /// An enumeration of supported browser types.
    /// </summary>
    public enum BrowserType
    {
        /// <summary>
        /// Represents <see cref="ChromeDriver"/>.
        /// </summary>
        Chrome,

        /// <summary>
        /// Represents <see cref="FirefoxDriver"/>.
        /// </summary>
        Firefox,

        /// <summary>
        /// Represents <see cref="InternetExplorerDriver"/>.
        /// </summary>
        InternetExplorer,

        /// <summary>
        /// Represents <see cref="EdgeDriver"/>.
        /// </summary>
        Edge,

        /// <summary>
        /// Represents <see cref="SafariDriver"/>.
        /// </summary>
        Safari,

        /// <summary>
        /// Represents the <see cref="RemoteWebDriver"/>.
        /// </summary>
        Remote,
    }
}

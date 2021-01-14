// <copyright file="SessionResponse.cs" company="TestProject">
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

namespace TestProject.OpenSDK.Internal.Rest.Messages
{
    using OpenQA.Selenium.Chrome;

    /// <summary>
    /// Payload object returned by the Agent when starting a development session.
    /// </summary>
    public class SessionResponse
    {
        /// <summary>
        /// Port number that Agent is listening on for the SDK to connect.
        /// </summary>
        public int DevSocketPort { get; set; }

        /// <summary>
        /// Remote address of a Selenium / Appium server for the driver to communicate with.
        /// </summary>
        public string ServerAddress { get; set; }

        /// <summary>
        /// ID of the session that has been initialized by the Agent.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Dialect of the session that has been initialized by the Agent.
        /// </summary>
        public string Dialect { get; set; }

        /// <summary>
        /// Capabilities of the session that has been initialized by the Agent.
        /// </summary>
        public ChromeOptions Capabilities { get; set; }

        /// <summary>
        /// The current version of the Agent.
        /// </summary>
        public string Version { get; set; }
    }
}

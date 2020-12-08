// <copyright file="AgentSession.cs" company="TestProject">
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

namespace TestProject.OpenSDK.Internal.Rest
{
    /// <summary>
    /// Agent session model containing data related to a specific Agent session.
    /// </summary>
    public class AgentSession
    {
        /// <summary>
        /// The Agent API base URL.
        /// </summary>
        public Uri RemoteAddress { get; }

        /// <summary>
        /// A unique identifier for the current session.
        /// </summary>
        public string SessionId { get; }

        /// <summary>
        /// The current WebDriver dialect (W3C or OSS).
        /// </summary>
        public string Dialect { get; }

        /// <summary>
        /// Actual capabilities returned by the driver after it has been created.
        /// </summary>
        public DriverOptions Capabilities { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentSession"/> class.
        /// </summary>
        /// <param name="remoteAddress">The Agent API base URL.</param>
        /// <param name="sessionId">A unique identifier for the current session.</param>
        /// <param name="dialect">The current WebDriver dialect (W3C or OSS).</param>
        /// <param name="capabilities">Actual capabilities returned by the driver after it has been created.</param>
        public AgentSession(Uri remoteAddress, string sessionId, string dialect, DriverOptions capabilities)
        {
            this.RemoteAddress = remoteAddress;
            this.SessionId = sessionId;
            this.Dialect = dialect;
            this.Capabilities = capabilities;
        }
    }
}

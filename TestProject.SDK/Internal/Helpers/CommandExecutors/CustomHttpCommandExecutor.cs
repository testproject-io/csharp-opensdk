// <copyright file="CustomHttpCommandExecutor.cs" company="TestProject">
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
using OpenQA.Selenium.Remote;
using TestProject.SDK.Internal.Rest;

namespace TestProject.SDK.Internal.Helpers.CommandExecutors
{
    /// <summary>
    /// A custom commands executor for Selenium drivers.
    /// Extends the original functionality by restoring driver session initiated by the Agent.
    /// Reports commands executed to Agent.
    /// </summary>
    public class CustomHttpCommandExecutor : HttpCommandExecutor
    {
        /// <summary>
        /// Client used to communicate with the Agent.
        /// </summary>
        public AgentClient AgentClient { get; }

        private static readonly TimeSpan RemoteConnectionTimeout = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomHttpCommandExecutor"/> class.
        /// </summary>
        /// <param name="agentClient"><see cref="AgentClient"/> used to communicate with the Agent.</param>
        /// <param name="addressOfRemoteServer">URL of the remote Selenium server managed by the Agent.</param>
        public CustomHttpCommandExecutor(AgentClient agentClient, Uri addressOfRemoteServer)
            : base(addressOfRemoteServer, RemoteConnectionTimeout)
        {
            this.AgentClient = agentClient;
        }

        /// <summary>
        /// Overrides base implementation of Execute().
        /// </summary>
        /// <param name="commandToExecute">The WebDriver command to execute.</param>
        /// <returns>The <see cref="Response"/> returned by the Agent.</returns>
        public override Response Execute(Command commandToExecute)
        {
            return this.Execute(commandToExecute, false);
        }

        /// <summary>
        /// Extended command execution method.
        /// Allows skipping reporting for "internal" commands, for example:
        /// - Taking screenshot for manual step reporting.
        /// - Inspecting element type to determine whether redaction is required.
        /// </summary>
        /// <param name="commandToExecute">The WebDriver command to execute.</param>
        /// <param name="skipReporting">True if the command should not be reported, false otherwise.</param>
        /// <returns>The <see cref="Response"/> returned by the Agent.</returns>
        public Response Execute(Command commandToExecute, bool skipReporting)
        {
            Response response = base.Execute(commandToExecute);

            if (!skipReporting)
            {
                // TODO: report command
            }

            return response;
        }
    }
}

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

namespace TestProject.OpenSDK.Internal.Helpers.CommandExecutors
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using OpenQA.Selenium.Remote;
    using TestProject.OpenSDK.Internal.Rest;

    /// <summary>
    /// A custom commands executor for Selenium drivers.
    /// Extends the original functionality by restoring driver session initiated by the Agent.
    /// </summary>
    public class CustomHttpCommandExecutor : HttpCommandExecutor, ITestProjectCommandExecutor
    {
        /// <summary>
        /// Object responsible for executing reporting to TestProject.
        /// </summary>
        public ReportingCommandExecutor ReportingCommandExecutor { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomHttpCommandExecutor"/> class.
        /// </summary>
        /// <param name="addressOfRemoteServer">URL of the remote Selenium server managed by the Agent.</param>
        /// <param name="disableReports">True if all reporting should be disabled, false otherwise.</param>
        public CustomHttpCommandExecutor(Uri addressOfRemoteServer, bool disableReports)
            : this(addressOfRemoteServer, disableReports, TimeSpan.FromSeconds(10))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomHttpCommandExecutor"/> class.
        /// </summary>
        /// <param name="addressOfRemoteServer">URL of the remote Selenium server managed by the Agent.</param>
        /// <param name="disableReports">True if all reporting should be disabled, false otherwise.</param>
        /// <param name="remoteConnectionTimeout">Timeout for the remote connection to the WebDriver server executing the commands.</param>
        public CustomHttpCommandExecutor(Uri addressOfRemoteServer, bool disableReports, TimeSpan remoteConnectionTimeout)
            : base(addressOfRemoteServer, remoteConnectionTimeout)
        {
            // If the driver returned by the Agent is in W3C mode, we need to update the command info repository
            // associated with the base HttpCommandExecutor to the W3C command info repository (default is OSS).
            if (AgentClient.GetInstance().IsInW3CMode())
            {
                FieldInfo commandInfoRepositoryField = typeof(HttpCommandExecutor).GetField("commandInfoRepository", BindingFlags.Instance | BindingFlags.NonPublic);
                commandInfoRepositoryField.SetValue(this, new W3CWireProtocolCommandInfoRepository());
            }

            this.ReportingCommandExecutor = new ReportingCommandExecutor(this, disableReports);
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
            // The Selenium HttpCommandExecutor modifies the command parameters, removing properties we need along the way
            // We want to use the original command parameters when reporting, not the modified one after command execution.
            var originalParameters = new Dictionary<string, object>(commandToExecute.Parameters);

            Response response = base.Execute(commandToExecute);

            // Create a command to report using the original parameters instead of the modified ones.
            var commandToReport = new Command(commandToExecute.SessionId, commandToExecute.Name, originalParameters);

            if (!skipReporting)
            {
                this.ReportingCommandExecutor.ReportCommand(commandToReport, response);
            }

            return response;
        }
    }
}

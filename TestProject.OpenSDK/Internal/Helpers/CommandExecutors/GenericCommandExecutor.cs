// <copyright file="GenericCommandExecutor.cs" company="TestProject">
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
    using NLog;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Remote;
    using TestProject.OpenSDK.Internal.Rest;

    /// <summary>
    /// A custom commands executor for the <see cref="GenericDriver"/>.
    /// Extends the original functionality by restoring driver session initiated by the Agent.
    /// </summary>
    public class GenericCommandExecutor : ITestProjectCommandExecutor
    {
        /// <summary>
        /// Object responsible for executing reporting to TestProject.
        /// </summary>
        public ReportingCommandExecutor ReportingCommandExecutor { get; set; }

        /// <summary>
        /// Logger instance for this class.
        /// </summary>
        private static Logger Logger { get; set; } = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericCommandExecutor"/> class.
        /// </summary>
        /// <param name="addressOfRemoteServer">URL of the Agent.</param>
        /// <param name="disableReports">True if all reporting should be disabled, false otherwise.</param>
        public GenericCommandExecutor(Uri addressOfRemoteServer, bool disableReports)
        {
            this.ReportingCommandExecutor = new ReportingCommandExecutor(this, disableReports);
        }

        /// <summary>
        /// Overrides base implementation of Execute().
        /// </summary>
        /// <param name="commandToExecute">The WebDriver command to execute.</param>
        /// <returns>The <see cref="Response"/> returned by the Agent.</returns>
        public Response Execute(Command commandToExecute)
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
            if (commandToExecute.Name.Equals(DriverCommand.Screenshot))
            {
                // Handle users trying to take a screenshot using the generic driver
                Logger.Info("The generic driver does not support taking screenshots");

                Response response = new Response();
                response.Status = WebDriverResult.UnableToCaptureScreen;
                response.SessionId = AgentClient.GetInstance().AgentSession.SessionId;
                response.Value = new Dictionary<string, object>();

                return response;
            }
            else
            {
                throw new NotImplementedException($"The generic driver cannot execute the WebDriver command '{commandToExecute.Name}'");
            }
        }
    }
}

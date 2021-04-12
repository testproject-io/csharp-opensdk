// <copyright file="AppiumCustomHttpCommandExecutor.cs" company="TestProject">
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
    using OpenQA.Selenium;
    using OpenQA.Selenium.Remote;

    /// <summary>
    /// A custom commands executor for Appium drivers.
    /// Extends the original functionality of <see cref="CustomHttpCommandExecutor"/> for appium drivers.
    /// </summary>
    internal class AppiumCustomHttpCommandExecutor : CustomHttpCommandExecutor
    {
        private bool skipReporting;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppiumCustomHttpCommandExecutor"/> class.
        /// </summary>
        /// <param name="addressOfRemoteServer">URL of the remote Selenium server managed by the Agent.</param>
        /// <param name="disableReports">True if all reporting should be disabled, false otherwise.</param>
        public AppiumCustomHttpCommandExecutor(Uri addressOfRemoteServer, bool disableReports)
            : base(addressOfRemoteServer, disableReports)
        {
            this.skipReporting = disableReports;
        }

        /// <summary>
        /// Extended command execution method for Appium drivers.
        /// </summary>
        /// <param name="commandToExecute">The WebDriver command to execute.</param>
        /// <returns>The <see cref="Response"/> returned by the Agent.</returns>
        public override Response Execute(Command commandToExecute)
        {
            // The Selenium HttpCommandExecutor modifies the command parameters, removing properties we need along the way
            // We want to use the original command parameters when reporting, not the modified one after command execution.
            var originalParameters = new Dictionary<string, object>(commandToExecute.Parameters);

            Response response;

            // If action is quit, report the command and return null to not abort the driver session
            if (commandToExecute.Name.Equals(DriverCommand.Quit))
            {
                if (!this.skipReporting)
                {
                    response = new Response
                    {
                        SessionId = commandToExecute.SessionId.ToString(),
                        Status = WebDriverResult.Success,
                    };
                    this.ReportingCommandExecutor.ReportCommand(commandToExecute, response);
                }

                // Returning null assures the quit of the base driver does not execute
                return null;
            }
            else
            {
                try
                {
                    response = base.Execute(commandToExecute);
                }
                catch (WebDriverException)
                {
                    Dictionary<string, object> responseValue = new Dictionary<string, object>();
                    responseValue.Add("error", "no such element");
                    responseValue.Add("message", $"Unable to locate element {commandToExecute.ParametersAsJsonString}");

                    response = new Response
                    {
                        Status = WebDriverResult.Timeout,
                        SessionId = commandToExecute.SessionId.ToString(),
                        Value = responseValue,
                    };
                }
            }

            // Create a command to report using the original parameters instead of the modified ones.
            var commandToReport = new Command(commandToExecute.SessionId, commandToExecute.Name, originalParameters);

            if (!this.skipReporting)
            {
                this.ReportingCommandExecutor.ReportCommand(commandToReport, response);
            }

            return response;
        }
    }
}

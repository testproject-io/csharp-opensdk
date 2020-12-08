// <copyright file="ReportingCommandExecutor.cs" company="TestProject">
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

using System.Collections.Generic;
using NLog;
using OpenQA.Selenium.Remote;
using TestProject.OpenSDK.Internal.Rest;
using TestProject.OpenSDK.Internal.Rest.Messages;

namespace TestProject.OpenSDK.Internal.Helpers.CommandExecutors
{
    /// <summary>
    /// Reports commands executed to Agent.
    /// </summary>
    public class ReportingCommandExecutor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportingCommandExecutor"/> class.
        /// </summary>
        public ReportingCommandExecutor()
        {
        }

        /// <summary>
        /// Report an executed WebDriver command and its result to TestProject.
        /// </summary>
        /// <param name="commandName">The name of the WebDriver command that was executed.</param>
        /// <param name="commandParams">The corresponding command parameters.</param>
        /// <param name="result">The result of the command execution.</param>
        /// <param name="passed">True if command execution was successful, false otherwise.</param>
        public void ReportCommand(string commandName, Dictionary<string, object> commandParams, Dictionary<string, object> result, bool passed)
        {
            if (commandName.Equals(DriverCommand.Quit))
            {
                // TODO: auto report a test if automatic test reporting is not disabled
                return;
            }

            // TODO: add command redaction

            // TODO: add logic to detect if we're inside a WebDriverWait

            DriverCommandReport driverCommandReport = new DriverCommandReport(commandName, commandParams, result, passed);

            // TODO: add screenshot if command was failed

            // TODO: add command stashing logic

            AgentClient.GetInstance().ReportDriverCommand(driverCommandReport);
        }
    }
}

// <copyright file="RedactHelper.cs" company="TestProject">
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

namespace TestProject.OpenSDK.Internal.Helpers
{
    using System.Collections.Generic;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Remote;
    using TestProject.OpenSDK.Internal.Helpers.CommandExecutors;
    using TestProject.OpenSDK.Internal.Rest;

    /// <summary>
    /// This class contains helper methods to redact WebDriver commands before reporting them to the Agent.
    /// </summary>
    public static class RedactHelper
    {
        /// <summary>
        /// Redacts sensitive data from commands reports (only if sent to a secured element).
        /// </summary>
        /// <param name="executor">The command executor used by the driver to execute WebDriver commands.</param>
        /// <param name="command">The command to be redacted.</param>
        /// <returns>The redacted command, or the original command if no redaction is required.</returns>
        public static Command RedactCommand(ITestProjectCommandExecutor executor, Command command)
        {
            if (command.Name.Equals(DriverCommand.SendKeysToElement) || command.Name.Equals(DriverCommand.SendKeysToActiveElement))
            {
                string elementId = command.Parameters.TryGetValue("id", out var elId) ? elId.ToString() : string.Empty;

                if (!IsRedactRequired(executor, command, elementId))
                {
                    return command;
                }

                command.Parameters["text"] = "***";
                command.Parameters["value"] = new string[] { "***" };
            }

            return command;
        }

        /// <summary>
        /// Checks whether redaction is required.
        /// </summary>
        /// <param name="executor">The command executor used by the driver to execute WebDriver commands.</param>
        /// <param name="command">The command to be redacted.</param>
        /// <param name="elementId">The unique ID of the element that the command was executed on.</param>
        /// <returns>True if redaction is required, false otherwise.</returns>
        private static bool IsRedactRequired(ITestProjectCommandExecutor executor, Command command, string elementId)
        {
            // TODO: check if element is a mobile element and act accordingly
            return IsSecuredElement(executor, elementId);
        }

        /// <summary>
        /// Checks whether or not the element is a secured element.
        /// </summary>
        /// <param name="executor">The command executor used by the driver to execute WebDriver commands.</param>
        /// <param name="elementId">The unique ID of the element that the command was executed on.</param>
        /// <returns>True if the element is a secure element, false otherwise.</returns>
        private static bool IsSecuredElement(ITestProjectCommandExecutor executor, string elementId)
        {
            Dictionary<string, object> getAttributeParameters = new Dictionary<string, object>();
            getAttributeParameters.Add("id", elementId);
            getAttributeParameters.Add("name", "type");

            Command getAttributeCommand = new Command(
                new SessionId(AgentClient.GetInstance().AgentSession.SessionId),
                DriverCommand.GetElementAttribute,
                getAttributeParameters);

            Response response = executor.Execute(getAttributeCommand, true);

            string inputType = response.Status.Equals(WebDriverResult.Success) ? (response.Value?.ToString() ?? string.Empty) : string.Empty;

            return inputType.Equals("password") || inputType.Equals("XCUIElementTypeSecureTextField");
        }
    }
}

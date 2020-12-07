﻿// <copyright file="DriverCommandReport.cs" company="TestProject">
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

namespace TestProject.SDK.Internal.Rest.Messages
{
    /// <summary>
    /// Payload object sent to the Agent to report a WebDriver command.
    /// </summary>
    public class DriverCommandReport
    {
        /// <summary>
        /// WebDriver command executed by the driver.
        /// </summary>
        public string CommandName { get; }

        /// <summary>
        /// The command parameters.
        /// </summary>
        public Dictionary<string, object> CommandParameters { get; }

        /// <summary>
        /// The result of the command that was executed.
        /// </summary>
        public Dictionary<string, object> Result { get; }

        /// <summary>
        /// True if the command was executed successfully, false otherwise.
        /// </summary>
        public bool Passed { get; }

        /// <summary>
        /// A base64 encoded screenshot, to be included in the report.
        /// </summary>
        public string Screenshot { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DriverCommandReport"/> class.
        /// </summary>
        /// <param name="commandName">WebDriver command executed by the driver.</param>
        /// <param name="commandParameters">The command parameters.</param>
        /// <param name="result">Result of the command that was executed.</param>
        /// <param name="passed">True if the command was executed successfully, false otherwise.</param>
        public DriverCommandReport(string commandName, Dictionary<string, object> commandParameters, Dictionary<string, object> result, bool passed)
        {
            this.CommandName = commandName;
            this.CommandParameters = commandParameters;
            this.Result = result;
            this.Passed = passed;
        }
    }
}

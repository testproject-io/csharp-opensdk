// <copyright file="StashedCommand.cs" company="TestProject">
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
    using OpenQA.Selenium.Remote;

    /// <summary>
    /// Payload object sent to the Agent to report a WebDriver command.
    /// </summary>
    public class StashedCommand
    {
        /// <summary>
        /// WebDriver command executed by the driver.
        /// </summary>
        public Command Command { get; }

        /// <summary>
        /// The command execution result.
        /// </summary>
        public object Result { get; }

        /// <summary>
        /// True if the command was executed successfully, false otherwise.
        /// </summary>
        public bool Passed { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StashedCommand"/> class.
        /// </summary>
        /// <param name="command">WebDriver command executed by the driver.</param>
        /// <param name="result">Result of the command that was executed.</param>
        /// <param name="passed">True if the command was executed successfully, false otherwise.</param>
        public StashedCommand(Command command, object result, bool passed)
        {
            this.Command = command;
            this.Result = result;
            this.Passed = passed;
        }
    }
}

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

using System;
using System.Collections.Generic;
using NLog;
using OpenQA.Selenium.Remote;
using TestProject.OpenSDK.Internal.CallStackAnalysis;
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
        /// Flag to enable / disable all reporting.
        /// </summary>
        public bool ReportsDisabled { get; set; }

        /// <summary>
        /// Flag to enable / disable automatic driver command reporting.
        /// </summary>
        public bool CommandReportsDisabled { get; set; }

        /// <summary>
        /// Flag to enable / disable automatic test reporting.
        /// </summary>
        public bool AutoTestReportsDisabled { get; set; }

        /// <summary>
        /// Flag to enable / disable command reporting.
        /// </summary>
        public bool RedactionDisabled { get; set; }

        private ITestProjectCommandExecutor commandExecutor;

        private StashedCommand stashedCommand;

        private string currentTestName;

        /// <summary>
        /// Logger instance for this class.
        /// </summary>
        private static Logger Logger { get; set; } = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportingCommandExecutor"/> class.
        /// </summary>
        /// <param name="commandExecutor">The HTTP command executor used to send WebDriver commands.</param>
        /// <param name="disableReports">True if all reporting should be disabled, false otherwise.</param>
        public ReportingCommandExecutor(ITestProjectCommandExecutor commandExecutor, bool disableReports)
        {
            this.commandExecutor = commandExecutor;
            this.ReportsDisabled = disableReports;
        }

        /// <summary>
        /// Report an executed WebDriver command and its result to TestProject.
        /// </summary>
        /// <param name="command">The WebDriver <see cref="Command"/> to report.</param>
        /// <param name="response">The <see cref="Response"/> from the WebDriver server upon sending the command.</param>
        public void ReportCommand(Command command, Response response)
        {
            bool isQuitCommand = command.Name.Equals(DriverCommand.Quit);

            if (!this.AutoTestReportsDisabled)
            {
                this.ReportTest(isQuitCommand);
            }

            if (isQuitCommand)
            {
                // Close the client after finishing the test using driver.Quit()
                AgentClient.GetInstance().Stop();

                // Do not report Quit() command to avoid creating a new test in the reports
                return;
            }

            Dictionary<string, object> result;

            try
            {
                result = (Dictionary<string, object>)response.Value;
            }
            catch (InvalidCastException)
            {
                result = new Dictionary<string, object>();
            }

            if (StackTraceHelper.Instance.IsRunningInsideWait())
            {
                // We're only interested in reporting the final FindElement or FindElements call
                // (these are executed by the ExpectedConditions helper methods)
                if (
                    command.Name.Equals(DriverCommand.FindElement) ||
                    command.Name.Equals(DriverCommand.FindElements))
                {
                    this.stashedCommand = new StashedCommand(command, result, response.IsPassed());
                }

                // Do not report the command right away if it's executed inside a WebDriverWait
                return;
            }

            // If we have a previously stashed command to report, report it first.
            if (this.stashedCommand != null)
            {
                this.SendCommandToAgent(this.stashedCommand.Command, this.stashedCommand.Result, this.stashedCommand.Passed);
                this.stashedCommand = null;
            }

            this.SendCommandToAgent(command, result, response.IsPassed());
        }

        /// <summary>
        /// Clear stashed command by reporting it to TestProject.
        /// </summary>
        public void ClearStash()
        {
            if (this.stashedCommand != null)
            {
                this.SendCommandToAgent(this.stashedCommand.Command, this.stashedCommand.Result, this.stashedCommand.Passed);
                this.stashedCommand = null;
            }
        }

        /// <summary>
        /// Report a test to the Agent.
        /// </summary>
        /// <param name="force">True if called just before the session closes, but test name has not changed.
        /// This forces reporting a test at the end of the session. False, otherwise.</param>
        public void ReportTest(bool force)
        {
            // Get the name of the test method currently running
            string inferredTestName = StackTraceHelper.Instance.GetInferredTestName();

            // If the current test name has not been set, set it
            if (this.currentTestName == null)
            {
                this.currentTestName = inferredTestName;
            }

            if (inferredTestName == null)
            {
                return;
            }

            if (!inferredTestName.Equals(this.currentTestName) || force)
            {
                if (this.ReportsDisabled)
                {
                    Logger.Trace($"Test [{this.currentTestName}] - [Passed]");
                    return;
                }

                // Report a finished test
                TestReport testReport = new TestReport(this.currentTestName, true, null);
                AgentClient.GetInstance().ReportTest(testReport);

                // Update the current test name
                this.currentTestName = inferredTestName;
            }
        }

        /// <summary>
        /// Creates a screenshot (PNG) and returns it as a base64 encoded string.
        /// </summary>
        /// <returns>The base64 encoded screenshot in PNG format.</returns>
        public string GetScreenshot()
        {
            string sessionId = AgentClient.GetInstance().AgentSession.SessionId;

            Dictionary<string, object> screenshotCommandParameters = new Dictionary<string, object>();
            screenshotCommandParameters.Add("sessionId", sessionId);

            Command screenshotCommand = new Command(new SessionId(sessionId), DriverCommand.Screenshot, screenshotCommandParameters);

            Response response = this.commandExecutor.Execute(screenshotCommand, true);

            return response.Value.ToString();
        }

        /// <summary>
        /// Send an executed WebDriver command to the Agent.
        /// </summary>
        /// <param name="command">The WebDriver command that was executed.</param>
        /// <param name="result">The result of the command execution.</param>
        /// <param name="passed">True if command execution was successful, false otherwise.</param>
        private void SendCommandToAgent(Command command, Dictionary<string, object> result, bool passed)
        {
            if (this.ReportsDisabled || this.CommandReportsDisabled)
            {
                Logger.Trace($"Command '{command.Name}' {(passed ? "passed" : "failed")}");
                return;
            }

            if (!this.RedactionDisabled)
            {
                command = RedactHelper.RedactCommand(this.commandExecutor, command);
            }

            DriverCommandReport driverCommandReport = new DriverCommandReport(command.Name, command.Parameters, result, passed);

            if (!passed)
            {
                driverCommandReport.Screenshot = this.GetScreenshot();
            }

            AgentClient.GetInstance().ReportDriverCommand(driverCommandReport);
        }
    }
}

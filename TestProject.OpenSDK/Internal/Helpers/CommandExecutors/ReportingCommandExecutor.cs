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

namespace TestProject.OpenSDK.Internal.Helpers.CommandExecutors
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using NLog;
    using OpenQA.Selenium.Remote;
    using TestProject.OpenSDK.Enums;
    using TestProject.OpenSDK.Internal.CallStackAnalysis;
    using TestProject.OpenSDK.Internal.Rest;
    using TestProject.OpenSDK.Internal.Rest.Messages;

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
        public DriverCommandsFilter CommandReportsDisabled { get; set; }

        /// <summary>
        /// Flag to enable / disable automatic test reporting.
        /// </summary>
        public bool AutoTestReportsDisabled { get; set; }

        /// <summary>
        /// Flag to enable / disable command reporting.
        /// </summary>
        public bool RedactionDisabled { get; set; }

        private ITestProjectCommandExecutor commandExecutor;

        private OrderedDictionary stashedCommands;

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
            this.stashedCommands = new OrderedDictionary();
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
                var instance = AgentClient.GetInstance();

                // Send SpecFlow test report, if exists.
                if (instance.SpecFlowTestReport != null)
                {
                    instance.ReportTest(instance.SpecFlowTestReport);
                }

                // Do not report Quit() command to avoid creating a new test in the reports
                return;
            }

            // If the command is IsDisplayed, the passed value we report to agent should be whether the element is visible or not.
            bool passed = response.IsPassed();
            if (CommandHelper.IsDisplayedCommand(command) && passed)
            {
                passed = bool.Parse(response.Value.ToString());
            }

            if (StackTraceHelper.Instance.IsRunningInsideWait())
            {
                // Save the command
                var stashedCommand = new StashedCommand(command, response.Value, response.IsPassed());
                this.stashedCommands[$"{command}_{response.IsPassed()}"] = stashedCommand;
                var stashedCommand = new StashedCommand(command, response.Value, passed);

                // Do not report the command right away if it's executed inside a WebDriverWait
                return;
            }

            // Send all stashed command first.
            this.ClearStash();

            // If we have a previously stashed command to report, report it first.
            this.SendCommandToAgent(command, response.Value, response.IsPassed());
        }

        /// <summary>
        /// Clear stashed command by reporting it to TestProject.
        /// </summary>
        public void ClearStash()
        {
            if (this.stashedCommands.Count > 0)
            {
                foreach (DictionaryEntry keyValuePair in this.stashedCommands)
                {
                    var command = (StashedCommand)keyValuePair.Value;
                    this.SendCommandToAgent(command.Command, command.Result, command.Passed);
                }

                this.stashedCommands.Clear();
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

            // Only report a test if:
            // - The inferred test name is not null and not equal to the current test name
            // - The force flag is set (== driver.Quit() is called)
            if ((inferredTestName == null ? false : !inferredTestName.Equals(this.currentTestName)) || force)
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
        private void SendCommandToAgent(Command command, object result, bool passed)
        {
            if (this.ReportsDisabled || this.CommandReportsDisabled.Equals(DriverCommandsFilter.All))
            {
                Logger.Trace($"Command '{command.Name}' {(passed ? "passed" : "failed")}");
                return;
            }

            if (this.CommandReportsDisabled.Equals(DriverCommandsFilter.Passing) && !passed)
            {
                // Report failed driver commands in a user friendly way if explicitly requested by the user
                StepReport stepReport = new StepReport(
                    description: $"Failed to execute driver command '{command.Name}'",
                    message: result.ToJson(),
                    passed: false,
                    screenshot: this.GetScreenshot());

                AgentClient.GetInstance().ReportStep(stepReport);
                return;
            }

            if (this.CommandReportsDisabled.Equals(DriverCommandsFilter.None))
            {
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
}

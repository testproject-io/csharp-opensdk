// <copyright file="Reporter.cs" company="TestProject">
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

using NLog;
using TestProject.OpenSDK.Internal.Helpers.CommandExecutors;
using TestProject.OpenSDK.Internal.Rest;
using TestProject.OpenSDK.Internal.Rest.Messages;

namespace TestProject.OpenSDK.Internal.Reporting
{
    /// <summary>
    /// Exposes reporting actions to the WebDriver object.
    /// </summary>
    public class Reporter
    {
        /// <summary>
        /// The HTTP command executor associated with the current driver session.
        /// </summary>
        private ReportingCommandExecutor reportingCommandExecutor;

        /// <summary>
        /// Logger instance for this class.
        /// </summary>
        private static Logger Logger { get; set; } = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initializes a new instance of the <see cref="Reporter"/> class.
        /// </summary>
        /// <param name="reportingCommandExecutor">The reporting command executor associated with the current WebDriver object.</param>
        public Reporter(ReportingCommandExecutor reportingCommandExecutor)
        {
            this.reportingCommandExecutor = reportingCommandExecutor;
        }

        /// <summary>
        /// Creates a test report and sends it to the <see cref="AgentClient"/>.
        /// </summary>
        /// <param name="name">The name of the test.</param>
        /// <param name="passed">True if the test should be marked as passed, false otherwise.</param>
        /// <param name="message">A message that goes with the test.</param>
        public void Test(string name, bool passed = true, string message = null)
        {
            if (!this.reportingCommandExecutor.ReportsDisabled)
            {
                if (!this.reportingCommandExecutor.AutoTestReportsDisabled)
                {
                    Logger.Warn("Automatic reporting is enabled, disable this using DisableAutoTestReports() " +
                        "to avoid duplicates in the report.");
                }

                TestReport testReport = new TestReport(name, passed, message);

                AgentClient.GetInstance().ReportTest(testReport);
            }
            else
            {
                Logger.Trace($"Test '{name}' {(passed ? "passed" : "failed")}");
            }
        }

        /// <summary>
        /// Creates a step report and sends it to the <see cref="AgentClient"/>.
        /// </summary>
        /// <param name="description">The description of the step.</param>
        /// /// <param name="message">A message that goes with the step.</param>
        /// <param name="passed">True if the step should be marked as passed, false otherwise.</param>
        /// <param name="screenshot">True if a screenshot should be attached to the step, false otherwise.</param>
        public void Step(string description, string message = null, bool passed = true, bool screenshot = false)
        {
            // Check whether it is necessary to report a test, too, and if so, do that.
            this.reportingCommandExecutor.ReportTest(false);

            if (!this.reportingCommandExecutor.ReportsDisabled)
            {
                string screenshotAsString = screenshot ? this.reportingCommandExecutor.GetScreenshot() : null;

                StepReport stepReport = new StepReport(description, message, passed, screenshotAsString);

                AgentClient.GetInstance().ReportStep(stepReport);
            }
            else
            {
                Logger.Trace($"Step '{description}' {(passed ? "passed" : "failed")}");
            }
        }

        /// <summary>
        /// Enables / disables all reporting.
        /// </summary>
        /// <param name="disable">True to disable all reporting, false to enable.</param>
        public void DisableReports(bool disable)
        {
            this.reportingCommandExecutor.ReportsDisabled = disable;
        }

        /// <summary>
        /// Enables / disables automatic driver command reporting.
        /// </summary>
        /// <param name="disable">True to disable automatic driver command reporting, false to enable.</param>
        public void DisableCommandReports(bool disable)
        {
            this.reportingCommandExecutor.CommandReportsDisabled = disable;
        }

        /// <summary>
        /// Enables / disables automatic test reporting.
        /// </summary>
        /// <param name="disable">True to disable automatic test reporting, false to enable.</param>
        public void DisableAutoTestReports(bool disable)
        {
            this.reportingCommandExecutor.AutoTestReportsDisabled = disable;
        }

        /// <summary>
        /// Enables / disables redaction of sensitive text sent to elements deemed private.
        /// </summary>
        /// <param name="disable">True to disable command redaction, false to enable.</param>
        public void DisableRedaction(bool disable)
        {
            this.reportingCommandExecutor.RedactionDisabled = disable;
        }
    }
}

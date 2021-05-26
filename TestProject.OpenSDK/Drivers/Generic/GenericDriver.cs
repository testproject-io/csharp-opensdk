// <copyright file="GenericDriver.cs" company="TestProject">
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

namespace TestProject.OpenSDK.Drivers.Generic
{
    using System;
    using NLog;
    using TestProject.OpenSDK.Enums;
    using TestProject.OpenSDK.Exceptions;
    using TestProject.OpenSDK.Internal.CallStackAnalysis;
    using TestProject.OpenSDK.Internal.Helpers.CommandExecutors;
    using TestProject.OpenSDK.Internal.Reporting;
    using TestProject.OpenSDK.Internal.Rest;

    /// <summary>
    /// Generic driver that can be used to execute non-UI automation and upload the results to TestProject.
    /// </summary>
    public class GenericDriver
    {
        /// <summary>
        /// Flag that indicates whether or not the driver instance is running.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// The minimum version of the Agent supporting the Generic driver.
        /// </summary>
        private readonly Version minGenericDriverSupportedVersion = new Version("0.64.40");

        /// <summary>
        /// The command executor that takes care of executing commands and reporting them to TestProject.
        /// </summary>
        private GenericCommandExecutor commandExecutor;

        /// <summary>
        /// Logger instance for this class.
        /// </summary>
        private static Logger Logger { get; set; } = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericDriver"/> class.
        /// </summary>
        /// <param name="remoteAddress">The base address for the Agent API (e.g. http://localhost:8585).</param>
        /// <param name="token">The development token used to communicate with the Agent, see <a href="https://app.testproject.io/#/integrations/sdk">here</a> for more info.</param>
        /// <param name="projectName">The project name to report.</param>
        /// <param name="jobName">The job name to report.</param>
        /// <param name="disableReports">Set to true to disable all reporting (no report will be created on TestProject).</param>
        /// <param name="reportType">The report type of the execution, can be local, cloud or both.</param>
        /// <param name="reportName">The name of the local generated report.</param>
        /// <param name="reportPath">The path of the local generated report.</param>
        public GenericDriver(
            Uri remoteAddress = null,
            string token = null,
            string projectName = null,
            string jobName = null,
            bool disableReports = false,
            ReportType reportType = ReportType.CLOUD_AND_LOCAL,
            string reportName = null,
            string reportPath = null)
        {
            AgentClient agentClient = AgentClient.GetInstance(remoteAddress, token, new GenericOptions(), new ReportSettings(projectName, jobName, reportType, reportName, reportPath), disableReports, this.minGenericDriverSupportedVersion);

            this.commandExecutor = new GenericCommandExecutor(remoteAddress, disableReports);

            this.IsRunning = true;

            if (StackTraceHelper.Instance.TryDetectSpecFlow())
            {
                var report = this.Report();

                if (!StackTraceHelper.Instance.IsSpecFlowPluginInstalled())
                {
                    string message = "TestProject Plugin for SpecFlow is not installed, please install the plugin and run the Test again.";
                    report.Step(description: message, passed: false);
                    Logger.Error(message);
                    this.Stop();
                    throw new SdkException(message);
                }

                report.DisableCommandReports(DriverCommandsFilter.All);
                report.DisableAutoTestReports(true);
                Logger.Info("SpecFlow detected, applying SpecFlow-specific reporting settings...");
            }
        }

        /// <summary>
        /// Enables access to the TestProject reporting actions from the driver object.
        /// </summary>
        /// <returns><see cref="Reporter"/> object exposing TestProject reporting methods.</returns>
        public Reporter Report()
        {
            return new Reporter(this.commandExecutor.ReportingCommandExecutor);
        }

        /// <summary>
        /// Quits the current driver session and wraps up reporting.
        /// </summary>
        public void Quit()
        {
            if (this.IsRunning)
            {
                this.Stop();
            }
            else
            {
                Logger.Info("Driver is not running, skipping shutdown sequence");
            }
        }

        /// <summary>
        /// Wraps up reporting for the current session.
        /// </summary>
        public void Stop()
        {
            if (!this.commandExecutor.ReportingCommandExecutor.ReportsDisabled)
            {
                this.commandExecutor.ReportingCommandExecutor.ReportTest(true);
            }

            this.IsRunning = false;
        }
    }
}

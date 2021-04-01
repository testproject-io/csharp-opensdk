// <copyright file="DriverBuilder.cs" company="TestProject">
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
namespace TestProject.OpenSDK.Drivers
{
    using System;
    using NLog;
    using OpenQA.Selenium;
    using TestProject.OpenSDK.Enums;

    /// <summary>
    /// Utility class to build Driver instances.
    /// </summary>
    /// <typeparam name="T"> Any of the supported drivers implementing <see cref="BaseDriver"/> class.</param>
    public class DriverBuilder<T>
        where T : BaseDriver
    {
        /// <summary>
        /// Logger instance for this class.
        /// </summary>
        private static Logger Logger { get; set; } = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Options required for session initialization.
        /// </summary>
        private DriverOptions builderOptions;

        /// <summary>
        /// Development token.
        /// </summary>
        private string builderToken;

        /// <summary>
        /// Agent API base URL (e.g. http://localhost:8585/).
        /// </summary>
        private Uri builderRemoteAddress;

        /// <summary>
        /// Project name to report.
        /// </summary>
        private string builderProjectName;

        /// <summary>
        /// Job name to report.
        /// </summary>
        private string builderJobName;

        /// <summary>
        /// Enable/Disable reports.
        /// </summary>
        private bool builderDisableReports;

        /// <summary>
        /// Set report type to Cloud, Local or Both.
        /// </summary>
        private ReportType builderReportType = ReportType.CLOUD_AND_LOCAL;

        /// <summary>
        /// Initializes a new instance of the <see cref="DriverBuilder{T}"/> class.
        /// </summary>
        /// <param name="options">See <see cref="DriverOptions"/> for more details.</param>
        public DriverBuilder(DriverOptions options = null)
        {
            this.builderOptions = options;
        }

        /// <summary>
        /// Set Driver Options that should be passed to the server for session initialization.
        /// </summary>
        /// <param name="options">See <see cref="DriverOptions"/> for more details.</param>
        /// <returns>Modified DriverBuilder instance.</returns>
        public DriverBuilder<T> WithOptions(DriverOptions options)
        {
            this.builderOptions = options;
            return this;
        }

        /// <summary>
        /// Set a development token to authorize with the Agent.
        /// </summary>
        /// <param name="token">Token can be obtained from the <a href="https://app.testproject.io/#/integrations/sdk">SDK</a> page.</param>
        /// <returns>Modified DriverBuilder instance.</returns>
        public DriverBuilder<T> WithToken(string token)
        {
            this.builderToken = token;
            return this;
        }

        /// <summary>
        /// Set an Agent API base URL (e.g. http://localhost:8585/).
        /// </summary>
        /// <param name="remoteAddress">URL to be set.</param>
        /// <returns>Modified DriverBuilder instance.</returns>
        public DriverBuilder<T> WithRemoteAddress(Uri remoteAddress)
        {
            this.builderRemoteAddress = remoteAddress;
            return this;
        }

        /// <summary>
        /// Set Project name to report.
        /// </summary>
        /// <param name="projectName">Project name to be set.</param>
        /// <returns>Modified DriverBuilder instance.</returns>
        public DriverBuilder<T> WithProjectName(string projectName)
        {
            this.builderProjectName = projectName;
            return this;
        }

        /// <summary>
        /// Set Job name to report.
        /// </summary>
        /// <param name="jobName">Job name to be set..</param>
        /// <returns>Modified DriverBuilder instance.</returns>
        public DriverBuilder<T> WithJobName(string jobName)
        {
            this.builderJobName = jobName;
            return this;
        }

        /// <summary>
        /// Enable/Disable reports flag.
        /// </summary>
        /// <param name="disableReports">Flag for disabling/enabling reports.</param>
        /// <returns>Modified DriverBuilder instance.</returns>
        public DriverBuilder<T> WithDisableReports(bool disableReports)
        {
            this.builderDisableReports = disableReports;
            return this;
        }

        /// <summary>
        /// Set Report Type to Cloud, Local or Both.
        /// </summary>
        /// <param name="reportType">Type of Report to generate.</param>
        /// <returns>Modified DriverBuilder instance.</returns>
        public DriverBuilder<T> WithReportType(ReportType reportType)
        {
            this.builderReportType = reportType;
            return this;
        }

        /// <summary>
        /// Builds an instance of the requested driver using set values.
        /// </summary>
        /// <returns>Driver instance.</returns>
        public T Build()
        {
            try
            {
                return (T)Activator.CreateInstance(
                    typeof(T),
                    this.builderRemoteAddress,
                    this.builderToken,
                    this.builderOptions,
                    this.builderProjectName,
                    this.builderJobName,
                    this.builderDisableReports,
                    this.builderReportType);
            } catch (Exception)
            {
                throw new WebDriverException($"Failed to create an instance of {typeof(T)}");
            }
        }
    }
}

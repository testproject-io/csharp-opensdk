// <copyright file="SessionRequest.cs" company="TestProject">
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

namespace TestProject.OpenSDK.Internal.Rest.Messages
{
    using System.Collections.Generic;
    using OpenQA.Selenium;
    using TestProject.OpenSDK.Drivers.Generic;
    using TestProject.OpenSDK.Enums;
    using TestProject.OpenSDK.Internal.Helpers;

    /// <summary>
    /// Payload object sent to the Agent to start a development session.
    /// </summary>
    public class SessionRequest
    {
        /// <summary>
        /// Capabilities that should be sent to the Agent for driver initialization.
        /// </summary>
        public Dictionary<string, object> Capabilities { get; }

        /// <summary>
        /// The current SDK version.
        /// </summary>
        public string SdkVersion { get; } = VersionHelper.GetSdkVersion();

        /// <summary>
        /// The SDK language, always C#.
        /// </summary>
        public string Language { get; } = "CSharp";

        /// <summary>
        /// The project name to report.
        /// </summary>
        public string ProjectName { get; }

        /// <summary>
        /// The job name to report.
        /// </summary>
        public string JobName { get; }

        /// <summary>
        /// The report type, can be local, cloud or both.
        /// </summary>
        public ReportType ReportType { get; }

        /// <summary>
        /// The name of local generated report.
        /// </summary>
        public string ReportName { get; }

        /// <summary>
        /// The path of local generated report.
        /// </summary>
        public string ReportPath { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionRequest"/> class.
        /// </summary>
        /// <param name="reportSettings">Report settings with the project and job names to report.</param>
        /// <param name="capabilities">Capabilities that should be sent to the Agent for driver initialization.</param>
        public SessionRequest(ReportSettings reportSettings, DriverOptions capabilities)
        {
            if (reportSettings != null)
            {
                this.ProjectName = reportSettings.ProjectName;
                this.JobName = reportSettings.JobName;
                this.ReportType = reportSettings.ReportType;
                this.ReportName = reportSettings.ReportName;
                this.ReportPath = reportSettings.ReportPath;
            }

            // Convert DriverOptions to a format that preserves arguments and extensions when serializing it.
            if (capabilities.GetType().Equals(typeof(GenericOptions)))
            {
                this.Capabilities = ((GenericOptions)capabilities).ToDictionary();
            }
            else
            {
                this.Capabilities = capabilities.ToString().FromJson<Dictionary<string, object>>();
            }
        }

        /// <summary>
        /// Checks if two objects are equal.
        /// </summary>
        /// <param name="obj">The object to compare the current object with.</param>
        /// <returns>true if the objects are equal, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }

            if (obj == null || obj.GetType() != this.GetType())
            {
                return false;
            }

            ReportSettings that = (ReportSettings)obj;

            return object.Equals(this.ProjectName, that.ProjectName) && object.Equals(this.JobName, that.JobName);
        }

        /// <summary>
        /// Calculates the object hashcode.
        /// </summary>
        /// <returns>The calculated hashcode.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}

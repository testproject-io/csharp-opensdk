// <copyright file="ReportSettings.cs" company="TestProject">
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

namespace TestProject.SDK.Internal.Rest
{
    /// <summary>
    /// Report settings model provided to the Agent upon session initialization.
    /// </summary>
    public class ReportSettings
    {
        /// <summary>
        /// The project name to report.
        /// </summary>
        public string ProjectName { get; }

        /// <summary>
        /// The job name to report.
        /// </summary>
        public string JobName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportSettings"/> class.
        /// </summary>
        /// <param name="projectName">The project name to report.</param>
        /// <param name="jobName">The job name to report.</param>
        public ReportSettings(string projectName, string jobName)
        {
            this.ProjectName = projectName;
            this.JobName = jobName;
        }
    }
}

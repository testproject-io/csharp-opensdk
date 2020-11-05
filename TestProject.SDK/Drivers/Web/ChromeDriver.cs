﻿// <copyright file="ChromeDriver.cs" company="TestProject">
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

using OpenQA.Selenium.Chrome;

namespace TestProject.SDK.Drivers.Web
{
    /// <summary>
    /// Extension of <see cref="OpenQA.Selenium.Chrome.ChromeDriver">ChromeDriver</see> for use with TestProject.
    /// Instead of initializing a new session, it starts it in the TestProject Agent and then reconnects to it.
    /// </summary>
    public class ChromeDriver : OpenQA.Selenium.Chrome.ChromeDriver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChromeDriver"/> class.
        /// </summary>
        /// <param name="remoteAddress">The base address for the Agent API (e.g. http://localhost:8585).</param>
        /// <param name="token">The development token used to communicate with the Agent, see <a href="https://app.testproject.io/#/integrations/sdk">here</a> for more info.</param>
        /// <param name="chromeOptions">See <see cref="OpenQA.Selenium.Chrome.ChromeOptions"/> for more details.</param>
        /// <param name="projectName">The project name to report.</param>
        /// <param name="jobName">The job name to report.</param>
        /// <param name="disableReports">Set to true to disable all reporting (no report will be created on TestProject).</param>
        public ChromeDriver(
            string remoteAddress = null,
            string token = null,
            ChromeOptions chromeOptions = null,
            string projectName = null,
            string jobName = null,
            bool disableReports = false)
            : base(ChromeDriverService.CreateDefaultService(string.Empty, string.Empty), chromeOptions)
        {
        }
    }
}

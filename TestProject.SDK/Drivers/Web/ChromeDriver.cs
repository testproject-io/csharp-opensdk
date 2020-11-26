// <copyright file="ChromeDriver.cs" company="TestProject">
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
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TestProject.SDK.Internal.Helpers.Threading;
using TestProject.SDK.Internal.Rest;

namespace TestProject.SDK.Drivers.Web
{
    /// <summary>
    /// Extension of <see cref="OpenQA.Selenium.Chrome.ChromeDriver">ChromeDriver</see> for use with TestProject.
    /// Instead of initializing a new session, it starts it in the TestProject Agent and then reconnects to it.
    /// </summary>
    public class ChromeDriver : OpenQA.Selenium.Chrome.ChromeDriver
    {
        private DriverShutdownThread driverShutdownThread;

        private static Logger Logger { get; set; } = LogManager.GetCurrentClassLogger();

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
            string remoteAddress = "http://localhost:8585",  // TODO: replace with proper logic
            string token = null,
            ChromeOptions chromeOptions = null,
            string projectName = null,
            string jobName = null,
            bool disableReports = false)
        {
            base.Quit(); // TODO: see if there's a better way to do this. We need to kill the locally started instance before connecting to the instance that the Agent provides us with.

            if (chromeOptions == null)
            {
                // These values are set because C# ChromeOptions defaults are not understood by the driver
                // They are equal to the values that the Agent would assign if these properties were not specified
                chromeOptions = new ChromeOptions();
                chromeOptions.UnhandledPromptBehavior = UnhandledPromptBehavior.DismissAndNotify;
                chromeOptions.PageLoadStrategy = PageLoadStrategy.Normal;
            }

            new AgentClient(new System.Uri(remoteAddress), token, chromeOptions, new ReportSettings(projectName, jobName), disableReports);

            this.driverShutdownThread = new DriverShutdownThread(this);
            AppDomain.CurrentDomain.ProcessExit += (sender, eventArgs) => this.driverShutdownThread.RunThread();
        }

        /// <summary>
        /// Quits the driver and stops the session with the Agent, cleaning up after itself.
        /// </summary>
        public new void Quit()
        {
            // Avoid performing the graceful shutdown more than once
            AppDomain.CurrentDomain.ProcessExit -= (sender, eventArgs) => this.driverShutdownThread.RunThread();

            this.Stop();
        }

        /// <summary>
        /// Sends any pending reports and closes the browser session.
        /// </summary>
        public void Stop()
        {
            // TODO: add reporting pending reports
            base.Quit();
        }
    }
}

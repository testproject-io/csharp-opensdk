// <copyright file="BaseDriver.cs" company="TestProject">
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
    using System.Collections.Generic;
    using NLog;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Remote;
    using TestProject.OpenSDK.Internal.Addons;
    using TestProject.OpenSDK.Internal.CallStackAnalysis;
    using TestProject.OpenSDK.Internal.Helpers;
    using TestProject.OpenSDK.Internal.Helpers.CommandExecutors;
    using TestProject.OpenSDK.Internal.Helpers.Threading;
    using TestProject.OpenSDK.Internal.Reporting;
    using TestProject.OpenSDK.Internal.Rest;

    /// <summary>
    /// Extension of <see cref="OpenQA.Selenium.Chrome.ChromeDriver">ChromeDriver</see> for use with TestProject.
    /// Instead of initializing a new session, it starts it in the TestProject Agent and then reconnects to it.
    /// </summary>
    public class BaseDriver : RemoteWebDriver, ITestProjectDriver
    {
        private const string KeepSessionEnvironmentVariable = "TP_KEEP_DRIVER_SESSION";
        private static readonly bool KeepDriverSession;

        /// <summary>
        /// Flag that indicates whether or not the driver instance is running.
        /// </summary>
        public bool IsRunning { get; private set; }

        private readonly DriverShutdownThread driverShutdownThread;

        private readonly string sessionId;

        private readonly CustomHttpCommandExecutor commandExecutor;

        /// <summary>
        /// Logger instance for this class.
        /// </summary>
        private static Logger Logger { get; set; } = LogManager.GetCurrentClassLogger();

        static BaseDriver()
        {
            var keepSessionVariable = Environment.GetEnvironmentVariable(KeepSessionEnvironmentVariable);
            bool.TryParse(keepSessionVariable, out bool result);
            KeepDriverSession = result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDriver"/> class.
        /// </summary>
        /// <param name="remoteAddress">The base address for the Agent API (e.g. http://localhost:8585).</param>
        /// <param name="token">The development token used to communicate with the Agent, see <a href="https://app.testproject.io/#/integrations/sdk">here</a> for more info.</param>
        /// <param name="driverOptions">See <see cref="DriverOptions"/> for more details.</param>
        /// <param name="projectName">The project name to report.</param>
        /// <param name="jobName">The job name to report.</param>
        /// <param name="disableReports">Set to true to disable all reporting (no report will be created on TestProject).</param>
        protected BaseDriver(
            Uri remoteAddress = null,
            string token = null,
            DriverOptions driverOptions = null,
            string projectName = null,
            string jobName = null,
            bool disableReports = false)
            : base(
                  AgentClient.GetInstance(remoteAddress, token, driverOptions, new ReportSettings(projectName, jobName), disableReports).AgentSession.Capabilities)
        {
            this.sessionId = AgentClient.GetInstance().AgentSession.SessionId;

            // Set the session ID for the base driver object to the session ID returned by the Agent.
            var sessionIdBase = this.GetType()
                .BaseType
                .GetField(
                    "sessionId",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            sessionIdBase.SetValue(this, this.sessionId);

            // Create a new command executor for this driver session and set disable reporting flag
            this.commandExecutor = new CustomHttpCommandExecutor(AgentClient.GetInstance().AgentSession.RemoteAddress, disableReports);

            this.IsRunning = true;

            // Add shutdown hook for gracefully shutting down the driver
            this.driverShutdownThread = new DriverShutdownThread(this);
            AppDomain.CurrentDomain.ProcessExit += (sender, eventArgs) => this.driverShutdownThread.RunThread();

            if (StackTraceHelper.Instance.TryDetectSpecFlow())
            {
                Logger.Info("SpecFlow detected, applying SpecFlow-specific reporting settings...");
                this.Report().DisableCommandReports(true);
                this.Report().DisableAutoTestReports(true);
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
        /// Enables access to the TestProject addon execution actions from the driver object.
        /// </summary>
        /// <returns><see cref="AddonHelper"/> object exposing TestProject action execution methods.</returns>
        public AddonHelper Addons()
        {
            return new AddonHelper();
        }

        /// <summary>
        /// Quits the driver and stops the session with the Agent, cleaning up after itself.
        /// </summary>
        public new void Quit()
        {
            if (this.IsRunning)
            {
                // Avoid performing the graceful shutdown more than once
                AppDomain.CurrentDomain.ProcessExit -= (sender, eventArgs) => this.driverShutdownThread.RunThread();

                this.Stop();
            }
            else
            {
                Logger.Info("Driver is not running, skipping shutdown sequence");
            }
        }

        /// <summary>
        /// Sends any pending reports and closes the browser session.
        /// </summary>
        public void Stop()
        {
            // Report any stashed commands
            this.commandExecutor.ReportingCommandExecutor.ClearStash();

            this.IsRunning = false;

            // Need to keep session alive if TP_KEEP_DRIVER_SESSION was specified.
            if (!KeepDriverSession)
            {
                base.Quit();
            }
        }

        /// <summary>
        /// Overrides the base Execute() method by redirecting the WebDriver command to our own command executor.
        /// </summary>
        /// <param name="driverCommandToExecute">The WebDriver command to execute.</param>
        /// <param name="parameters">Contains the parameters associated with this command.</param>
        /// <returns>The response returned by the Agent upon requesting to execute this command.</returns>
        protected override Response Execute(string driverCommandToExecute, Dictionary<string, object> parameters)
        {
            if (driverCommandToExecute.Equals(DriverCommand.NewSession))
            {
                var resp = new Response();
                resp.Status = WebDriverResult.Success;
                resp.SessionId = this.sessionId;
                resp.Value = new Dictionary<string, object>();
                return resp;
            }

            // The Agent does not understand the default way Selenium sends the driver command parameters for SendKeys
            // This means we'll need to patch them so these commands can be executed.
            if (driverCommandToExecute.ShouldBePatched())
            {
                parameters = CommandHelper.UpdateSendKeysParameters(parameters);
            }

            Command command = new Command(new SessionId(this.sessionId), driverCommandToExecute, parameters);

            Response commandResponse = this.commandExecutor.Execute(command);

            return commandResponse;
        }
    }
}

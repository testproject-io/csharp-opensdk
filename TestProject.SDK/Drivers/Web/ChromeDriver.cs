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

using System;
using System.Collections.Generic;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using TestProject.SDK.Internal.Helpers;
using TestProject.SDK.Internal.Helpers.CommandExecutors;
using TestProject.SDK.Internal.Helpers.Threading;
using TestProject.SDK.Internal.Rest;

namespace TestProject.SDK.Drivers.Web
{
    /// <summary>
    /// Extension of <see cref="OpenQA.Selenium.Chrome.ChromeDriver">ChromeDriver</see> for use with TestProject.
    /// Instead of initializing a new session, it starts it in the TestProject Agent and then reconnects to it.
    /// </summary>
    public class ChromeDriver : OpenQA.Selenium.Remote.RemoteWebDriver
    {
        private DriverShutdownThread driverShutdownThread;

        private string sessionId;

        private AgentClient agentClient;

        private CustomHttpCommandExecutor commandExecutor;

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
            : base(
                  new System.Uri(remoteAddress),
                  AgentClient.GetInstance(new System.Uri(remoteAddress), token, chromeOptions, new ReportSettings(projectName, jobName), disableReports).AgentSession.Capabilities)
        {
            this.sessionId = AgentClient.GetInstance().AgentSession.SessionId;

            // Set the session ID for the base driver object to the session ID returned by the Agent.
            var sessionIdBase = this.GetType()
                .BaseType
                .GetField(
                    "sessionId",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            sessionIdBase.SetValue(this, new OpenQA.Selenium.Remote.SessionId(this.sessionId));

            this.commandExecutor = new CustomHttpCommandExecutor(this.agentClient, AgentClient.GetInstance().AgentSession.RemoteAddress);

            // Add shutdown hook for gracefully shutting down the driver
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

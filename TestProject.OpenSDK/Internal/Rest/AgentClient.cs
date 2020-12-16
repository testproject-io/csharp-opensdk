// <copyright file="AgentClient.cs" company="TestProject">
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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using OpenQA.Selenium;
using RestSharp;
using TestProject.OpenSDK.Internal.Exceptions;
using TestProject.OpenSDK.Internal.Helpers;
using TestProject.OpenSDK.Internal.Rest.Messages;
using TestProject.OpenSDK.Internal.Tcp;

namespace TestProject.OpenSDK.Internal.Rest
{
    /// <summary>
    /// Client used to communicate with the TestProject Agent process.
    /// </summary>
    public class AgentClient
    {
        /// <summary>
        /// Class member to store Agent session details.
        /// </summary>
        public AgentSession AgentSession { get; private set; }

        /// <summary>
        /// A singleton instance of the <see cref="AgentClient"/> class.
        /// </summary>
        private static AgentClient instance;

        /// <summary>
        /// Name of the environment variable that stores the development token.
        /// </summary>
        private readonly string tpDevToken = "TP_DEV_TOKEN";

        /// <summary>
        /// Name of the environment variable that stores the Agent address.
        /// </summary>
        private readonly string tpAgentAddress = "TP_AGENT_URL";

        /// <summary>
        /// The default Agent address to be used if no address is specified in the driver constructor or environment variable.
        /// </summary>
        private readonly string agentDefaultAddress = "http://localhost:8585";

        /// <summary>
        /// Minimum Agent version that support session reuse.
        /// </summary>
        private readonly Version minSessionReuseCapableVersion = new Version("0.64.32");

        /// <summary>
        /// The current version of the Agent in use.
        /// </summary>
        private Version agentVersion;

        /// <summary>
        /// The remote address where the Agent is running.
        /// </summary>
        private Uri remoteAddress;

        /// <summary>
        /// The development token used to authenticate with the Agent.
        /// </summary>
        private string token;

        /// <summary>
        /// A REST client instance used to send HTTP requests to the Agent.
        /// </summary>
        private RestClient client;

        /// <summary>
        /// Configuration settings for (de-)serializing objects to / from JSON.
        /// </summary>
        private JsonSerializerSettings serializerSettings;

        /// <summary>
        /// The queue used to submit and report driver command, test and step reports.
        /// </summary>
        private ReportsQueue reportsQueue;

        /// <summary>
        /// Logger instance for this class.
        /// </summary>
        private static Logger Logger { get; set; } = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Returns a singleton instance of the <see cref="AgentClient"/>.
        /// </summary>
        /// <returns>A singleton instance of the <see cref="AgentClient"/>.</returns>
        public static AgentClient GetInstance()
        {
            return GetInstance(null, string.Empty, null, null, false);
        }

        /// <summary>
        /// If necessary, creates and then returns a singleton instance of the <see cref="AgentClient"/>.
        /// </summary>
        /// <param name="remoteAddress">The remote address where the Agent is running.</param>
        /// <param name="token">The development token used to authenticate with the Agent.</param>
        /// <param name="capabilities">Requested driver options for the browser session.</param>
        /// <param name="reportSettings">Contains the project and job name to report to TestProject.</param>
        /// <param name="disableReports">Set to true to disable all reporting to TestProject, false otherwise.</param>
        /// <returns>A singleton instance of the <see cref="AgentClient"/>.</returns>
        public static AgentClient GetInstance(Uri remoteAddress, string token, DriverOptions capabilities, ReportSettings reportSettings, bool disableReports)
        {
            if (instance == null)
            {
                instance = new AgentClient(remoteAddress, token, capabilities, reportSettings, disableReports);
            }

            return instance;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentClient"/> class.
        /// </summary>
        /// <param name="remoteAddress">The remote address where the Agent is running.</param>
        /// <param name="token">The development token used to authenticate with the Agent.</param>
        /// <param name="capabilities">Requested driver options for the browser session.</param>
        /// <param name="reportSettings">Contains the project and job name to report to TestProject.</param>
        /// <param name="disableReports">Set to true to disable all reporting to TestProject, false otherwise.</param>
        private AgentClient(Uri remoteAddress, string token, DriverOptions capabilities, ReportSettings reportSettings, bool disableReports)
        {
            this.remoteAddress = this.InferRemoteAddress(remoteAddress);

            if (token != null)
            {
                this.token = token;
            }
            else if (Environment.GetEnvironmentVariable(this.tpDevToken) != null)
            {
                this.token = Environment.GetEnvironmentVariable(this.tpDevToken);
            }
            else
            {
                throw new InvalidTokenException("No token has been provided.");
            }

            this.client = new RestClient(this.remoteAddress);
            this.client.AddDefaultHeader("Authorization", this.token);

            this.serializerSettings = CustomJsonSerializer.Populate(new JsonSerializerSettings());

            this.reportsQueue = new ReportsQueue(this.client);

            this.StartSession(reportSettings, capabilities);
        }

        /// <summary>
        /// Reports a WebDriver command to TestProject.
        /// </summary>
        /// <param name="driverCommandReport">Payload object containing command information and execution result.</param>
        public void ReportDriverCommand(DriverCommandReport driverCommandReport)
        {
            RestRequest sendDriverCommandRequest = new RestRequest(Endpoints.REPORT_COMMAND, Method.POST);
            sendDriverCommandRequest.RequestFormat = DataFormat.Json;

            string json = CustomJsonSerializer.ToJson(driverCommandReport, this.serializerSettings);

            sendDriverCommandRequest.AddJsonBody(json);

            this.reportsQueue.Submit(sendDriverCommandRequest, driverCommandReport);
        }

        /// <summary>
        /// Sends a <see cref="TestReport"/> to the Agent.
        /// </summary>
        /// <param name="testReport">The payload object containing the test details to be reported.</param>
        public void ReportTest(TestReport testReport)
        {
            RestRequest sendTestReportRequest = new RestRequest(Endpoints.REPORT_TEST, Method.POST);
            sendTestReportRequest.RequestFormat = DataFormat.Json;

            string json = CustomJsonSerializer.ToJson(testReport, this.serializerSettings);

            sendTestReportRequest.AddJsonBody(json);

            this.reportsQueue.Submit(sendTestReportRequest, testReport);
        }

        /// <summary>
        /// Sends a <see cref="StepReport"/> to the Agent.
        /// </summary>
        /// <param name="stepReport">The payload object containing the step details to be reported.</param>
        public void ReportStep(StepReport stepReport)
        {
            RestRequest sendStepReportRequest = new RestRequest(Endpoints.REPORT_STEP, Method.POST);
            sendStepReportRequest.RequestFormat = DataFormat.Json;

            string json = CustomJsonSerializer.ToJson(stepReport, this.serializerSettings);

            sendStepReportRequest.AddJsonBody(json);

            this.reportsQueue.Submit(sendStepReportRequest, stepReport);
        }

        /// <summary>
        /// Stops the current development session with the Agent.
        /// </summary>
        public void Stop()
        {
            this.reportsQueue.Stop();

            if (!this.CanReuseSession())
            {
                SocketManager.GetInstance().CloseSocket();
            }
        }

        /// <summary>
        /// Starts a new session with the Agent.
        /// </summary>
        /// <param name="reportSettings">Settings (project name, job name) to be included in the report.</param>
        /// <param name="capabilities">Additional options to be applied to the driver instance.</param>
        private void StartSession(ReportSettings reportSettings, DriverOptions capabilities)
        {
            RestRequest startSessionRequest = new RestRequest(Endpoints.DEVELOPMENT_SESSION, Method.POST);
            startSessionRequest.RequestFormat = DataFormat.Json;

            string json = CustomJsonSerializer.ToJson(new SessionRequest(reportSettings, capabilities), this.serializerSettings);

            startSessionRequest.AddJsonBody(json);

            IRestResponse startSessionResponse = this.client.Execute(startSessionRequest);

            if ((int)startSessionResponse.StatusCode >= 400)
            {
                this.HandleSessionStartFailure(startSessionResponse);
                return;
            }

            SessionResponse sessionResponse = CustomJsonSerializer.FromJson<SessionResponse>(startSessionResponse.Content, this.serializerSettings);

            Logger.Info($"Session [{sessionResponse.SessionId}] initialized");

            this.AgentSession = new AgentSession(new Uri(sessionResponse.ServerAddress), sessionResponse.SessionId, sessionResponse.Dialect, sessionResponse.Capabilities);

            SocketManager.GetInstance().OpenSocket(this.remoteAddress.Host, sessionResponse.DevSocketPort);

            this.agentVersion = this.GetAgentVersion();
        }

        /// <summary>
        /// Handle any failures that might occur whenever the session request results in an error returned by the Agent.
        /// </summary>
        /// <param name="startSessionResponse">The session response object returned by the Agent.</param>
        private void HandleSessionStartFailure(IRestResponse startSessionResponse)
        {
            JObject responseBody = JObject.Parse(startSessionResponse.Content);
            string responseMessage = string.Empty;

            if (responseBody.ContainsKey("message"))
            {
                responseMessage = (string)responseBody.GetValue("message");
            }
            else
            {
                Logger.Error("Failed to read message from Agent response");
            }

            switch ((int)startSessionResponse.StatusCode)
            {
                case 401:
                    Logger.Error("Failed to initialize a session with the Agent - invalid developer token supplied");
                    Logger.Error("Get your developer token from https://app.testproject.io/#/integrations/sdk?lang=CSharp" +
                        " and set it in the TP_DEV_TOKEN environment variable");
                    throw new InvalidTokenException(responseMessage);
                case 404:
                    Logger.Error("Failed to initialize a session with the Agent - requested browser not found on this system");
                    throw new MissingBrowserException(responseMessage);
                case 406:
                    Logger.Error("Failed to initialize a session with the Agent - obsolete SDK version");
                    throw new ObsoleteVersionException(responseMessage);
                default:
                    Logger.Error("Failed to initialize a session with the Agent");
                    throw new AgentConnectException($"Agent responsed with status code {(int)startSessionResponse.StatusCode}: [{responseMessage}]");
            }
        }

        /// <summary>
        /// Retrieves the version of the Agent currently in use.
        /// </summary>
        /// <returns>An instance of the <see cref="Version"/> class containing the Agent version.</returns>
        private Version GetAgentVersion()
        {
            RestRequest getAgentStatusRequest = new RestRequest(Endpoints.STATUS, Method.GET);

            IRestResponse getAgentStatusResponse = this.client.Execute(getAgentStatusRequest);

            if ((int)getAgentStatusResponse.StatusCode >= 400)
            {
                throw new AgentConnectException($"Failed to get Agent status: {getAgentStatusResponse.ErrorMessage}");
            }

            AgentStatusResponse agentStatusResponse = CustomJsonSerializer.FromJson<AgentStatusResponse>(getAgentStatusResponse.Content, this.serializerSettings);

            Logger.Info($"Current Agent version is {agentStatusResponse.Tag}");

            return new Version(agentStatusResponse.Tag);
        }

        /// <summary>
        /// Indicates whether the current Agent version supports session reuse.
        /// </summary>
        /// <returns>True if the Agent version supports session reuse, false otherwise.</returns>
        private bool CanReuseSession()
        {
            if (this.agentVersion == null)
            {
                return false;
            }

            // a.CompareTo(b) returns:
            // * -1 if a is earlier than b
            // *  0 if a is the same version as b
            // *  1 is a is later than b
            return this.agentVersion.CompareTo(this.minSessionReuseCapableVersion) >= 0;
        }

        private Uri InferRemoteAddress(Uri originalUri)
        {
            if (originalUri != null)
            {
                return originalUri;
            }

            if (Environment.GetEnvironmentVariable(this.tpAgentAddress) != null)
            {
                return new Uri(Environment.GetEnvironmentVariable(this.tpAgentAddress));
            }
            else
            {
                return new Uri(this.agentDefaultAddress);
            }
        }

        /// <summary>
        /// Internal class used to store Agent API endpoints.
        /// </summary>
        internal static class Endpoints
        {
            /// <summary>
            /// Endpoint for retrieving Agent status info.
            /// </summary>
            public const string STATUS = "/api/status";

            /// <summary>
            /// Endpoint for starting a new development session.
            /// </summary>
            public const string DEVELOPMENT_SESSION = "/api/development/session";

            /// <summary>
            /// Endpoint for reporting a driver command.
            /// </summary>
            public const string REPORT_COMMAND = "/api/development/report/command";

            /// <summary>
            /// Endpoint for reporting a test.
            /// </summary>
            public const string REPORT_TEST = "/api/development/report/test";

            /// <summary>
            /// Endpoint for reporting a step.
            /// </summary>
            public const string REPORT_STEP = "/api/development/report/step";
        }
    }
}

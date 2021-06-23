// <copyright file="ReportsQueue.cs" company="TestProject">
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

namespace TestProject.OpenSDK.Internal
{
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;
    using NLog;
    using RestSharp;
    using TestProject.OpenSDK.Internal.Rest.Messages;

    /// <summary>
    /// A queue class managing the sending of report items to the Agent in a separate thread.
    /// </summary>
    public class ReportsQueue
    {
        /// <summary>
        /// Maximum amount of time to wait in milliseconds before forcibly terminating the queue.
        /// </summary>
        private const int REPORTS_QUEUE_TIMEOUT = 1000 * 10 * 60; // 10 minutes

        /// <summary>
        /// Progress report delay in milliseconds.
        /// </summary>
        private const int PROGRESS_REPORT_DELAY = 3000;

        /// <summary>
        /// HTTP client used to send reports to the Agent.
        /// </summary>
        private RestClient client;

        /// <summary>
        /// Queue that will hold the items to be reported.
        /// </summary>
        private BlockingCollection<QueueItem> reportItems = new BlockingCollection<QueueItem>();

        /// <summary>
        /// Thread that will take care of reporting while test execution continues.
        /// </summary>
        private Thread reporterThread;

        /// <summary>
        /// True if the reporting queue is active, false otherwise.
        /// </summary>
        private bool running;

        /// <summary>
        /// Logger instance for this class.
        /// </summary>
        private static Logger Logger { get; set; } = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportsQueue"/> class.
        /// </summary>
        /// <param name="client">The <see cref="RestClient"/> HTTP client to send reports to the Agent.</param>
        public ReportsQueue(RestClient client)
        {
            this.client = client;
            this.reporterThread = new Thread(new ThreadStart(this.Worker));
            this.reporterThread.IsBackground = true;
            this.reporterThread.Start();
        }

        /// <summary>
        /// Adds a report to the queue.
        /// </summary>
        /// <param name="request">The <see cref="RestRequest"/> HTTP request to send to the Agent.</param>
        /// <param name="report">The report that this HTTP request contains.</param>
        public void Submit(RestRequest request, Report report)
        {
            this.reportItems.Add(new QueueItem(request, report));
        }

        /// <summary>
        /// Sends all remaining items to the Agent and stops the thread.
        /// </summary>
        public void Stop()
        {
            this.running = false;

            this.reportItems.Add(new QueueItem(null, null));

            this.reportItems.CompleteAdding();
            var task = Task.Run(async () => await this.LogItems());
        }

        private async Task Dowork()
        {
            while (this.reportItems.Count != 0)
            {
                await Task.Delay(PROGRESS_REPORT_DELAY);
                Logger.Info(
                    $"There are [{this.reportItems.Count}] outstanding reports that should be transmitted to the Agent before"
                             + " the process exits.");
            }

            Logger.Trace("Reporting queue is empty, stopping progress report...");
            return;
        }

        private async Task LogItems()
        {
            var task = this.Dowork();
            if (await Task.WhenAny(task, Task.Delay(REPORTS_QUEUE_TIMEOUT)) == task)
            {
                // Finished within timeout
                return;
            }
            else
            {
                // Took more time then configured timeout
                Logger.Warn($"There are {this.reportItems.Count} unreported items left in the queue.");
            }
        }

        /// <summary>
        /// Worker method that polls the queue and waits for and sends new items to report.
        /// </summary>
        private void Worker()
        {
            this.running = true;

            while (this.running || this.reportItems.Count > 0)
            {
                foreach (QueueItem itemToReport in this.reportItems.GetConsumingEnumerable(CancellationToken.None))
                {
                    this.SendReport(itemToReport);
                }
            }
        }

        /// <summary>
        /// Submits a report to the Agent.
        /// </summary>
        /// <param name="itemToReport">The <see cref="QueueItem"/> to report to the Agent.</param>
        private void SendReport(QueueItem itemToReport)
        {
            if (itemToReport.Request == null && itemToReport.Report == null)
            {
                if (this.running)
                {
                    // These nulls are not OK, something went wrong preparing the report/request.
                    Logger.Error("An empty request and report were submitted to the queue");
                }

                // These nulls are OK, they were added by stop() method on purpose.
                return;
            }

            IRestResponse response = this.client.Execute(itemToReport.Request);

            if ((int)response.StatusCode >= 400)
            {
                Logger.Error($"Agent returned HTTP {(int)response.StatusCode} with message: {response.ErrorMessage}");
            }
        }

        /// <summary>
        /// Internal class to keep the HTTP request and contained report together in the queue.
        /// </summary>
        public class QueueItem
        {
            /// <summary>
            /// The HTTP request containing the report.
            /// </summary>
            public RestRequest Request { get; }

            /// <summary>
            /// The report that will be sent to the Agent.
            /// </summary>
            public Report Report { get; }

            /// <summary>
            /// Initializes a new instance of the <see cref="QueueItem"/> class.
            /// </summary>
            /// <param name="request">The <see cref="RestRequest"/> HTTP request to send to the Agent.</param>
            /// <param name="report">The report that this HTTP request contains.</param>
            public QueueItem(RestRequest request, Report report)
            {
                this.Request = request;
                this.Report = report;
            }
        }
    }
}

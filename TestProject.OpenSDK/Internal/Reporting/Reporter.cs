// <copyright file="Reporter.cs" company="TestProject">
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

using TestProject.OpenSDK.Internal.Helpers.CommandExecutors;
using TestProject.OpenSDK.Internal.Rest;
using TestProject.OpenSDK.Internal.Rest.Messages;

namespace TestProject.OpenSDK.Internal.Reporting
{
    /// <summary>
    /// Exposes reporting actions to the WebDriver object.
    /// </summary>
    public class Reporter
    {
        private CustomHttpCommandExecutor commandExecutor;

        /// <summary>
        /// Initializes a new instance of the <see cref="Reporter"/> class.
        /// </summary>
        /// <param name="commandExecutor">The command executor associated with the current WebDriver object.</param>
        public Reporter(CustomHttpCommandExecutor commandExecutor)
        {
            this.commandExecutor = commandExecutor;
        }

        /// <summary>
        /// Creates a test report and sends it to the <see cref="AgentClient"/>.
        /// </summary>
        /// <param name="name">The name of the test.</param>
        /// <param name="passed">True if the test should be marked as passed, false otherwise.</param>
        /// <param name="message">A message that goes with the test.</param>
        public void Test(string name, bool passed = true, string message = null)
        {
            TestReport testReport = new TestReport(name, passed, message);

            AgentClient.GetInstance().ReportTest(testReport);
        }

        /// <summary>
        /// Creates a step report and sends it to the <see cref="AgentClient"/>.
        /// </summary>
        /// <param name="description">The description of the step.</param>
        /// /// <param name="message">A message that goes with the step.</param>
        /// <param name="passed">True if the step should be marked as passed, false otherwise.</param>
        /// <param name="screenshot">True if a screenshot should be attached to the step, false otherwise.</param>
        public void Step(string description, string message = null, bool passed = true, bool screenshot = false)
        {
            // TODO: report a test if necessary
            string screenshotAsString = screenshot ? this.commandExecutor.GetScreenshot() : null;

            StepReport stepReport = new StepReport(description, message, passed, screenshotAsString);

            AgentClient.GetInstance().ReportStep(stepReport);
        }
    }
}

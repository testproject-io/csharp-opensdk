// <copyright file="TestProjectTestTracerWrapper.cs" company="TestProject">
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

namespace TestProject.SpecFlowPlugin
{
    using System;
    using NLog;
    using TechTalk.SpecFlow.Bindings;
    using TechTalk.SpecFlow.BindingSkeletons;
    using TechTalk.SpecFlow.Configuration;
    using TechTalk.SpecFlow.Tracing;
    using TestProject.OpenSDK.Internal.Exceptions;
    using TestProject.OpenSDK.Internal.Rest;
    using TestProject.OpenSDK.Internal.Rest.Messages;

    /// <summary>
    /// A custom wrapper around the SpecFlow test tracer, handling post-test execution reporting.
    /// </summary>
    public class TestProjectTestTracerWrapper : TestTracer, ITestTracer
    {
        /// <summary>
        /// Logger instance for this class.
        /// </summary>
        private static Logger Logger { get; set; } = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initializes a new instance of the <see cref="TestProjectTestTracerWrapper"/> class.
        /// </summary>
        /// <param name="traceListener">Two.</param>
        /// <param name="stepFormatter">Three.</param>
        /// <param name="stepDefinitionSkeletonProvider">Four.</param>
        /// <param name="specFlowConfiguration">Five.</param>
        public TestProjectTestTracerWrapper(
            ITraceListener traceListener,
            IStepFormatter stepFormatter,
            IStepDefinitionSkeletonProvider stepDefinitionSkeletonProvider,
            SpecFlowConfiguration specFlowConfiguration)
            : base(traceListener, stepFormatter, stepDefinitionSkeletonProvider, specFlowConfiguration)
        {
        }

        /// <summary>
        /// Report a passed step to TestProject when the SpecFlow step finishes executing.
        /// </summary>
        /// <param name="match">The step definition binding that was executed.</param>
        /// <param name="arguments">The step definitions arguments that were used.</param>
        /// <param name="duration">The duration of the step execution.</param>
        public new void TraceStepDone(BindingMatch match, object[] arguments, TimeSpan duration)
        {
            if (AgentClient.IsInitialized())
            {
                // Create a preliminary test report in the AgentClient (will not be reported until AgentClient is stopped).
                AgentClient.GetInstance().SpecFlowTestReport = new TestReport(TestProjectStepContext.CurrentScenario, true, null);

                string message = $"Step execution time: {duration}";

                StepReport stepReport = new StepReport(TestProjectStepContext.CurrentStep, message, true, null);
                AgentClient.GetInstance().ReportStep(stepReport);
            }
            else
            {
                string errorMessage = "No active Agent development session found.";

                Logger.Error(errorMessage);
                throw new AgentConnectException(errorMessage);
            }

            base.TraceStepDone(match, arguments, duration);
        }

        /// <summary>
        /// Report a failed step and test to TestProject when SpecFlow scenario execution produces an error.
        /// </summary>
        /// <param name="ex">The exception that was thrown during SpecFlow scenario execution.</param>
        public new void TraceError(Exception ex)
        {
            if (AgentClient.IsInitialized())
            {
                StepReport stepReport = new StepReport(TestProjectStepContext.CurrentStep, ex.Message, false, null);
                AgentClient.GetInstance().ReportStep(stepReport);

                // Create a preliminary test report in the AgentClient (will not be reported until AgentClient is stopped).
                AgentClient.GetInstance().SpecFlowTestReport = new TestReport(TestProjectStepContext.CurrentScenario, false, null);
            }
            else
            {
                string errorMessage = "No active Agent development session found.";

                Logger.Error(errorMessage);
                throw new AgentConnectException(errorMessage);
            }

            base.TraceError(ex);
        }
    }
}

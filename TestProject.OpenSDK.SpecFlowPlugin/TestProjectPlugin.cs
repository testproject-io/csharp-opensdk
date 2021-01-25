// <copyright file="TestProjectPlugin.cs" company="TestProject">
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

using NLog;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Plugins;
using TechTalk.SpecFlow.UnitTestProvider;
using TestProject.OpenSDK.Internal.Exceptions;
using TestProject.OpenSDK.Internal.Rest;
using TestProject.OpenSDK.Internal.Rest.Messages;
using TestProject.OpenSDK.SpecFlowPlugin;

[assembly: RuntimePlugin(typeof(TestProjectPlugin))]

namespace TestProject.OpenSDK.SpecFlowPlugin
{
    /// <summary>
    /// SpecFlow plugin to automatically report scenario steps to TestProject.
    /// </summary>
    public class TestProjectPlugin : IRuntimePlugin
    {
        private static Logger Logger { get; set; } = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Custom SpecFlow plugin taking care of automatically reporting SpecFlow scenario information to TestProject.
        /// </summary>
        /// <param name="runtimePluginEvents">Runtime plugin events.</param>
        /// <param name="runtimePluginParameters">Parameters for the runtime plugin events.</param>
        /// <param name="unitTestProviderConfiguration">Unit test provider configuration.</param>
        public void Initialize(
            RuntimePluginEvents runtimePluginEvents,
            RuntimePluginParameters runtimePluginParameters,
            UnitTestProviderConfiguration unitTestProviderConfiguration)
        {
            runtimePluginEvents.CustomizeGlobalDependencies += this.RuntimePluginEvents_CustomizeGlobalDependencies;
        }

        /// <summary>
        /// Adds our own AfterStep hook code to the methods that are executed after each step in a SpecFlow scenario.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="eventArgs">Arguments passed to the event.</param>
        private void RuntimePluginEvents_CustomizeGlobalDependencies(object sender, CustomizeGlobalDependenciesEventArgs eventArgs)
        {
            RuntimePluginTestExecutionLifecycleEvents runtimePluginTestExecutionLifecycleEvents = eventArgs.ObjectContainer.Resolve<RuntimePluginTestExecutionLifecycleEvents>();

            runtimePluginTestExecutionLifecycleEvents.AfterStep += this.RuntimePluginTestExecutionLifecycleEvents_AfterStep;
        }

        /// <summary>
        /// Reports steps and tests to the Agent.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="eventArgs">Arguments passed to the event.</param>
        private void RuntimePluginTestExecutionLifecycleEvents_AfterStep(object sender, RuntimePluginAfterStepEventArgs eventArgs)
        {
            if (AgentClient.IsInitialized())
            {
                ScenarioContext context = eventArgs.ObjectContainer.Resolve<ScenarioContext>();

                string scenarioTitle;

                if (context.ScenarioInfo.Arguments.Count > 0)
                {
                    // If a scenario has arguments, it means it is an example from a Scenario Outline
                    // Similar to what SpecFlow itself does, we append the value from the first argument
                    // to the name of the current scenario to uniquely identify it
                    scenarioTitle = $"{context.ScenarioInfo.Title} [{context.ScenarioInfo.Arguments[0]}]";
                }
                else
                {
                    // If a scenario does not have arguments, we consider it to be a 'regular' scenario
                    // In this case, we use the Scenario title as the name of the test
                    scenarioTitle = context.ScenarioInfo.Title;
                }

                // Create a preliminary test report in the AgentClient (will not be reported until AgentClient is stopped).
                AgentClient.GetInstance().SpecFlowTestReport = new TestReport(scenarioTitle, true, context.ScenarioInfo.Description);

                string stepText = $"{context.StepContext.StepInfo.StepDefinitionType} {context.StepContext.StepInfo.Text}";

                StepReport stepReport;

                if (context.TestError == null)
                {
                    stepReport = new StepReport(stepText, context.StepContext.StepInfo.MultilineText, true, null);
                }
                else
                {
                    stepReport = new StepReport(stepText, context.TestError.Message, false, null);
                }

                AgentClient.GetInstance().ReportStep(stepReport);
            }
            else
            {
                string message = $"No active Agent development session found. Please ensure that driver.Quit() is called in an [After] method, not in a step definition method.";
                
                Logger.Error(message);
                throw new AgentConnectException(message);
            }
        }
    }
}

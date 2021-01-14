// <copyright file="TestProjectBindingInvoker.cs" company="TestProject">
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
    using TechTalk.SpecFlow.Bindings;
    using TechTalk.SpecFlow.Configuration;
    using TechTalk.SpecFlow.ErrorHandling;
    using TechTalk.SpecFlow.Infrastructure;
    using TechTalk.SpecFlow.Tracing;

    /// <summary>
    /// Defines custom actions to be executed whenever specific binding methods are invoked.
    /// </summary>
    public class TestProjectBindingInvoker : BindingInvoker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestProjectBindingInvoker"/> class.
        /// </summary>
        /// <param name="specFlowConfiguration">The active SpecFlow configuration.</param>
        /// <param name="errorProvider">A reference to an error provider.</param>
        /// <param name="synchronousBindingDelegateInvoker">Delegate invoker.</param>
        public TestProjectBindingInvoker(
            SpecFlowConfiguration specFlowConfiguration,
            IErrorProvider errorProvider,
            ISynchronousBindingDelegateInvoker synchronousBindingDelegateInvoker)
            : base(specFlowConfiguration, errorProvider, synchronousBindingDelegateInvoker)
        {
        }

        /// <summary>
        /// Code to be executed whenever a SpecFlow step definition binding is invoked.
        /// </summary>
        /// <param name="binding">The step definition method that is executed.</param>
        /// <param name="contextManager">Container for context objects.</param>
        /// <param name="arguments">Binding arguments passed.</param>
        /// <param name="testTracer">Test tracer.</param>
        /// <param name="duration">The duration of the step definition execution.</param>
        /// <returns>Result of the step definition binding invocation.</returns>
        public override object InvokeBinding(
            IBinding binding,
            IContextManager contextManager,
            object[] arguments,
            ITestTracer testTracer,
            out TimeSpan duration)
        {
            if (binding is StepDefinitionBinding)
            {
                TestProjectStepContext.CurrentStep = $"{contextManager.StepContext.StepInfo.StepDefinitionType} {contextManager.StepContext.StepInfo.Text}";

                if (contextManager.ScenarioContext.ScenarioInfo.Arguments.Count > 0)
                {
                    // If a scenario has arguments, it means it is an example from a Scenario Outline
                    // Similar to what SpecFlow itself does, we append the value from the first argument
                    // to the name of the current scenario to uniquely identify it
                    TestProjectStepContext.CurrentScenario = $"{contextManager.ScenarioContext.ScenarioInfo.Title} [{contextManager.ScenarioContext.ScenarioInfo.Arguments[0]}]";
                }
                else
                {
                    // If a scenario does not have arguments, we consider it to be a 'regular' scenario
                    // In this case, we use the Scenario title as the name of the test
                    TestProjectStepContext.CurrentScenario = contextManager.ScenarioContext.ScenarioInfo.Title;
                }
            }

            return base.InvokeBinding(binding, contextManager, arguments, testTracer, out duration);
        }
    }
}

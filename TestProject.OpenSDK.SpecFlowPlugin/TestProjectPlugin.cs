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

using TechTalk.SpecFlow.Bindings;
using TechTalk.SpecFlow.Plugins;
using TechTalk.SpecFlow.Tracing;
using TechTalk.SpecFlow.UnitTestProvider;
using TestProject.OpenSDK.SpecFlowPlugin;

[assembly: RuntimePlugin(typeof(TestProjectPlugin))]

namespace TestProject.OpenSDK.SpecFlowPlugin
{
    /// <summary>
    /// SpecFlow plugin to automatically report scenario steps to TestProject.
    /// </summary>
    public class TestProjectPlugin : IRuntimePlugin
    {
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
            runtimePluginEvents.CustomizeGlobalDependencies += (sender, args) =>
                args.ObjectContainer.RegisterTypeAs<TestProjectBindingInvoker, IBindingInvoker>();

            runtimePluginEvents.CustomizeTestThreadDependencies += (sender, args) =>
                args.ObjectContainer.RegisterTypeAs<TestProjectTestTracerWrapper, ITestTracer>();
        }
    }
}

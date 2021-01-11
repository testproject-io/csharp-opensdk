// <copyright file="SpecFlowAnalyzer.cs" company="TestProject">
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

using System.Linq;
using System.Reflection;

namespace TestProject.OpenSDK.Internal.CallStackAnalysis
{
    /// <summary>
    /// Defines methods that are used to determine whether or not a method is run as part of a SpecFlow scenario.
    /// </summary>
    public class SpecFlowAnalyzer
    {
        private const string SpecFlowNamespace = "TechTalk.SpecFlow";

        /// <summary>
        /// Determines whether or not a method is running inside a SpecFlow scenario.
        /// </summary>
        /// <param name="method">The method to be analyzed.</param>
        /// <returns>True if the method is running inside a SpecFlow scenario, false otherwise.</returns>
        public bool IsSpecFlow(MethodBase method) =>
            method.DeclaringType.Namespace.StartsWith(SpecFlowNamespace);
    }
}

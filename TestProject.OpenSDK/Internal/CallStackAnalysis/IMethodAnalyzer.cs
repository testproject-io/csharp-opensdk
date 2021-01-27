// <copyright file="IMethodAnalyzer.cs" company="TestProject">
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

namespace TestProject.OpenSDK.Internal.CallStackAnalysis
{
    using System.Reflection;

    /// <summary>
    /// Defines methods that are used to determine whether or not a method belongs to a unit testing framework class.
    /// </summary>
    internal interface IMethodAnalyzer
    {
        /// <summary>
        /// Determines whether or not the class containing the method that is run belongs to a unit testing framework.
        /// </summary>
        /// <param name="method">The method to be analyzed.</param>
        /// <returns>True if the class containing the method is a test framework class, false otherwise.</returns>
        bool IsTestClass(MethodBase method);

        /// <summary>
        /// Determines whether or not the given method is run inside the setup hook of any of the supported unit testing frameworks.
        /// </summary>
        /// <param name="method">The method to be analyzed.</param>
        /// <returns>True if the method is run inside a setup hook, false otherwise.</returns>
        bool IsSetupMethod(MethodBase method);

        /// <summary>
        /// Gets the test name, taking into account custom display names specified by the user.
        /// </summary>
        /// <param name="method">The method to be analyzed.</param>
        /// <returns>Test name, or null if method not supported by this analyzer.</returns>
        string GetTestName(MethodBase method);
    }
}

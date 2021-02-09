// <copyright file="XUnitAnalyzer.cs" company="TestProject">
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
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Defines methods that are used to determine whether or not a method belongs to XUnit.
    /// </summary>
    public class XUnitAnalyzer : IMethodAnalyzer
    {
        private const string XUnitNamespace = "Xunit";
        private const string TestNameProperty = "DisplayName";
        private static readonly string[] AttributeNames = { "FactAttribute", "TheoryAttribute" };

        /// <summary>
        /// Determines whether or not the class containing the method that is run belongs to XUnit.
        /// </summary>
        /// <param name="method">The method to be analyzed.</param>
        /// <returns>True if the class containing the method is an XUnit class, false otherwise.</returns>
        public bool IsTestClass(MethodBase method) =>
            method.GetCustomAttributes(true)
                .Any(a => AttributeNames.Contains(a.GetType().Name) &&
                          (a.GetType().Namespace?.Equals(XUnitNamespace) ?? false));

        /// <summary>
        /// Determines whether or not the given method is run inside an xUnit.NET setup method.
        /// </summary>
        /// <param name="method">The method to be analyzed.</param>
        /// <returns>True if the method is run inside a setup method, false otherwise.</returns>
        public bool IsSetupMethod(MethodBase method)
        {
            // xUnit.NET does not support setup methods, so this is always false.
            return false;
        }

        /// <inheritdoc cref="IMethodAnalyzer"/>
        public string GetTestName(MethodBase method)
        {
            // Attribute has a DisplayName property set this way: [TestMethod(DisplayName = "name")]
            var attribute = method.GetCustomAttributes(true)
                .FirstOrDefault(a => AttributeNames.Contains(a.GetType().Name) &&
                          (a.GetType().Namespace?.Equals(XUnitNamespace) ?? false));

            return attribute?.GetType().GetProperty(TestNameProperty)?.GetValue(attribute)?.ToString();
        }
    }
}

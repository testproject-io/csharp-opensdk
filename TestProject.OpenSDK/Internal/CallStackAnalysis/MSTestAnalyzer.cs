// <copyright file="MSTestAnalyzer.cs" company="TestProject">
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
    /// Defines methods that are used to determine whether or not a method belongs to MSTest.
    /// </summary>
    public class MSTestAnalyzer : IMethodAnalyzer
    {
        private const string TestAttribute = "TestMethod";
        private const string SetUpAttribute = "TestInitialize";
        private const string TestClassAttribute = "TestClass";
        private const string MSTestFrameworkNamespace = "Microsoft.VisualStudio.TestTools.UnitTesting";
        private const string TestNameProperty = "DisplayName";

        /// <summary>
        /// Determines whether or not the class containing the method that is run belongs to MSTest.
        /// </summary>
        /// <param name="method">The method to be analyzed.</param>
        /// <returns>True if the class containing the method is an MSTest class, false otherwise.</returns>
        public bool IsTestClass(MethodBase method)
        {
            // MSTest tests are identified by [TestMethod] on the method and [TestClass] on the class (must be both)
            return method.GetCustomAttributes(true).Any(a => a.GetType().Name.Contains(TestAttribute)
            && a.GetType().Namespace.Equals(MSTestFrameworkNamespace))
                && method.DeclaringType.GetCustomAttributes(true).Any(a => a.GetType().Name.Contains(TestClassAttribute) && a.GetType().Namespace.Equals(MSTestFrameworkNamespace));
        }

        /// <summary>
        /// Determines whether or not the given method is run inside an MSTest [TestInitialize] method.
        /// </summary>
        /// <param name="method">The method to be analyzed.</param>
        /// <returns>True if the method is run inside [TestInitialize], false otherwise.</returns>
        public bool IsSetupMethod(MethodBase method)
        {
            // MSTest setup methods are identified by [TestInitialize] on the method and [TestClass] on the class (must be both)
            return method.GetCustomAttributes(true).Any(a => a.GetType().Name.Contains(SetUpAttribute)
            && a.GetType().Namespace.Equals(MSTestFrameworkNamespace))
                && method.DeclaringType.GetCustomAttributes(true).Any(a => a.GetType().Name.Contains(TestClassAttribute) && a.GetType().Namespace.Equals(MSTestFrameworkNamespace));
        }

        /// <inheritdoc cref="IMethodAnalyzer"/>
        public string GetTestName(MethodBase method)
        {
            // Attribute has a DisplayName property set this way: [TestMethod("name")]
            var attribute = method.GetCustomAttributes(true).FirstOrDefault(a =>
                a.GetType().Name.Contains(TestAttribute)
                && a.GetType().Namespace.Equals(MSTestFrameworkNamespace));
            return attribute?.GetType().GetProperty(TestNameProperty)?.GetValue(attribute)?.ToString();
        }

        /// <inheritdoc cref="IMethodAnalyzer"/>
        public string GetTestClassDescription(MethodBase method)
        {
            // MSTest does not support description attributes at the test class level.
            return null;
        }
    }
}

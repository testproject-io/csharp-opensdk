// <copyright file="StackTraceHelper.cs" company="TestProject">
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
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using OpenQA.Selenium.Support.UI;

    /// <summary>
    /// Helps analyzing stack trace.
    /// </summary>
    public class StackTraceHelper
    {
        private static readonly Lazy<StackTraceHelper> Lazy = new Lazy<StackTraceHelper>(() => new StackTraceHelper());

        /// <summary>
        /// Singleton instance of the <see cref="StackTraceHelper"/> class.
        /// </summary>
        public static StackTraceHelper Instance => Lazy.Value;

        private StackTraceHelper()
        {
        }

        private readonly IMethodAnalyzer[] analyzers = { new MSTestAnalyzer(), new NUnitAnalyzer(), new XUnitAnalyzer() };

        /// <summary>
        /// Infers the test method name from the current stack trace.
        /// </summary>
        /// <returns>The inferred test method name.</returns>
        public string GetInferredTestName()
        {
            MethodBase testMethod = this.TryDetectTestMethod();
            if (testMethod == null)
            {
                return null;
            }
            else
            {
                return this.analyzers.Select(a => a.GetTestName(testMethod)).FirstOrDefault(n => !string.IsNullOrEmpty(n))
                    ?? testMethod.Name;
            }
        }

        /// <summary>
        /// Infer the project name from the current stack trace.
        /// This is equivalent to the final part of the namespace that the class containing the test method is in.
        /// </summary>
        /// <returns>The inferred project name.</returns>
        public string GetInferredProjectName()
        {
            MethodBase method = this.TryDetectSetupMethod() ?? this.TryDetectTestMethod() ?? this.TryDetectConstructor() ?? this.GetTestMethod();

            return method.DeclaringType.Namespace.Split('.').Last();
        }

        /// <summary>
        /// Infers the job name from the current stack trace.
        /// </summary>
        /// <returns>The inferred job name.</returns>
        public string GetInferredJobName()
        {
            MethodBase testMethod = this.TryDetectSetupMethod() ?? this.TryDetectTestMethod() ?? this.TryDetectConstructor() ?? this.GetTestMethod();

            return this.analyzers.Select(a => a.GetTestClassDescription(testMethod)).FirstOrDefault(n => !string.IsNullOrEmpty(n))
                ?? testMethod.DeclaringType.Name;
        }

        /// <summary>
        /// Checks whether we are running inside <see cref="IWait{T}.Until{TResult}"/>.
        /// </summary>
        /// <returns>True if we are running inside a WebDriverWait, false otherwise.</returns>
        public bool IsRunningInsideWait()
        {
            var wait = typeof(IWait<>);
            return new StackTrace().GetFrames()
                .Any(f =>
                {
                    var declaringType = f.GetMethod().DeclaringType;
                    return declaringType != null &&
                           declaringType.GetInterfaces()
                               .Any(i => i.Name.StartsWith(wait.GetGenericTypeDefinition().Name));
                });
        }

        /// <summary>
        /// Checks if we're running inside a SpecFlow scenario.
        /// </summary>
        /// <returns>True if a SpecFlow reference is found in the stack trace, false otherwise.</returns>
        public bool TryDetectSpecFlow()
        {
            StackFrame[] stackFrames = new StackTrace().GetFrames();
            MethodBase callingMethod = stackFrames.Select(f => f.GetMethod()).FirstOrDefault(m => new SpecFlowAnalyzer().IsSpecFlow(m));

            // If SpecFlow was detected, callingMethod is not null.
            return callingMethod != null;
        }

        /// <summary>
        /// Checks if the TestProject SpecFlow plugin is referenced.
        /// </summary>
        /// <returns>True if SpecFlow Plugin reference is found, false otherwise.</returns>
        public bool IsSpecFlowPluginInstalled()
        {
            try
            {
                // This will fail if the assembly is not present.
                Assembly.Load("TestProject.OpenSDK.SpecFlowPlugin");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if we're running inside a test method (such as NUnit test) and returns said method
        /// If no such method is found, attempts to detect the method that called the runner.
        /// </summary>
        /// <returns>The test method or method that called the runner.</returns>
        private MethodBase TryDetectTestMethod()
        {
            StackFrame[] stackFrames = new StackTrace().GetFrames();
            MethodBase callingMethod = stackFrames.Select(f => f.GetMethod()).FirstOrDefault(m => this.analyzers.Any(a => a.IsTestClass(m)));

            if (callingMethod == null)
            {
                // Check to see if we are inside a setup or teardown method.
                // Commands executed inside a setup method should not be reported in a separate test,
                // this is why we return null in that case.
                MethodBase setupMethod = stackFrames.Select(f => f.GetMethod()).FirstOrDefault(m => this.analyzers.Any(a => a.IsSetupMethod(m)));

                if (setupMethod != null)
                {
                    return null;
                }
            }

            return callingMethod;
        }

        /// <summary>
        /// Checks if we're running inside a setup method and returns said method.
        /// </summary>
        /// <returns>The setup method currently running, or null if none was detected.</returns>
        private MethodBase TryDetectSetupMethod()
        {
            StackFrame[] stackFrames = new StackTrace().GetFrames();
            return stackFrames.Select(f => f.GetMethod()).FirstOrDefault(m => this.analyzers.Any(a => a.IsSetupMethod(m)));
        }

        /// <summary>
        /// Checks if we're running inside a (test) class constructor and returns said constructor.
        /// We always return the last constructor from the call stack, as that is the constructor of
        /// the actual test class where the project / job inferring initiated and whose data should be used.
        /// </summary>
        /// <returns>The constructor method currently running, or null if none was detected.</returns>
        private MethodBase TryDetectConstructor()
        {
            StackFrame[] stackFrames = new StackTrace().GetFrames();
            return stackFrames.Select(f => f.GetMethod()).LastOrDefault(m => m.MemberType.Equals(MemberTypes.Constructor));
        }

        /// <summary>
        /// Get the method that (presumably) contains the driver calls when no unit testing framework is found
        /// and the driver calls are not made from within a test class constructor.
        /// </summary>
        /// <returns>The method that (presumably) contains the driver calls.</returns>
        private MethodBase GetTestMethod()
        {
            // Select the first method in the call stack where the assembly is equal
            // to the assembly of the entry method (typically Main()).
            // This is assumed to be the method that contains the driver calls.
            StackFrame[] stackFrames = new StackTrace().GetFrames();
            Assembly currentAssembly = stackFrames.Last<StackFrame>().GetMethod().DeclaringType.Assembly;
            return stackFrames.Select(f => f.GetMethod()).FirstOrDefault(f => f.DeclaringType.Assembly.Equals(currentAssembly));
        }
    }
}

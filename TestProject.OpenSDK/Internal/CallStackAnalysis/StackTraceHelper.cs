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

using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using OpenQA.Selenium.Support.UI;

namespace TestProject.OpenSDK.Internal.CallStackAnalysis
{
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

        private readonly IMethodAnalyzer[] analyzers = { new MSTestAnalyzer() };

        /// <summary>
        /// Infers the test method name from the current stack trace.
        /// </summary>
        /// <returns>The inferred test method name.</returns>
        public string GetInferredTestName()
        {
            return this.TryDetectTestMethod().Name;
        }

        /// <summary>
        /// Infer the project name from the current stack trace.
        /// This is equivalent to the final part of the namespace that the class containing the test method is in.
        /// </summary>
        /// <returns>The inferred project name.</returns>
        public string GetInferredProjectName()
        {
            return this.TryDetectTestMethod().DeclaringType.Namespace.Split('.').Last();
        }

        /// <summary>
        /// Infers the job name from the current stack trace.
        /// </summary>
        /// <returns>The inferred job name.</returns>
        public string GetInferredJobName()
        {
            return this.TryDetectTestMethod().DeclaringType.Name;
        }

        /// <summary>
        /// Checks whether we are running inside <see cref="IWait{T}.Until{TResult}"/>.
        /// </summary>
        /// <returns>True if we are running inside a WebDriverWait, false otherwise.</returns>
        public bool IsRunningWithRepeats()
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
                callingMethod = stackFrames.Select(f => f.GetMethod()).FirstOrDefault(f => !f.DeclaringType.Assembly.Equals(Assembly.GetAssembly(this.GetType())));
            }

            return callingMethod;
        }
    }
}

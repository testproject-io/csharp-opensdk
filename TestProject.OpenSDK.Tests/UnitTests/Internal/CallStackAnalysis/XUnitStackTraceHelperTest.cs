// <copyright file="XUnitStackTraceHelperTest.cs" company="TestProject">
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

namespace TestProject.OpenSDK.Tests.UnitTests.Internal.CallStackAnalysis
{
    using TestProject.OpenSDK.Internal.CallStackAnalysis;
    using Xunit;

    /// <summary>
    /// Class containing unit tests for the <see cref="StackTraceHelper"/> class using xUnit.NET.
    /// </summary>
    public class XUnitStackTraceHelperTest
    {
        private readonly string projectNameFromConstructor;
        private readonly string jobNameFromConstructor;

        /// <summary>
        /// Initializes a new instance of the <see cref="XUnitStackTraceHelperTest"/> class.
        /// </summary>
        public XUnitStackTraceHelperTest()
        {
            this.projectNameFromConstructor = StackTraceHelper.Instance.GetInferredProjectName();
            this.jobNameFromConstructor = StackTraceHelper.Instance.GetInferredJobName();
        }

        /// <summary>
        /// Inferring the current test name should return the expected value.
        /// </summary>
        [Fact]
        public void GetInferredTestName_CheckResult_ShouldEqualCurrentTestMethodName()
        {
            string testName = StackTraceHelper.Instance.GetInferredTestName();

            Assert.Equal("GetInferredTestName_CheckResult_ShouldEqualCurrentTestMethodName", testName);
        }

        /// <summary>
        /// Inferring the current test name should return the expected value when using a description in the annotation.
        /// </summary>
        [Fact(DisplayName ="Custom test name")]
        public void GetInferredTestName__UsingCustomTestName_CheckResult_ShouldEqualCurrentTestMethodName()
        {
            string testName = StackTraceHelper.Instance.GetInferredTestName();

            Assert.Equal("Custom test name", testName);
        }

        /// <summary>
        /// Inferring the job name should return the expected value.
        /// </summary>
        [Fact]
        public void GetInferredJobName_CheckResult_ShouldEqualCurrentTestClass()
        {
            string jobName = StackTraceHelper.Instance.GetInferredJobName();

            Assert.Equal("XUnitStackTraceHelperTest", jobName);
        }

        /// <summary>
        /// Inferring the project name should return the expected value.
        /// </summary>
        [Fact]
        public void GetInferredProjectName_CheckResult_ShouldEqualFinalPartOfCurrentNamespace()
        {
            string projectName = StackTraceHelper.Instance.GetInferredProjectName();

            Assert.Equal("CallStackAnalysis", projectName);
        }

        /// <summary>
        /// Inferring the project name inside an XUnit test class constructor should return the expected value.
        /// </summary>
        [Fact]
        public void GetInferredProjectNameFromConstructor_CheckResult_ShouldEqualFinalPartOfCurrentNamespace()
        {
            Assert.Equal("CallStackAnalysis", this.projectNameFromConstructor);
        }

        /// <summary>
        /// Inferring the job name inside an XUnit test class constructor should return the expected value.
        /// </summary>
        [Fact]
        public void GetInferredJobNameFromConstructor_CheckResult_ShouldEqualCurrentTestClass()
        {
            Assert.Equal("XUnitStackTraceHelperTest", this.jobNameFromConstructor);
        }
    }
}

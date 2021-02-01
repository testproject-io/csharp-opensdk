// <copyright file="MSTestStackTraceHelperTest.cs" company="TestProject">
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
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TestProject.OpenSDK.Internal.CallStackAnalysis;

    /// <summary>
    /// Class containing unit tests for the <see cref="StackTraceHelper"/> class using MSTest.
    /// </summary>
    [TestClass]
    public class MSTestStackTraceHelperTest
    {
        private string projectNameFromTestInitialize;
        private string jobNameFromTestInitialize;

        /// <summary>
        /// Infer the project and job name from within a [TestInitialize] method for later verification.
        /// </summary>
        [TestInitialize]
        public void GetInferredProjectAndJobName()
        {
            this.projectNameFromTestInitialize = StackTraceHelper.Instance.GetInferredProjectName();
            this.jobNameFromTestInitialize = StackTraceHelper.Instance.GetInferredJobName();
        }

        /// <summary>
        /// Inferring the current test name should return the expected value.
        /// </summary>
        [TestMethod]
        public void GetInferredTestName_CheckResult_ShouldEqualCurrentTestMethodName()
        {
            string testName = StackTraceHelper.Instance.GetInferredTestName();

            Assert.AreEqual("GetInferredTestName_CheckResult_ShouldEqualCurrentTestMethodName", testName);
        }

        /// <summary>
        /// Inferring the current test name should return the expected value when using a description in the annotation.
        /// </summary>
        [TestMethod("Custom test name")]
        public void GetInferredTestName__UsingCustomTestName_CheckResult_ShouldEqualCurrentTestMethodName()
        {
            string testName = StackTraceHelper.Instance.GetInferredTestName();

            Assert.AreEqual("Custom test name", testName);
        }

        /// <summary>
        /// Inferring the job name should return the expected value.
        /// </summary>
        [TestMethod]
        public void GetInferredJobName_CheckResult_ShouldEqualCurrentTestClass()
        {
            string jobName = StackTraceHelper.Instance.GetInferredJobName();

            Assert.AreEqual("MSTestStackTraceHelperTest", jobName);
        }

        /// <summary>
        /// Inferring the project name should return the expected value.
        /// </summary>
        [TestMethod]
        public void GetInferredProjectName_CheckResult_ShouldEqualFinalPartOfCurrentNamespace()
        {
            string projectName = StackTraceHelper.Instance.GetInferredProjectName();

            Assert.AreEqual("CallStackAnalysis", projectName);
        }

        /// <summary>
        /// Inferring the project name in a [TestInitialize] method should return the expected value.
        /// </summary>
        [TestMethod]
        public void CheckInferredProjectNameFromTestInitialize_ShouldEqualFinalPartOfCurrentNamespace()
        {
            Assert.AreEqual("CallStackAnalysis", this.projectNameFromTestInitialize);
        }

        /// <summary>
        /// Inferring the job name in a [TestInitialize] method should return the expected value.
        /// </summary>
        [TestMethod]
        public void CheckInferredJobNameFromTestInitialize_ShouldEqualCurrentTestClass()
        {
            Assert.AreEqual("MSTestStackTraceHelperTest", this.jobNameFromTestInitialize);
        }
    }
}

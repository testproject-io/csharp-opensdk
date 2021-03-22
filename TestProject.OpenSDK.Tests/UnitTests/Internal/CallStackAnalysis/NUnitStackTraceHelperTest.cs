// <copyright file="NUnitStackTraceHelperTest.cs" company="TestProject">
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
    using NUnit.Framework;
    using TestProject.OpenSDK.Internal.CallStackAnalysis;

    /// <summary>
    /// Class containing unit tests for the <see cref="StackTraceHelper"/> class using NUnit.
    /// </summary>
    [TestFixture]
    public class NUnitStackTraceHelperTest
    {
        private readonly string projectNameFromConstructor;
        private readonly string jobNameFromConstructor;

        private string projectNameFromSetUp;
        private string jobNameFromSetUp;

        /// <summary>
        /// Initializes a new instance of the <see cref="NUnitStackTraceHelperTest"/> class.
        /// </summary>
        public NUnitStackTraceHelperTest()
        {
            this.projectNameFromConstructor = StackTraceHelper.Instance.GetInferredProjectName();
            this.jobNameFromConstructor = StackTraceHelper.Instance.GetInferredJobName();
        }

        /// <summary>
        /// Infer the project and job name from within a [SetUp] method for later verification.
        /// </summary>
        [SetUp]
        public void GetInferredProjectAndJobName()
        {
            this.projectNameFromSetUp = StackTraceHelper.Instance.GetInferredProjectName();
            this.jobNameFromSetUp = StackTraceHelper.Instance.GetInferredJobName();
        }

        /// <summary>
        /// Inferring the current test name should return the expected value.
        /// </summary>
        [Test]
        public void GetInferredTestName_CheckResult_ShouldEqualCurrentTestMethodName()
        {
            string testName = StackTraceHelper.Instance.GetInferredTestName();

            Assert.AreEqual("GetInferredTestName_CheckResult_ShouldEqualCurrentTestMethodName", testName);
        }

        /// <summary>
        /// Inferring the current test name should return the expected value when using a description in the annotation.
        /// </summary>
        [Test(Description ="Custom test name")]
        public void GetInferredTestName__UsingCustomTestName_CheckResult_ShouldEqualCurrentTestMethodName()
        {
            string testName = StackTraceHelper.Instance.GetInferredTestName();

            Assert.AreEqual("Custom test name", testName);
        }

        /// <summary>
        /// Inferring the job name should return the expected value.
        /// </summary>
        [Test]
        public void GetInferredJobName_CheckResult_ShouldEqualCurrentTestClass()
        {
            string jobName = StackTraceHelper.Instance.GetInferredJobName();

            Assert.AreEqual("NUnitStackTraceHelperTest", jobName);
        }

        /// <summary>
        /// Inferring the project name should return the expected value.
        /// </summary>
        [Test]
        public void GetInferredProjectName_CheckResult_ShouldEqualFinalPartOfCurrentNamespace()
        {
            string projectName = StackTraceHelper.Instance.GetInferredProjectName();

            Assert.AreEqual("CallStackAnalysis", projectName);
        }

        /// <summary>
        /// Inferring the project name in a [SetUp] method should return the expected value.
        /// </summary>
        [Test]
        public void CheckInferredProjectNameFromTestInitialize_ShouldEqualFinalPartOfCurrentNamespace()
        {
            Assert.AreEqual("CallStackAnalysis", this.projectNameFromSetUp);
        }

        /// <summary>
        /// Inferring the job name in a [SetUp] method should return the expected value.
        /// </summary>
        [Test]
        public void CheckInferredJobNameFromTestInitialize_ShouldEqualCurrentTestClass()
        {
            Assert.AreEqual("NUnitStackTraceHelperTest", this.jobNameFromSetUp);
        }

        /// <summary>
        /// Inferring the project name inside an NUnit test class constructor should return the expected value.
        /// </summary>
        [Test]
        public void GetInferredProjectNameFromConstructor_CheckResult_ShouldEqualFinalPartOfCurrentNamespace()
        {
            Assert.AreEqual("CallStackAnalysis", this.projectNameFromConstructor);
        }

        /// <summary>
        /// Inferring the job name inside an NUnit test class constructor should return the expected value.
        /// </summary>
        [Test]
        public void GetInferredJobNameFromConstructor_CheckResult_ShouldEqualCurrentTestClass()
        {
            Assert.AreEqual("NUnitStackTraceHelperTest", this.jobNameFromConstructor);
        }
    }
}

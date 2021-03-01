// <copyright file="NUnitStackTraceHelperWithClassDescriptionTest.cs" company="TestProject">
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
    [Description("This is a class-level description")]
    public class NUnitStackTraceHelperWithClassDescriptionTest
    {
        /// <summary>
        /// Inferring the job name for a class with a [Description] attribute should yield the description value.
        /// </summary>
        [Test]
        public void GetInferredJobName_CheckResult_ShouldEqualCurrentTestClass()
        {
            string jobName = StackTraceHelper.Instance.GetInferredJobName();

            Assert.AreEqual("This is a class-level description", jobName);
        }
    }
}

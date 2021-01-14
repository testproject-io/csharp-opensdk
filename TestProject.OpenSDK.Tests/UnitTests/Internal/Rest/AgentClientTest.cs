// <copyright file="AgentClientTest.cs" company="TestProject">
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

namespace TestProject.OpenSDK.Tests.UnitTests.Internal.Rest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TestProject.OpenSDK.Internal.Rest;

    /// <summary>
    /// Class containing unit tests for the <see cref="AgentClient"/> class.
    /// </summary>
    [TestClass]
    public class AgentClientTest
    {
        /// <summary>
        /// Helper method should return false when no Agent client has not been created yet.
        /// </summary>
        [TestMethod]
        public void IsInitialized_NoAgentClient_ShouldReturnFalse()
        {
            Assert.IsFalse(AgentClient.IsInitialized());
        }
    }
}

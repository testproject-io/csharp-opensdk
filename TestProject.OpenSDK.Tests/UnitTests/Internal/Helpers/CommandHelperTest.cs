// <copyright file="CommandHelperTest.cs" company="TestProject">
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

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestProject.OpenSDK.Internal.Helpers;

namespace TestProject.OpenSDK.Tests.UnitTests.Internal.Helpers
{
    /// <summary>
    /// Class containing unit tests for the <see cref="CommandHelper"/> class.
    /// </summary>
    [TestClass]
    public class CommandHelperTest
    {
        /// <summary>
        /// Parameters should not be updated when there is no 'value' key in the parameter dictionary.
        /// </summary>
        [TestMethod]
        public void UpdateSendKeysParameters_NoValue_ShouldReturnUnchangedParameters()
        {
            Dictionary<string, object> originalParams = new Dictionary<string, object>();
            originalParams.Add("abc", "def");

            Dictionary<string, object> modifiedParams = CommandHelper.UpdateSendKeysParameters(originalParams);

            Assert.AreEqual(originalParams, modifiedParams);
        }

        /// <summary>
        /// Parameters should not be updated when there is a 'value' key but its value is not an array.
        /// </summary>
        [TestMethod]
        public void UpdateSendKeysParameters_WithValueButNotAnArray_ShouldReturnUnchangedParameters()
        {
            Dictionary<string, object> originalParams = new Dictionary<string, object>();
            originalParams.Add("value", "abc");

            Dictionary<string, object> modifiedParams = CommandHelper.UpdateSendKeysParameters(originalParams);

            Assert.AreEqual(originalParams, modifiedParams);
        }

        /// <summary>
        /// Parameters should be updated when there is a 'value' key and its value is an array.
        /// </summary>
        [TestMethod]
        public void UpdateSendKeysParameters_WithValueThatIsAnArray_ShouldReturnPatchedParameters()
        {
            Dictionary<string, object> originalParams = new Dictionary<string, object>();
            originalParams.Add("value", new string[] { "astring" });

            Dictionary<string, object> modifiedParams = CommandHelper.UpdateSendKeysParameters(originalParams);

            Assert.IsTrue(modifiedParams.ContainsKey("text"));
            Assert.AreEqual("astring", modifiedParams["text"]);
            Assert.IsTrue(modifiedParams.ContainsKey("value"));
        }
    }
}

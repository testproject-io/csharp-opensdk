// <copyright file="LocatorHelperTest.cs" company="TestProject">
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

namespace TestProject.OpenSDK.Tests.UnitTests.Internal.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OpenQA.Selenium;
    using TestProject.OpenSDK.Internal.Helpers;

    /// <summary>
    /// Class containing unit tests for the <see cref="LocatorHelper"/> class.
    /// </summary>
    [TestClass]
    public class LocatorHelperTest
    {
        /// <summary>
        /// Test.
        /// </summary>
        /// <param name="by">A Selenium <see cref="By"/> locator.</param>
        /// <param name="locatorKey">A key representing a locator strategy.</param>
        /// <param name="expectedLocatorValue">The expected locator value corresponding with the specified key.</param>
        [DataTestMethod]
        [LocatorDataSource]
        public void ConvertToSerializable_ForAllSupportedByTypes_ShouldReturnExpectedDictionary(By by, string locatorKey, string expectedLocatorValue)
        {
            Dictionary<string, string> result = by.ConvertToSerializable();
            string actualLocatorValue;

            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.TryGetValue(locatorKey, out actualLocatorValue));
            Assert.AreEqual(expectedLocatorValue, actualLocatorValue);
        }

        /// <summary>
        /// Private class containing a custom data provider for the data driven test above.
        /// </summary>
        private class LocatorDataSourceAttribute : Attribute, ITestDataSource
        {
            public IEnumerable<object[]> GetData(MethodInfo methodInfo)
            {
                yield return new object[] { By.Id("idValue"), "id", "idValue" };
                yield return new object[] { By.Name("nameValue"), "name", "nameValue" };
                yield return new object[] { By.XPath("//my/xpath/value"), "xpath", "//my/xpath/value" };
                yield return new object[] { By.CssSelector("#myCss"), "cssSelector", "#myCss" };
                yield return new object[] { By.ClassName("classNameValue"), "className", "classNameValue" };
                yield return new object[] { By.LinkText("a_link"), "linkText", "a_link" };
                yield return new object[] { By.PartialLinkText("a_partial_link"), "partialLinkText", "a_partial_link" };
                yield return new object[] { By.TagName("myTag"), "tagName", "myTag" };
            }

            public string GetDisplayName(MethodInfo methodInfo, object[] data)
            {
                if (data != null)
                {
                    return string.Format(CultureInfo.CurrentCulture, "Custom - {0} ({1})", methodInfo.Name, string.Join(",", data));
                }

                return null;
            }
        }
    }
}

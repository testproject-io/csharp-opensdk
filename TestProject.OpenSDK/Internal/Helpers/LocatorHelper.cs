// <copyright file="LocatorHelper.cs" company="TestProject">
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

namespace TestProject.OpenSDK.Internal.Helpers
{
    using System.Collections.Generic;
    using OpenQA.Selenium;
    using TestProject.OpenSDK.Internal.Exceptions;

    /// <summary>
    /// Class containing extension methods for Selenium <see cref="By"/> locators.
    /// </summary>
    public static class LocatorHelper
    {
        /// <summary>
        /// Converts a <see cref="By"/> locator to a dictionary that the Agent understands.
        /// </summary>
        /// <param name="by">The <see cref="By"/> locator to convert.</param>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> containing the converted locator data.</returns>
        public static Dictionary<string, string> ConvertToSerializable(this By by)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            string[] byAsString = by.ToString().Split(':');

            string detectedLocator = string.Empty;

            switch (byAsString[0])
            {
                case "By.Id":
                    detectedLocator = "id";
                    break;
                case "By.Name":
                    detectedLocator = "name";
                    break;
                case "By.XPath":
                    detectedLocator = "xpath";
                    break;
                case "By.CssSelector":
                    detectedLocator = "cssSelector";
                    break;
                case "By.ClassName[Contains]":
                    detectedLocator = "className";
                    break;
                case "By.LinkText":
                    detectedLocator = "linkText";
                    break;
                case "By.PartialLinkText":
                    detectedLocator = "partialLinkText";
                    break;
                case "By.TagName":
                    detectedLocator = "tagName";
                    break;
                default:
                    throw new SdkException($"Unsupported locator type {byAsString[0]}");
            }

            result.Add(detectedLocator, byAsString[1].TrimStart());

            return result;
        }
    }
}

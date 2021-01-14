// <copyright file="CommandHelper.cs" company="TestProject">
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
    using System;
    using System.Collections.Generic;
    using NLog;

    /// <summary>
    /// This class contains helper methods to convert WebDriver commands to formats that the Agent understands.
    /// </summary>
    public class CommandHelper
    {
        private const string KEY_VALUE = "value";
        private const string KEY_TEXT = "text";

        private static Logger Logger { get; set; } = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Tries to update SendKeys parameters to a format that the Agent understands.
        /// </summary>
        /// <param name="parameters">The parameters to update.</param>
        /// <returns>The updated parameters.</returns>
        public static Dictionary<string, object> UpdateSendKeysParameters(Dictionary<string, object> parameters)
        {
            if (parameters.ContainsKey(KEY_VALUE))
            {
                if (parameters[KEY_VALUE] is Array)
                {
                    // Typically, the original value of the 'value' parameter is an array of objects
                    // We need to convert all of its elements (there should be only one) to strings
                    object[] value = (object[])parameters[KEY_VALUE];
                    string[] result = Array.ConvertAll(value, ConvertObjectToString);

                    // After that, we take the first value (should also be the only one) and assign
                    // that to the 'text' parameter
                    parameters.Add(KEY_TEXT, result[0]);
                }
                else
                {
                    Logger.Warn($"Original value of parameter '${KEY_VALUE}' was not an array and will not be patched.");
                }
            }
            else
            {
                Logger.Warn($"Parameter '${KEY_VALUE}' could not be found in the parameter dictionary and will not be patched.");
            }

            return parameters;
        }

        /// <summary>
        /// Converts an object to its string representation, or an empty string if that isn't possible.
        /// </summary>
        /// <param name="obj">The object to stringify.</param>
        /// <returns>A string representation of the object.</returns>
        private static string ConvertObjectToString(object obj)
        {
            return obj?.ToString() ?? string.Empty;
        }
    }
}

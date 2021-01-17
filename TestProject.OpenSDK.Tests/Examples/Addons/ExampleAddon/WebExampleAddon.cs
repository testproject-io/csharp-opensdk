// <copyright file="WebExampleAddon.cs" company="TestProject">
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

namespace TestProject.OpenSDK.Tests.Examples.Addons.ExampleAddon
{
    /// <summary>
    /// An example web addon class.
    /// </summary>
    public class WebExampleAddon
    {
        /// <summary>
        /// Addon method to generate a random phone number and type it into a specified field.
        /// </summary>
        /// <param name="countryCode">The country code to use when generating the random phone number.</param>
        /// <param name="maxDigits">The maximum number of digits in the phone number to generate.</param>
        /// <returns>The resulting action.</returns>
        public static TypeRandomPhoneAction TypeRandomPhoneAction(string countryCode, int maxDigits)
        {
            return new TypeRandomPhoneAction(countryCode, maxDigits);
        }

        /// <summary>
        /// Addon method to clear specified fields.
        /// </summary>
        /// <returns>The resulting action.</returns>
        public static ClearFieldsAction ClearFieldsAction()
        {
            return new ClearFieldsAction();
        }
    }
}

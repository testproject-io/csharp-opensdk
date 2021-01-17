// <copyright file="TypeRandomPhoneAction.cs" company="TestProject">
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
    using TestProject.OpenSDK.Internal.Addons;

    /// <summary>
    /// A custom action to generate and type a random phone number into a specified field.
    /// </summary>
    public class TypeRandomPhoneAction : ActionProxy
    {
        /// <summary>
        /// The country code to use when generating the random phone number.
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// The maximum number of digits in the phone number to generate.
        /// </summary>
        public int MaxDigits { get; set; }

        /// <summary>
        /// The generated phone number.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeRandomPhoneAction"/> class.
        /// </summary>
        /// <param name="countryCode">The country code to use when generating the random phone number.</param>
        /// <param name="maxDigits">The maximum number of digits in the phone number to generate.</param>
        public TypeRandomPhoneAction(string countryCode, int maxDigits)
            : base()
        {
            this.ProxyDescriptor = new ProxyDescriptor(
                guid: "GrQN1LQqTEmuYTnIujiEwA",
                className: "io.testproject.examples.sdk.actions.TypeRandomPhoneAction");
            this.CountryCode = countryCode;
            this.MaxDigits = maxDigits;
            this.Phone = string.Empty;
        }
    }
}

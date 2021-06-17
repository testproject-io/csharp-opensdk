// <copyright file="TestProjectCapabilityType.cs" company="TestProject">
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

namespace TestProject.OpenSDK.Drivers
{
    /// <summary>
    /// Custom capabilities provided by TestProject.
    /// </summary>
    public sealed class TestProjectCapabilityType
    {

        /// <summary>
        /// Custom capability to specify cloud provider URL for the driver initialization.
        /// </summary>
        public const string CLOUD_URL = "cloud:URL";

        /// <summary>
        /// Initializes a new instance of the <see cref="TestProjectCapabilityType"/> class.
        /// Private constructor to prevent creating instances of sealed class.
        /// </summary>
        private TestProjectCapabilityType()
        {
        }
    }
}

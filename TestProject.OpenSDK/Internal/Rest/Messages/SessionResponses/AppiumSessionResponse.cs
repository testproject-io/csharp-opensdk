// <copyright file="AppiumSessionResponse.cs" company="TestProject">
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

namespace TestProject.OpenSDK.Internal.Rest.Messages.SessionResponses
{
    using System.Collections.Generic;

    /// <summary>
    /// Payload object returned by the Agent when starting a mobile development session.
    /// </summary>
    public class AppiumSessionResponse : SessionResponse
    {
        /// <summary>
        /// Capabilities of the session that has been initialized by the Agent.
        /// </summary>
        public Dictionary<string, object> Capabilities { get; set; }
    }
}

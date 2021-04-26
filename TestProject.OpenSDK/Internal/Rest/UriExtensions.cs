// <copyright file="UriExtensions.cs" company="TestProject">
// Copyright 2021 TestProject (https://testproject.io)
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

namespace TestProject.OpenSDK.Internal.Rest
{
    using System;
    using System.Linq;

    /// <summary>
    /// Extension methods for SessionResponse.
    /// </summary>
    internal static class UriExtensions
    {
        private static readonly string[] LocalAddresses = { "localhost", "127.0.0.1", "0.0.0.0" };

        /// <summary>
        /// Checks if session is open on a local agent. Null URI are considered a local based on their use in AgentClient.
        /// </summary>
        /// <param name="uri">This session response</param>
        /// <returns>True if local session.</returns>
        internal static bool IsLocal(this Uri uri) =>
            uri == null || LocalAddresses.Any(a => uri.Host.Contains(a));
    }
}

// <copyright file="DriverCommandsFilter.cs" company="TestProject">
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

namespace TestProject.OpenSDK.Enums
{
    /// <summary>
    /// Enumeration of options for driver command reporting.
    /// </summary>
    public enum DriverCommandsFilter
    {
        /// <summary>
        /// Always report driver commands.
        /// </summary>
        None,

        /// <summary>
        /// Only report failing driver commands.
        /// </summary>
        Passing,

        /// <summary>
        /// Never report driver commands.
        /// </summary>
        All,
    }
}

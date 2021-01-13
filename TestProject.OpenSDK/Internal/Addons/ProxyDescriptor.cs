// <copyright file="ProxyDescriptor.cs" company="TestProject">
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

namespace TestProject.OpenSDK.Internal.Addons
{
    using System.Collections.Generic;

    /// <summary>
    /// Describes an Addon and an Action to be executed via the Agent.
    /// </summary>
    public class ProxyDescriptor
    {
        /// <summary>
        /// A unique GUID for the addon.
        /// </summary>
        public string Guid { get; }

        /// <summary>
        /// The name of the action class that is contained in the addon.
        /// </summary>
        public string ClassName { get; }

        /// <summary>
        /// Dictionary representing the target element locator (optional).
        /// </summary>
        public Dictionary<string, string> By { get; set; }

        /// <summary>
        /// The command parameters.
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyDescriptor"/> class.
        /// </summary>
        /// <param name="guid">The addon GUID.</param>
        /// <param name="className">The action class name.</param>
        public ProxyDescriptor(string guid, string className)
        {
            this.Guid = guid;
            this.ClassName = className;
        }
    }
}

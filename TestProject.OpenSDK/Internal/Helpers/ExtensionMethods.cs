// <copyright file="ExtensionMethods.cs" company="TestProject">
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
    using System.Reflection;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Remote;
    using TestProject.OpenSDK.Internal.Rest;

    /// <summary>
    /// This class contains custom extension methods implementing SDK specific business logic.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Determines whether or not a driver command should be patched before sending it to the Agent.
        /// </summary>
        /// <param name="driverCommand">The driver command that should or should not be patched.</param>
        /// <returns>True if the driver command should be patched, false otherwise.</returns>
        public static bool ShouldBePatched(this string driverCommand)
        {
            return driverCommand.Equals(DriverCommand.SendKeysToElement) || driverCommand.Equals(DriverCommand.SendKeysToActiveElement);
        }

        /// <summary>
        /// Determines whether or not a WebDriver command execution is deemed successful.
        /// </summary>
        /// <param name="response">The <see cref="Response"/> to be inspected.</param>
        /// <returns>True if the Response result is interpreted as successful, false otherwise.</returns>
        public static bool IsPassed(this Response response)
        {
            return response.Status.Equals(WebDriverResult.Success);
        }

        /// <summary>
        /// Changes the hostname of a <see cref="Uri"/> to '127.0.0.1' if the hostname is 'localhost'.
        /// </summary>
        /// <param name="originalUri">The original <see cref="Uri"/>.</param>
        /// <returns>An updated Uri where 'localhost' is replaced with '127.0.0.1'.</returns>
        public static Uri LocalhostTo127001(this Uri originalUri)
        {
            if (originalUri.Host.ToLower().Equals("localhost"))
            {
                UriBuilder builder = new UriBuilder(originalUri);
                builder.Host = "127.0.0.1";
                return builder.Uri;
            }
            else
            {
                return originalUri;
            }
        }

        /// <summary>
        /// Checks whether the session associated with the given AgentClient is a W3C compatible session.
        /// </summary>
        /// <param name="agentClient">The <see cref="AgentClient"/> that we want to check W3C compatibility for.</param>
        /// <returns>True if the session is W3C compatible, false otherwise.</returns>
        public static bool IsInW3CMode(this AgentClient agentClient)
        {
            return agentClient.AgentSession.Dialect.Equals("W3C");
        }

        /// <summary>
        /// Retrieves field info for a field in a given type or any of its supertypes, using reflection.
        /// </summary>
        /// <param name="type">The type for which a field is to be found.</param>
        /// <param name="name">The name of the field that should be found.</param>
        /// <param name="flags">The binding flags to be applied when looking for the specified field.</param>
        /// <returns>A <see cref="FieldInfo"/> object representing the field, or null if none was found.</returns>
        public static FieldInfo GetInheritedField(this Type type, string name, BindingFlags flags)
        {
            FieldInfo fieldInfo = null;
            while (type != null)
            {
                fieldInfo = type.GetField(name, flags);
                if (fieldInfo != null)
                {
                    break;
                }

                type = type.BaseType;
            }

            return fieldInfo;
        }

        /// <summary>
        /// Call private method.
        /// </summary>
        /// <param name="type">Object.</param>
        /// <param name="methodName">Method name.</param>
        /// <param name="args">Arguments.</param>
        /// <returns>Return value.</returns>
        public static object CallPrivateStaticMethod(this Type type, string methodName, params object[] args)
        {
            MethodInfo methodInfo = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);

            return methodInfo == null ? null : methodInfo.Invoke(null, args);
        }
    }
}

// <copyright file="AddonHelper.cs" company="TestProject">
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
    using System.Reflection;
    using Newtonsoft.Json;
    using NLog;
    using OpenQA.Selenium;
    using TestProject.OpenSDK.Exceptions;
    using TestProject.OpenSDK.Internal.Helpers;
    using TestProject.OpenSDK.Internal.Rest;
    using TestProject.OpenSDK.Internal.Rest.Messages;

    /// <summary>
    /// Helper class for the execution of TestProject addons.
    /// </summary>
    public class AddonHelper
    {
        /// <summary>
        /// Logger instance for this class.
        /// </summary>
        private static Logger Logger { get; set; } = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initializes a new instance of the <see cref="AddonHelper"/> class.
        /// </summary>
        public AddonHelper()
        {
        }

        /// <summary>
        /// Executes an action using its proxy.
        /// </summary>
        /// <param name="actionProxy">The action proxy to execute.</param>
        /// <param name="by">A locator for the <see cref="IWebElement"/> to execute the action on.</param>
        /// <param name="timeoutInMilliSeconds">Timeout for the action execution duration.</param>
        /// <returns>A potentially updated <see cref="ActionProxy"/> with updated output fields (if any).</returns>
        public ActionProxy Execute(ActionProxy actionProxy, By by = null, int timeoutInMilliSeconds = -1)
        {
            if (by != null)
            {
                // Because serializing does not work 'out of the box' for By objects,
                // we need to handle that with a custom method.
                actionProxy.ProxyDescriptor.By = by.ConvertToSerializable();
            }

            // The payload to be sent to the Agent needs to be formatted first.
            actionProxy.ProxyDescriptor.Parameters = this.FormatParameters(actionProxy);

            ActionExecutionResponse response = AgentClient.GetInstance().ExecuteProxy(actionProxy, timeoutInMilliSeconds);

            if (!response.ResultType.Equals(ActionExecutionResponse.ExecutionResultType.Passed))
            {
                throw new SdkException($"Error occurred during addon action execution: {response.Message}");
            }

            foreach (ActionExecutionResponse.ResultField field in response.Fields)
            {
                if (!field.Output)
                {
                    // Skip input fields
                    continue;
                }

                var proxyField = actionProxy.GetType().GetProperty(field.Name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                // Check if action has an attribute with the name of the field
                if (proxyField == null)
                {
                    Logger.Warn($"Action {actionProxy.ProxyDescriptor.Guid} does not have a field named '{field.Name}'");
                    continue;
                }

                proxyField.SetValue(actionProxy, field.Value);

                Logger.Trace($"Value of output field '{proxyField.Name}' has been set to '{proxyField.GetValue(actionProxy)}'");
            }

            return actionProxy;
        }

        /// <summary>
        /// Converts action parameters to a format understood by the Agent.
        /// </summary>
        /// <param name="actionProxy">The <see cref="ActionProxy"/> for which the parameters should be formatted.</param>
        /// <returns>A formatted set of action parameters.</returns>
        private Dictionary<string, object> FormatParameters(ActionProxy actionProxy)
        {
            string actionProxyAsJson = CustomJsonSerializer.ToJson(actionProxy, CustomJsonSerializer.Populate(new JsonSerializerSettings()));

            Dictionary<string, object> actionParameters = JsonConvert.DeserializeObject<Dictionary<string, object>>(actionProxyAsJson);

            actionParameters.Remove("proxyDescriptor");

            return actionParameters;
        }
    }
}

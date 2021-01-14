// <copyright file="ActionExecutionResponse.cs" company="TestProject">
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

namespace TestProject.OpenSDK.Internal.Rest.Messages
{
    using System.Collections.Generic;

    /// <summary>
    /// Agent response to an Action proxy execution request.
    /// </summary>
    public class ActionExecutionResponse
    {
        /// <summary>
        /// Action execution result (passed / failed / skipped).
        /// </summary>
        public ExecutionResultType ResultType { get; set; }

        /// <summary>
        /// Execution result message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Addon fields (input / output).
        /// </summary>
        public List<ResultField> Fields { get; set; }

        /// <summary>
        /// Action execution result types.
        /// </summary>
        public enum ExecutionResultType
        {
            /// <summary>
            /// Passed result.
            /// </summary>
            Passed,

            /// <summary>
            /// Failed result
            /// </summary>
            Failed,

            /// <summary>
            /// Skipped result.
            /// </summary>
            Skipped,
        }

        /// <summary>
        /// Action proxy execution result field.
        /// Returns as part of the Agent response with potentially updated output fields.
        /// </summary>
        public class ResultField
        {
            /// <summary>
            /// The name of the result field.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// The result field value.
            /// </summary>
            public object Value { get; set; }

            /// <summary>
            /// True if the field is an output field, false otherwise.
            /// </summary>
            public bool Output { get; set; }
        }
    }
}

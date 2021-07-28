// <copyright file="XUnitLoggerTarget.cs" company="TestProject">
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

namespace TestProject.OpenSDK.XUnitLogger
{
    using NLog;
    using NLog.Targets;
    using Xunit.Abstractions;

    /// <summary>
    /// Creates a custom NLog target that writes to Xunit's ITestOutputHelperr.
    /// </summary>
    internal class XunitLoggerTarget : TargetWithLayout
    {
        private readonly ITestOutputHelper outputHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="XunitLoggerTarget"/> class.
        /// </summary>
        /// <param name="outputHelper">XUnit's output helper which allows capturing output. </param>
        public XunitLoggerTarget(ITestOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
        }

        /// <summary>
        /// Writes the log message to XUnit's outputHelper,
        /// which then redirects it to the console.
        /// </summary>
        /// <param name="logEvent"> logEvent information which is used to get the logged message.</param>
        protected override void Write(LogEventInfo logEvent)
        {
            var logMessage = this.Layout.Render(logEvent);
            this.outputHelper.WriteLine(logMessage);
        }
    }
}

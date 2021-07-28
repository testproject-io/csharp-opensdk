// <copyright file="XUnitLogger.cs" company="TestProject">
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
    using NLog.Config;
    using Xunit.Abstractions;

    /// <summary>
    /// Class that is used to configure the logger with XUnit.
    /// </summary>
    public abstract class XUnitLogger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XUnitLogger"/> class.
        /// </summary>
        /// <param name="outputHelper"> Redirecting the logs to XUnit's outputHelper.</param>
        protected XUnitLogger(ITestOutputHelper outputHelper)
        {
            var config = new LoggingConfiguration();
            var target = new XunitLoggerTarget(outputHelper);

            config.AddTarget("xUnit", target);
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, target, "*");
            LogManager.Configuration = config;
        }
    }
}

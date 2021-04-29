// <copyright file="TestProjectDataProvider.cs" company="TestProject">
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


namespace TestProject.OpenSDK.DataProviders
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Provides shit.
    /// </summary>
    public class TestProjectDataProvider : IEnumerable<object[]>
    {
        private const string TpDataProviderFile = "TP_TEST_DATA_PROVIDER";

        /// <summary>
        /// Returns the data source values to be used by parameterized tests that are uploaded to TestProject.
        /// </summary>
        /// <returns>Data source enumerable</returns>
        public static IEnumerable<string[]> DataSource
        {
            get
            {
                var dataProviderFile = Environment.GetEnvironmentVariable(TpDataProviderFile);
                if (dataProviderFile == null || !File.Exists(dataProviderFile))
                {
                    throw new Exception(
                        "No data provider was specified. Make sure this annotation is used for uploaded tests only.");
                }

                return File.ReadAllLines(dataProviderFile).Skip(1).Select(l => l.Split(','));
            }
        }

        /// <inheritdoc cref="IEnumerable{T}"/>
        public IEnumerator<object[]> GetEnumerator()
        {
            return DataSource.GetEnumerator();
        }

        /// <inheritdoc cref="IEnumerable{T}"/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
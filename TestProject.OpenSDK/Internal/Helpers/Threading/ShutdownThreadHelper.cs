// <copyright file="ShutdownThreadHelper.cs" company="TestProject">
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

namespace TestProject.OpenSDK.Internal.Helpers.Threading
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;

    /// <summary>
    /// Helper for calling shutdown thread actions in-order.
    /// Actions are executed in increased priority
    /// </summary>
    internal class ShutdownThreadHelper
    {
        private static readonly Lazy<ShutdownThreadHelper> Lazy = new Lazy<ShutdownThreadHelper>(() => new ShutdownThreadHelper());

        private readonly ConcurrentDictionary<int, BaseThread> threads;

        /// <summary>
        /// Gets the instance of this class.
        /// </summary>
        public static ShutdownThreadHelper Instance => Lazy.Value;

        private ShutdownThreadHelper()
        {
            this.threads = new ConcurrentDictionary<int, BaseThread>();
            AppDomain.CurrentDomain.ProcessExit += this.OnProcessExit;
        }

        /// <summary>
        /// Register a BaseThread to be executed when process shuts down.
        /// </summary>
        /// <param name="priority">Priority of thread execution.</param>
        /// <param name="thread">Thread to register.</param>
        /// <exception cref="IndexOutOfRangeException">If registering a thread with an occupied priority.</exception>
        public void Register(int priority, BaseThread thread)
        {
            if (this.threads.ContainsKey(priority))
            {
                throw new IndexOutOfRangeException("Attempted to register a thread with a priority already in use.");
            }

            this.threads[priority] = thread;
        }

        /// <summary>
        /// Un-registers a thread with requested priority. If it isn't registered nothing happens.
        /// </summary>
        /// <param name="priority">Priority to unregister.</param>
        public void Unregister(int priority)
        {
            this.threads.TryRemove(priority, out _);
        }

        private void OnProcessExit(object sender, EventArgs e)
        {
            foreach (var thread in this.threads.OrderBy((kv) => kv.Key).Select(kv => kv.Value))
            {
                thread.RunThread();
            }
        }
    }
}

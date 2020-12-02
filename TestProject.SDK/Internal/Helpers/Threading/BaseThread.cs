// <copyright file="BaseThread.cs" company="TestProject">
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

using System.Threading;

namespace TestProject.SDK.Internal.Helpers.Threading
{
    /// <summary>
    /// A BaseThread object used for composition as <see cref="System.Threading.Thread"/> cannot be inherited from directly.
    /// </summary>
    public abstract class BaseThread
    {
        private Thread thread;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseThread"/> class.
        /// </summary>
        protected BaseThread()
        {
            this.thread = new Thread(new ThreadStart(this.RunThread));
        }

        /// <summary>
        /// Causes the thread to be scheduled for execution.
        /// </summary>
        public void Start() => this.thread.Start();

        /// <summary>
        /// Blocks the calling thread until the thread represented by this instance terminates.
        /// </summary>
        public void Join() => this.thread.Join();

        /// <summary>
        /// Gets a value indicating the execution status of the current thread.
        /// </summary>
        public bool IsAlive => this.thread.IsAlive;

        /// <summary>
        /// Contains the statements to be executed in this thread.
        /// </summary>
        public abstract void RunThread();
    }
}

// <copyright file="SocketManager.cs" company="TestProject">
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

namespace TestProject.OpenSDK.Internal.Tcp
{
    using System;
    using System.Net.Sockets;
    using NLog;
    using TestProject.OpenSDK.Internal.Exceptions;
    using TestProject.OpenSDK.Internal.Helpers.Threading;

    /// <summary>
    /// Manages the TCP socket connection for a development session.
    /// </summary>
    public class SocketManager
    {
        /// <summary>
        /// The SocketManager singleton instance.
        /// </summary>
        private static SocketManager instance;

        /// <summary>
        /// Thread object used to gracefully close the socket when the session ends.
        /// </summary>
        private static SocketClosingThread socketClosingThread;

        /// <summary>
        /// Logger instance for this class.
        /// </summary>
        private static Logger Logger { get; set; } = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Holds an instance of a TCP socket connection between the SDK and the Agent.
        /// </summary>
        private Socket socket;

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketManager"/> class.
        /// </summary>
        private SocketManager()
        {
            socketClosingThread = new SocketClosingThread();
        }

        /// <summary>
        /// Static method to obtain a singleton instance of the <see cref="SocketManager"/> class.
        /// </summary>
        /// <returns>Singleton <see cref="SocketManager"/> instance.</returns>
        public static SocketManager GetInstance()
        {
            if (instance == null)
            {
                instance = new SocketManager();
            }

            return instance;
        }

        /// <summary>
        /// Opens a TCP socket connection to the Agent using provided host and port (if one does not exist yet).
        /// </summary>
        /// <param name="host">The host name to connect to.</param>
        /// <param name="port">The development socket port to connect to.</param>
        public void OpenSocket(string host, int port)
        {
            if (this.socket != null && this.socket.Connected)
            {
                Logger.Debug("Socket is already connected.");
                return;
            }

            try
            {
                this.socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                this.socket.Connect(host, port);
                Logger.Info($"Successfully connected to TCP socket at {host}:{port}");
            }
            catch (SocketException se)
            {
                Logger.Error($"An error occurred when connecting to {host}:{port} - {se.Message}");
                throw new AgentConnectException("Failed connecting to Agent socket");
            }
        }

        /// <summary>
        /// Closes the TCP socket connection to the Agent.
        /// </summary>
        public void CloseSocket()
        {
            if (this.IsOpen())
            {
                try
                {
                    this.socket.Shutdown(SocketShutdown.Both);
                }
                finally
                {
                    this.socket.Close();
                }

                this.socket = null;
                Logger.Debug("Development socket successfully closed");
            }
            else
            {
                Logger.Debug("Development socket was already closed.");
            }
        }

        /// <summary>
        /// Checks if the socket is connected.
        /// </summary>
        /// <returns>True if the socket is connected, false otherwise.</returns>
        private bool IsOpen()
        {
            return this.socket != null && this.socket.Connected;
        }
    }
}

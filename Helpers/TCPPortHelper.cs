//----------------------------------------------------------------------------------------------
// <copyright file="TCPPortHelper.cs" company="SonarSource SA and Microsoft Corporation">
// Copyright (c) SonarSource SA and Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
//----------------------------------------------------------------------------------------------

using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using System;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace Wix.SonarQube
{
    /// <summary>
    /// [TODO] Lookup for the next available port and return that to the caller.
    /// </summary>
    public class TCPPortHelper
    {
        /// <summary>
        /// Checks if the given port is free to use or not.
        /// </summary>
        public static bool IsPortAvailable(int portNumber)
        {
            bool result = true;

            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] tcpConnectionInformationArray = ipGlobalProperties.GetActiveTcpConnections();

            foreach (TcpConnectionInformation tcpi in tcpConnectionInformationArray)
            {
                if (tcpi.LocalEndPoint.Port == portNumber)
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Creates rule in firewall settings to enable the port.
        /// </summary>
        public static bool EnablePortForFirewall(string ruleName, int portNumber)
        {
            Process process = new Process();

            try
            {
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = "netsh.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.Arguments = string.Format("advfirewall firewall add rule name=\"{0}\" dir=in action=allow protocol=TCP localport={1}", ruleName, portNumber);

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                BootstrapperManager.Instance.Bootstrapper.Engine.Log(LogLevel.Error, string.Format("[EnablePortForFirewall] {0}", e.Message));
            }

            return true;
        }
    }
}

//----------------------------------------------------------------------------------------------
// <copyright file="SonarProcessHelper.cs" company="SonarSource SA and Microsoft Corporation">
// Copyright (c) SonarSource SA and Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
//----------------------------------------------------------------------------------------------

using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using System;
using System.Diagnostics;
using System.Linq;
using System.Management;

namespace SonarQubeBootstrapper.Helpers
{
    /// <summary>
    /// Helper class to detect if a SonarQube service/process is running on the target machine
    /// </summary>
    public class SonarProcessHelper
    {
        /// <summary>
        /// Checks either SonarQube service or batch file running on the machine.
        /// </summary>
        /// <returns></returns>
        public static bool IsSonarQubeRunning()
        {
            return DoesSonarQubeServiceExist() || IsSonarQubeBatchRunning();
        }

        /// <summary>
        /// Checks if a service that goes by the name SonarQube exists on the target machine.
        /// </summary>
        public static bool DoesSonarQubeServiceExist()
        {
            bool result = false;

            try
            {
                string queryString = "SELECT * FROM WIN32_SERVICE WHERE DISPLAYNAME='SonarQube'";

                ObjectQuery oQuery = new ObjectQuery(queryString);
                ManagementObjectSearcher objectSearcher = new ManagementObjectSearcher(oQuery);
                ManagementObjectCollection objectCollection = objectSearcher.Get();

                if (objectCollection.Count != 0)
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                BootstrapperManager.Instance.Bootstrapper.Engine.Log(LogLevel.Error, string.Format("[DoesSonarQubeServiceExist] {0}", e.Message));
            }

            return result;
        }
        
        /// <summary>
        /// Checks if SonarQube batch file is running on the machine.
        /// </summary>
        /// <returns></returns>
        public static bool IsSonarQubeBatchRunning()
        {
            bool result = false;

            try
            {
                var processes = Process.GetProcesses();

                var cmdProcesses = processes.Where(x => (x.ProcessName.Equals("cmd", StringComparison.InvariantCultureIgnoreCase)
                                                          && x.MainWindowTitle != null
                                                          && x.MainWindowTitle.Equals("sonarqube", StringComparison.InvariantCultureIgnoreCase))).ToList();

                if (cmdProcesses.Count != 0)
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                BootstrapperManager.Instance.Bootstrapper.Engine.Log(LogLevel.Error, string.Format("[IsSonarQubeBatchRunning] {0}", e.Message));
            }

            return result;
        }
    }
}

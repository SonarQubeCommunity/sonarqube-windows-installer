//----------------------------------------------------------------------------------------------
// <copyright file="JavaVersionHelper.cs" company="SonarSource SA and Microsoft Corporation">
// Copyright (c) SonarSource SA and Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
//----------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Wix.SonarQube
{
    public enum Architecture
    {
        x86 = 32,
        x64 = 64
    }

    public struct JavaVersionInformation
    {
        public string Version;

        public Architecture Architecture;
    }

    public class JavaVersionHelper
    {
        /// <summary>
        /// Returns the installed java version and the architecture
        /// </summary>
        public static JavaVersionInformation GetJavaVersionInformation()
        {
            JavaVersionInformation information = new JavaVersionInformation();

            Process process = new Process();
            List<string> output = new List<string>();

            // Always fetch the latest value of Path variable to update the cached value.
            Environment.SetEnvironmentVariable("Path",
                                               Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine),
                                               EnvironmentVariableTarget.Process);

            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.Arguments = "/c \"" + "java -version " + "\"";

            process.OutputDataReceived += new DataReceivedEventHandler((s, e) =>
            {
                if (e.Data != null)
                {
                    output.Add((string)e.Data);
                }
            });
            process.ErrorDataReceived += new DataReceivedEventHandler((s, e) =>
            {
                if (e.Data != null)
                {
                    output.Add((String)e.Data);
                }
            });

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();

            string versionLine = output.Where(x => x.StartsWith("java version", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            if (process.ExitCode == 0)
            {
                information.Version = versionLine.Substring(14, 3);
                information.Architecture = output.Any(x => x.Contains("64-Bit")) ? Architecture.x64 : Architecture.x86;
            }

            return information;
        }

        /// <summary>
        /// Compares if the installed version of java matches with the operating system's
        /// </summary>
        public static bool IsExpectedJavaVersionInstalled(JavaVersionInformation information, string version)
        {
            bool result = false;
            
            if (((information.Architecture == Architecture.x64 &&
                System.Environment.Is64BitOperatingSystem) ||
                (information.Architecture == Architecture.x86 &&
                !System.Environment.Is64BitOperatingSystem)) &&
                information.Version.CompareTo(version) >= 0)
            {
                result = true;
            }

            return result;
        }
    }
}

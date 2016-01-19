//----------------------------------------------------------------------------------------------
// <copyright file="SonarQubeWixConstants.cs" company="SonarSource SA and Microsoft Corporation">
// Copyright (c) SonarSource SA and Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
//----------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Wix.SonarQube
{
    /// <summary>
    /// Setup type for SonarQube service
    /// </summary>
    public static class SetupType
    {
        public const string Evaluation = "EVALUATION";
        public const string Express = "EXPRESS";
        public const string Custom = "CUSTOM";
    }
    
    public static class DbEngine
    {
        public const string H2 = "H2";
        public const string MsSql = "MSSQL";
        public const string MySql = "MYSQL";
        public const string Oracle = "ORACLE";
        public const string PostGre = "POSTGRE";
    }

    /// <summary>
    /// Architecture type used for installer
    /// </summary>
    public static class ArchitectureType
    {
        public const string X64 = "64";
        public const string X86 = "86";
    }

    /// <summary>
    /// Authentication type used for MSSQL server database
    /// </summary>
    public static class AuthenticationType
    {
        public const string Windows = "WINDOWS";
        public const string Sql = "SQL";
    }

    /// <summary>
    /// Map for MsiPackages and their formal names.
    /// </summary>
    public static class MsiPackages
    {
        public static Dictionary<string, string> MsiPackageMap = new Dictionary<string, string>()
                                                                 {
                                                                    { "SonarQubePackage", "SonarQube" },
                                                                    { "SqlExpressPackage", "Sql Express" }
                                                                 };
    }

    /// <summary>
    /// Bootstrapper constants list
    /// </summary>
    public static class BootstrapperConstants
    {
        public static string FormalProductName = "SonarQube Server 5.2";
        public static string DefaultInstallDir = @"%ProgramFiles%\SonarQube Server 5.2";

        public static string DefaultInstallServiceOption = "YES";
        public static string DefaultSonarQubePort = "9000";
        public static string DefaultSonarQubeDatabaseName = "sonarqubedb";
        public static string DefaultSqlInstance = "localhost";
        public static string DefaultSqlUserName = "sonarqube";
        public static string DefaultSqlUserPassword = "sonarqube";

        public static string SonarQubeProductName = "sonarqube-5.2";
        public static string SqlSummaryLogFilePath = (Environment.Is64BitOperatingSystem ? "%ProgramW6432%" : "%ProgramFiles%") +
                                                     @"\Microsoft SQL Server\110\Setup Bootstrap\Log\Summary.txt";
        public static int MSSqlPortNumber = 1433;
        public static int MySqlPortNumber = 3306;
        public static int OraclePortNumber = 1521;
        public static int PostGrePortNumber = 5432;

        public static int InstallCustomActionsCount = 6;
        public static int UninstallCustomActionsCount = 2;

        public static string ExpectedJavaVersion = "1.7";
        public static string ProductVersion = "1.0.0.0";

        public static int WebRequestTimeout = 5 * 60 * 1000;   // 5 minute
        public static string SqlConnectionInfo = "You can specify previously configured named instance in Microsoft SQL Server in the following form:\n    ServerName\\InstanceName or\n    Server,Port";
    }

    /// <summary>
    /// These enums are required to separate out our messages from
    /// other messages sent from MSIs.
    /// </summary>
    public enum MessageId
    {
        Progress = 0x000C0DE1,

        MessageBox = 0x000C0DE2
    }

    /// <summary>
    /// SonarQube setup state
    /// </summary>
    public enum SetupState
    {
        Success,

        FailedOnSqlSetup,

        FailedOnDowngrade,

        FailedOnOtherReason
    }
}

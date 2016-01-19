//----------------------------------------------------------------------------------------------
// <copyright file="SqlServerHelper.cs" company="SonarSource SA and Microsoft Corporation">
// Copyright (c) SonarSource SA and Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
//----------------------------------------------------------------------------------------------

using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using Microsoft.Win32;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace Wix.SonarQube
{
    public static class SqlServerHelper
    {
        /// <summary>
        /// Checks if sql is already installed on the machine
        /// </summary>
        public static bool IsSqlServerInstalled()
        {
            bool result = false;

            RegistryView registryView = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;
            using (RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView))
            {
                RegistryKey instanceKey = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL", false);
                if (instanceKey != null &&
                    instanceKey.GetValueNames().Length != 0)
                {
                    result = CheckSqlServer(instanceKey);
                }
                else
                {
                    // 32-bit instance on 64-bit machine.
                    instanceKey = hklm.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Microsoft SQL Server\Instance Names\SQL", false);

                    if (instanceKey != null &&
                        instanceKey.GetValueNames().Length != 0)
                    {
                        result = CheckSqlServer(instanceKey);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Create SqlConnection object for windows authentication
        /// </summary>
        public static SqlConnection CreateSqlConnectionWithWindowsLogin(string dataSource, string databaseName = null)
        {
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();
            SqlConnection sqlConnection = null;
            
            connectionStringBuilder.DataSource = dataSource;
            connectionStringBuilder.IntegratedSecurity = true;

            if (databaseName != null)
            {
                connectionStringBuilder.InitialCatalog = databaseName;
            }

            try
            {
                sqlConnection = new SqlConnection(connectionStringBuilder.ConnectionString);
            }
            catch (SqlException e)
            {
                BootstrapperManager.Instance.Bootstrapper.Engine.Log(LogLevel.Error, string.Format("[CreateSqlConnectionWithWindowsLogin] {0}", e.Message));
            }

            return sqlConnection;
        }

        /// <summary>
        /// Creates sql connection object with sql login
        /// </summary>
        public static SqlConnection CreateSqlConnectionWithSqlLogin(string dataSource, string username, string password, string databaseName = null)
        {
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();
            SqlConnection sqlConnection = null;

            connectionStringBuilder.DataSource = dataSource;
            connectionStringBuilder.UserID = username;
            connectionStringBuilder.Password = password;

            if (databaseName != null)
            {
                connectionStringBuilder.InitialCatalog = databaseName;
            }

            try
            {
                sqlConnection = new SqlConnection(connectionStringBuilder.ConnectionString);
            }
            catch (SqlException e)
            {
                BootstrapperManager.Instance.Bootstrapper.Engine.Log(LogLevel.Error, string.Format("[CreateSqlConnectionWithSqlLogin] {0}", e.Message));
            }

            return sqlConnection;
        }

        /// <summary>
        /// Tries to connect to sql database with windows login.
        /// </summary>
        public static bool TestSqlServerConnectionWithWindowsLogin(string dataSource)
        {
            bool result = false;

            try
            {
                using (SqlConnection connection = CreateSqlConnectionWithWindowsLogin(dataSource))
                {
                    connection.Open();

                    result = true;
                }
            }
            catch (SqlException e)
            {
                BootstrapperManager.Instance.Bootstrapper.Engine.Log(LogLevel.Error, string.Format("[TestSqlServerConnectionWithWindowsLogin] {0}", e.Message));
            }

            return result;
        }

        /// <summary>
        /// Tries to connect to sql database with user provided credentials
        /// </summary>
        public static bool TestSqlConnectionWithSqlServerLogin(string dataSource, string username, string password)
        {
            bool result = false;

            try
            {
                using (SqlConnection connection = CreateSqlConnectionWithSqlLogin(dataSource, username, password))
                {
                    connection.Open();

                    result = true;
                }
            }
            catch (SqlException e)
            {
                BootstrapperManager.Instance.Bootstrapper.Engine.Log(LogLevel.Error, string.Format("[TestSqlConnectionWithSqlServerLogin] {0}", e.Message));
            }

            return result;
        }

        private static bool CheckSqlServer(RegistryKey instanceKey)
        {
            return instanceKey.GetValueNames().Any();
        }
    }
}

//----------------------------------------------------------------------------------------------
// <copyright file="SonarConfigurationFileEditor.cs" company="SonarSource SA and Microsoft Corporation">
// Copyright (c) SonarSource SA and Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
//----------------------------------------------------------------------------------------------

using Microsoft.Deployment.WindowsInstaller;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WixSharp;

namespace Wix.SonarQube
{
    /// <summary>
    /// Helper class to modify the sonar.properties file.
    /// [TODO] See if we can come up with a better approach to update this file.
    /// </summary>
    public static class SonarConfigurationFileEditor
    {
        public static void UpdateSonarPropertiesFileForSql(Session session)
        {
            try
            {
                Dictionary<string, string> propertyValueMap = CreateSonarPropertyValueMapForSqlServer(session);

                string path = Path.Combine(session.Property("INSTALLDIR"), BootstrapperConstants.SonarQubeProductName, @"conf\sonar.properties");

                using (StreamWriter sw = System.IO.File.AppendText(path))
                {
                    sw.Write(sw.NewLine);

                    foreach (var property in propertyValueMap)
                    {
                        sw.WriteLine(string.Format(SonarPropertiesMap[property.Key], property.Value));
                    }
                }
            }
            catch (Exception e)
            {
                session.Log("[UpdateSonarPropertiesFileForSql] {0}", e.Message);
            }
        }

        private static Dictionary<string, string> CreateSonarPropertyValueMapForSqlServer(Session session)
        {
            Dictionary<string, string> resultMap = new Dictionary<string, string>();

            string authenticationType = session.Property("SQLAUTHTYPE");
            string dbEngine = session.Property("DBENGINE");

            // Only for MSSQL with Windows Authentication, we needn't set the user name and password.
            // For any other RDBMS and MSSQL with SQL authentication, we set both these properties.
            if (!(DbEngine.MsSql.Equals(dbEngine, StringComparison.InvariantCultureIgnoreCase)
                && AuthenticationType.Windows.Equals(authenticationType, StringComparison.InvariantCultureIgnoreCase)))
            {
                resultMap.Add("username", session.Property("DATABASEUSERNAME"));
                resultMap.Add("password", session.Property("DATABASEPASSWORD"));
            }

            if (DbEngine.MsSql.Equals(dbEngine, StringComparison.InvariantCultureIgnoreCase))
            {
                resultMap.Add("mssqlserver", MsSqlDatabaseStringBuilder(session));
            }
            else if (DbEngine.MySql.Equals(dbEngine, StringComparison.InvariantCultureIgnoreCase))
            {
                resultMap.Add("mysqlserver", MySqlDatabaseStringBuilder(session));
            }
            else if (DbEngine.Oracle.Equals(dbEngine, StringComparison.InvariantCultureIgnoreCase))
            {
                resultMap.Add("oracleserver", OracleDatabaseStringBuilder(session));
            }
            else if (DbEngine.PostGre.Equals(dbEngine, StringComparison.InvariantCultureIgnoreCase))
            {
                resultMap.Add("postgreserver", PostGreDatabaseStringBuilder(session));
            }
            
            resultMap.Add("webport", session.Property("PORT"));

            return resultMap;
        }

        private static Dictionary<string, string> SonarPropertiesMap = new Dictionary<string, string>()
        {
            { "username", "sonar.jdbc.username={0}" },
            { "password", "sonar.jdbc.password={0}" },
            { "mssqlserver", "sonar.jdbc.url=jdbc:sqlserver://{0}" },
            { "mysqlserver", "sonar.jdbc.url=jdbc:mysql://{0}" },
            { "oracleserver", "sonar.jdbc.url=jdbc:oracle:thin:@{0}" },
            { "postgreserver", "sonar.jdbc.url=jdbc:postgresql://{0}" },
            { "webport", "sonar.web.port={0}" }
        };

        private static string MsSqlDatabaseStringBuilder(Session session)
        {
            StringBuilder result = new StringBuilder();

            string instancePropertyValue = session.Property("INSTANCE");
            string serverName = string.Empty;
            string instanceName = string.Empty;
            string databaseName = session.Property("DATABASENAME");
            string authType = session.Property("SQLAUTHTYPE");

            // For named instance
            if (instancePropertyValue.Contains(@"\"))
            {
                serverName = instancePropertyValue.Substring(0, instancePropertyValue.IndexOf('\\'));
                instanceName = instancePropertyValue.Substring(instancePropertyValue.IndexOf('\\') + 1);

                if (serverName.Contains(@","))
                {
                    //Format: MachineName,Port\InstanceName
                    string serverPort = serverName.Substring(instancePropertyValue.IndexOf(',') + 1);
                    serverName = serverName.Substring(0, instancePropertyValue.IndexOf(','));
                    result.Append(string.Format("{0}:{1};Instance={2}", serverName, serverPort, instanceName));
                }
                else
                {
                    //Format: MachineName\InstanceName
                    result.Append(string.Format("{0};Instance={1}", serverName, instanceName));
                }
            }
            else
            {
                serverName = instancePropertyValue;

                if (serverName.Contains(@","))
                {
                    //Format: MachineName,Port
                    string serverPort = serverName.Substring(instancePropertyValue.IndexOf(',') + 1);
                    serverName = serverName.Substring(0, instancePropertyValue.IndexOf(','));
                    result.Append(string.Format("{0}:{1}", serverName, serverPort));
                }
                else
                {
                    //Format: MachineName
                    result.Append(serverName);
                }                
            }

            result.Append(string.Format(";databaseName={0}", databaseName));

            if (AuthenticationType.Windows.Equals(authType, StringComparison.InvariantCultureIgnoreCase))
            {
                result.Append(";IntegratedSecurity=True");
            }

            return result.ToString();
        }

        private static string MySqlDatabaseStringBuilder(Session session)
        {
            // Used format: sonar.jdbc.url=jdbc:mysql://localhost:3306/sonar?useUnicode=true&characterEncoding=utf8&rewriteBatchedStatements=true&useConfigs=maxPerformance
            string databaseServer = session.Property("INSTANCE");
            string databaseName = session.Property("DATABASENAME");
            string dbPort = session.Property("DBPORT");

            return string.Format(@"{0}:{1}/{2}?useUnicode=true&characterEncoding=utf8&rewriteBatchedStatements=true&useConfigs=maxPerformance",
                                 databaseServer,
                                 dbPort,
                                 databaseName);
        }

        private static string OracleDatabaseStringBuilder(Session session)
        {
            // Used format: sonar.jdbc.url=jdbc:oracle:thin:@localhost:1521/XE
            string databaseServer = session.Property("INSTANCE");
            string databaseName = session.Property("DATABASENAME");
            string dbPort = session.Property("DBPORT");

            return string.Format(@"{0}:{1}/{2}", databaseServer, dbPort, databaseName);
        }
        
        private static string PostGreDatabaseStringBuilder(Session session)
        {
            // Used format: sonar.jdbc.url=jdbc:postgresql://localhost:5432/sonar
            string databaseServer = session.Property("INSTANCE");
            string databaseName = session.Property("DATABASENAME");
            string dbPort = session.Property("DBPORT");

            return string.Format(@"{0}:{1}/{2}", databaseServer, dbPort, databaseName);
        }
    }
}

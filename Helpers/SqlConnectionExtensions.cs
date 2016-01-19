//----------------------------------------------------------------------------------------------
// <copyright file="SqlConnectionExtensions.cs" company="SonarSource SA and Microsoft Corporation">
// Copyright (c) SonarSource SA and Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
//----------------------------------------------------------------------------------------------

using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using System.Data;
using System.Data.SqlClient;

namespace SonarQubeBootstrapper.Helpers
{
    public static class SqlConnectionExtensions
    {
        /// <summary>
        /// Creates a new sql server login that is used to create users in database
        /// </summary>
        /// <remarks>Needs connection to sql server only without specifying any database</remarks>
        public static bool CreateNewSqlServerLogin(this SqlConnection connection, string username, string password)
        {
            bool result = false;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("CREATE LOGIN {0} WITH PASSWORD='{1}', CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF", username, password);
                cmd.ExecuteNonQuery();

                result = true;
            }
            catch (SqlException e)
            {
                BootstrapperManager.Instance.Bootstrapper.Engine.Log(LogLevel.Error, string.Format("[CreateNewSqlServerLogin] {0}", e.Message));
            }

            return result;
        }

        /// <summary>
        /// Creates a new sql database
        /// </summary>
        /// <remarks>Needs connection to sql server only without specifying any database</remarks>
        public static bool CreateSqlServerDatabase(this SqlConnection connection, string databaseName)
        {
            bool result = false;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("CREATE DATABASE \"{0}\" COLLATE SQL_Latin1_General_CP1_CS_AS", databaseName);
                cmd.ExecuteNonQuery();

                result = true;
            }
            catch (SqlException e)
            {
                BootstrapperManager.Instance.Bootstrapper.Engine.Log(LogLevel.Error, string.Format("[CreateSqlServerDatabase] {0}", e.Message));
            }

            return result;
        }

        /// <summary>
        /// Creates a new user for a database from a login
        /// </summary>
        /// <remarks>Needs connection to sql server with a database</remarks>
        public static bool CreateSqlUserFromSqlLogin(this SqlConnection connection, string username, string loginName)
        {
            bool result = false;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = string.Format("CREATE USER {0} from LOGIN \"{1}\"", username, loginName);
                cmd.ExecuteNonQuery();

                result = true;
            }
            catch (SqlException e)
            {
                BootstrapperManager.Instance.Bootstrapper.Engine.Log(LogLevel.Error, string.Format("[CreateSqlUserFromSqlLogin] {0}", e.Message));
            }

            return result;
        }

        /// <summary>
        /// Changes role of the user for given database
        /// </summary>
        /// <param name="rolename">eg. db_owner</param>
        /// <param name="username">user name</param>
        /// <remarks>Needs connection to sql server with a database</remarks>
        public static bool ChangeSqlUserDbRole(this SqlConnection connection, string rolename, string username)
        {
            bool result = false;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = @"sp_addrolemember";
                cmd.Parameters.Add("@rolename", SqlDbType.VarChar).Value = rolename;
                cmd.Parameters.Add("@membername", SqlDbType.VarChar).Value = username;
                cmd.ExecuteNonQuery();

                result = true;
            }
            catch (SqlException e)
            {
                BootstrapperManager.Instance.Bootstrapper.Engine.Log(LogLevel.Error, string.Format("[ChangeSqlUserDbRole] {0}", e.Message));
            }

            return result;
        }
    }
}

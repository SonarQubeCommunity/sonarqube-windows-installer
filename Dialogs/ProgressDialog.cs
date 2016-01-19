//----------------------------------------------------------------------------------------------
// <copyright file="ProgressDialog.cs" company="SonarSource SA and Microsoft Corporation">
// Copyright (c) SonarSource SA and Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
//----------------------------------------------------------------------------------------------

using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using SonarQubeBootstrapper.Helpers;
using SonarQubeBootstrapper.Resources;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Wix.SonarQube.Dialogs
{
    /// <summary>
    /// The standard Installation Progress dialog
    /// </summary>
    public partial class ProgressDialog : BAForm
    {
        private string packageName = String.Empty;
        private bool setupFailed = false;
        private int totalActions = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressDialog"/> class.
        /// </summary>
        public ProgressDialog()
        {
            InitializeComponent();
        }

        void ProgressDialog_Load(object sender, EventArgs e)
        {
            this.Text = BootstrapperConstants.FormalProductName;
            this.next.Text = BootstrapperResources.NextButtonText;
            this.cancel.Text = BootstrapperResources.CancelButtonText;
            this.back.Text = BootstrapperResources.BackButtonText;          
            this.description.Text = BootstrapperResources.ProgressDialogSubtitle;
            this.label4.Text = string.Format(ProgressStatus(), packageName);
            this.cancel.Click += new System.EventHandler(base.cancel_Click);

            banner.Image = BootstrapperResources.sonarqube_banner;
            this.Icon = BootstrapperResources.setup;

            if (BootstrapperManager.Instance.LaunchAction == LaunchAction.Install)
            {
                this.label1.Text = string.Format(BootstrapperResources.ProgressDialogInstallingTitle, BootstrapperConstants.FormalProductName);
                totalActions = BootstrapperConstants.InstallCustomActionsCount;
            }
            else if (BootstrapperManager.Instance.LaunchAction == LaunchAction.Uninstall)
            {
                this.label1.Text = string.Format(BootstrapperResources.ProgressDialogUninstallingTitle, BootstrapperConstants.FormalProductName);
                totalActions = BootstrapperConstants.UninstallCustomActionsCount;
            }

            BootstrapperManager.Instance.Bootstrapper.ExecuteProgress += Bootstrapper_ExecuteProgress;
            BootstrapperManager.Instance.Bootstrapper.ApplyComplete += Bootstrapper_ApplyComplete;
            BootstrapperManager.Instance.Bootstrapper.ExecutePackageComplete += Bootstrapper_ExecutePackageComplete;
            BootstrapperManager.Instance.Bootstrapper.ExecutePackageBegin += Bootstrapper_ExecutePackageBegin;
            BootstrapperManager.Instance.Bootstrapper.ExecuteMsiMessage += Bootstrapper_ExecuteMsiMessage;
            BootstrapperManager.Instance.ExecutePlan();
        }

        private void Bootstrapper_ExecuteMsiMessage(object sender, ExecuteMsiMessageEventArgs e)
        {
            if (e.Message != null &&
                e.MessageType == InstallMessage.Warning)
            {
                MessageId flag;
                var fields = e.Data.ToList();

                if (fields.Count >= 1
                    && Enum.TryParse(fields[0], out flag))
                {
                    switch (flag)
                    {
                        case MessageId.Progress:
                            if (fields.Count == 3)
                            {
                                this.Invoke((MethodInvoker)delegate
                                {
                                    int progress = 0;

                                    if (Int32.TryParse(fields[2], out progress))
                                    {
                                        this.label4.Text = fields[1];

                                        this.progress.Value = (int)((double)progress / (double)totalActions * 100.0);
                                    }
                                });
                            }
                            break;

                        case MessageId.MessageBox:
                            if (fields.Count == 2)
                            {
                                MessageBox.Show(this, fields[1]);
                            }
                            break;
                    }
                }
            }
        }

        private void Bootstrapper_ExecutePackageBegin(object sender, ExecutePackageBeginEventArgs e)
        {
            string setupType = BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["SETUPTYPE"];

            if (BootstrapperManager.Instance.SetupState != SetupState.Success)
            {
                e.Result = Result.Cancel;
                return;
            }
            
            // If we are going to start SonarQubePackage installation and
            // the setup type is Express, we need to setup database for SonarQube
            if (e.PackageId.Equals("SonarQubePackage") &&
                setupType.Equals(SetupType.Express) &&
                BootstrapperManager.Instance.LaunchAction == LaunchAction.Install)
            {
                try
                {
                    SetupSonarQubeDatabaseForExpressInstallation();
                }
                catch (Exception ex)
                {
                    this.setupFailed = true;

                    BootstrapperManager.Instance.Bootstrapper.Engine.Log(LogLevel.Error, string.Format("[Bootstrapper_ExecutePackageBegin] {0}", ex.Message));
                }
            }
        }

        private void Bootstrapper_ExecutePackageComplete(object sender, ExecutePackageCompleteEventArgs e)
        {
            if (e.Status != 0)
            {
                BootstrapperManager.Instance.SetupState = SetupState.FailedOnOtherReason;

                return;
            }

            // If SqlExpressPackage has been installed which means that SQL setup has
            // been extracted, we need to execute setup in order to install SQL express
            // before we install and start SonarQube setup
            if (e.PackageId.Equals("SqlExpressPackage") &&
                BootstrapperManager.Instance.LaunchAction == LaunchAction.Install)
            {
                if (InstallSqlExpress())
                {
                    TCPPortHelper.EnablePortForFirewall("sonarsqlexpress", BootstrapperConstants.MSSqlPortNumber);
                }
                else
                {
                    this.setupFailed = true;
                    BootstrapperManager.Instance.SetupState = SetupState.FailedOnSqlSetup;
                }
            }

            // If SonarQubePackage has been installed add its port to firewall
            if (BootstrapperManager.Instance.SetupState == SetupState.Success && 
                e.PackageId.Equals("SonarQubePackage") &&
                BootstrapperManager.Instance.LaunchAction == LaunchAction.Install)
            {
                TCPPortHelper.EnablePortForFirewall("SonarQube", Int32.Parse(BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["PORT"]));                
            }
        }

        private void Bootstrapper_ApplyComplete(object sender, ApplyCompleteEventArgs e)
        {
            if (e.Status != 0)
            {
                BootstrapperManager.Instance.SetupExitState = Result.Cancel;
            }

            this.Invoke((MethodInvoker)delegate
            {
                DialogManager.Instance.Go<ExitDialog>();
            });
        }

        private void Bootstrapper_ExecuteProgress(object sender, ExecuteProgressEventArgs e)
        {
            // If setup has failed, set the Result to Error to let Engine know
            // about the state.
            if (this.setupFailed)
            {
                e.Result = Result.Cancel;

                this.Invoke((MethodInvoker)delegate
                {
                    this.label4.Text = BootstrapperResources.ProgressSetupFailedMessage;
                });
            }
            else
            {
                this.Invoke((MethodInvoker)delegate
                {
                    this.packageName = MsiPackages.MsiPackageMap[e.PackageId];
                    this.label4.Text = string.Format(ProgressStatus(), packageName);
                });
            }
        }

        protected override void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Set the setupFailed to true on cancel button click to propagate the message
            // to engine to start roll back.
            this.setupFailed = true;
        }

        protected override void cancel_Click(object sender, EventArgs e)
        {
            // Set the setupFailed to true on cancel button click to propagate the message
            // to engine to start roll back.
            this.setupFailed = true;
        }

        string ProgressStatus()
        {
            string progressFormatString = string.Empty;

            switch (BootstrapperManager.Instance.LaunchAction)
            {
                case LaunchAction.Install:
                    progressFormatString = BootstrapperResources.ProgressDialogInstallProgress;
                    break;
                case LaunchAction.Uninstall:
                    progressFormatString = BootstrapperResources.ProgressDialogUninstallProgress;
                    break;
                case LaunchAction.Repair:
                    progressFormatString = BootstrapperResources.ProgressDialogRepairProgress;
                    break;
            }

            return progressFormatString;
        }

        /// <summary>
        /// Installs SQL express setup.
        /// </summary>
        bool InstallSqlExpress()
        {
            bool result = false;

            try
            {
                Process process = new Process();

                string setupPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), @"SqlExpress.exe");
                string configPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), @"ConfigurationFile.ini");

                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.CreateNoWindow = true;

                string currentPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                process.StartInfo.FileName = setupPath;
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.Arguments = "/IACCEPTSQLSERVERLICENSETERMS /SAPWD=\"p@ssword1\" /ConfigurationFile=" + configPath;
                process.EnableRaisingEvents = true;

                process.Start();

                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                BootstrapperManager.Instance.Bootstrapper.Engine.Log(LogLevel.Error,
                                                                     string.Format("[InstallSqlExpress] {0}", e.Message));
            }

            // Do the installation verification from the log file as well.
            result = result && VerifySqlInstallationFromSummaryLog();

            return result;
        }

        /// <summary>
        /// Sets up the sql database for SonarQube service.
        /// </summary>
        void SetupSonarQubeDatabaseForExpressInstallation()
        {
            string databaseName = BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["DATABASENAME"];
            string userName = BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["DATABASEUSERNAME"];
            string password = BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["DATABASEPASSWORD"];
            string dataSource = BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["INSTANCE"];
            string sqlAuthType = BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["SQLAUTHTYPE"];
            string installSql = BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["INSTALLSQL"];

            // Scenario 1 - Sql New Installation
            //              a) Windows Authentication
            //                 -- Only create db (local user gets permissions by default)
            //              b) Sql Authentication
            //                 -- Create login with given credentials -> create db -> create user with created login -> make user db_owner
            // Scenario 2 - Sql Old Installation
            //              a) Windows Authentication
            //                 -- Create db
            //              b) Sql Authentication
            //                 -- Create db using given login -> create user for given login -> make user db_owner

            if (AuthenticationType.Windows.Equals(sqlAuthType, StringComparison.InvariantCultureIgnoreCase))
            {
                using (SqlConnection connection = SqlServerHelper.CreateSqlConnectionWithWindowsLogin(dataSource))
                {
                    connection.Open();

                    connection.CreateSqlServerDatabase(databaseName);
                }
            }
            else
            {
                // If sql has been installed by our installer and user has chosen the option to have sql authentication,
                // we will have to use windows authentication to create that login.
                if (installSql.Equals("TRUE", StringComparison.InvariantCultureIgnoreCase))
                {
                    using (SqlConnection connection = SqlServerHelper.CreateSqlConnectionWithWindowsLogin(dataSource))
                    {
                        connection.Open();

                        connection.CreateNewSqlServerLogin(userName, password);
                        connection.CreateSqlServerDatabase(databaseName);
                    }

                    using (SqlConnection connection = SqlServerHelper.CreateSqlConnectionWithSqlLogin(dataSource, userName, password, databaseName))
                    {
                        connection.Open();

                        connection.CreateSqlUserFromSqlLogin(userName, userName);
                        connection.ChangeSqlUserDbRole("db_owner", userName);
                    }
                }
                else
                {
                    using (SqlConnection connection = SqlServerHelper.CreateSqlConnectionWithSqlLogin(dataSource, userName, password))
                    {
                        connection.Open();

                        connection.CreateSqlServerDatabase(databaseName);
                    }

                    using (SqlConnection connection = SqlServerHelper.CreateSqlConnectionWithSqlLogin(dataSource, userName, password, databaseName))
                    {
                        connection.Open();

                        connection.CreateSqlUserFromSqlLogin(userName, userName);
                        connection.ChangeSqlUserDbRole("db_owner", userName);
                    }
                }

                // For sql authentication, SonarQube service runs as SYSTEM, 
                // We need to give permission to this account in db.
                using (SqlConnection connection = SqlServerHelper.CreateSqlConnectionWithSqlLogin(dataSource, userName, password, databaseName))
                {
                    connection.Open();

                    connection.CreateSqlUserFromSqlLogin("NTSYSTEM", @"NT AUTHORITY\SYSTEM");
                    connection.ChangeSqlUserDbRole("db_owner", "NTSYSTEM");
                }
            }
        }

        /// <summary>
        /// Read SQL summary log and verify if the installation of SQL is successful.
        /// </summary>
        /// <remarks>Only applicable to SQL 2012</remarks>
        bool VerifySqlInstallationFromSummaryLog()
        {
            bool result = false;

            try
            {
                var sqlLogFilePath = Environment.ExpandEnvironmentVariables(BootstrapperConstants.SqlSummaryLogFilePath);
                var sqlLogFile = File.ReadAllLines(sqlLogFilePath);
                var finalResult = sqlLogFile.Where(x => x.Contains("Final result:"));

                if (finalResult.Any())
                {
                    if (finalResult.FirstOrDefault().Contains("Passed"))
                    {
                        result = true;
                    }
                }
            }
            catch (Exception e)
            {
                BootstrapperManager.Instance.Bootstrapper.Engine.Log(LogLevel.Error, string.Format("[ReadSqlInstallationSummaryLog] {0}", e.Message));
            }

            return result;
        }
    }
}
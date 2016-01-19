//----------------------------------------------------------------------------------------------
// <copyright file="ExitDialog.cs" company="SonarSource SA and Microsoft Corporation">
// Copyright (c) SonarSource SA and Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
//----------------------------------------------------------------------------------------------

using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using SonarQubeBootstrapper.Resources;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Wix.SonarQube.Dialogs
{
    /// <summary>
    /// The standard Exit dialog
    /// </summary>
    public partial class ExitDialog : BAForm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExitDialog"/> class.
        /// </summary>
        public ExitDialog()
        {
            InitializeComponent();
        }

        void ExitDialog_Load(object sender, System.EventArgs e)
        {
            this.Text = BootstrapperConstants.FormalProductName;
            this.finish.Text = BootstrapperResources.FinishButtonText;
            this.label1.Text = BootstrapperResources.ExitDialogTitle;
            this.Icon = BootstrapperResources.setup;

            if ((BootstrapperManager.Instance.SetupExitState == Result.Ok ||
                BootstrapperManager.Instance.SetupExitState == Result.Yes ||
                BootstrapperManager.Instance.SetupExitState == Result.None) &&
                BootstrapperManager.Instance.SetupState == SetupState.Success)
            {
                string port = BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["PORT"];

                serviceWebLink.Text = string.Format(BootstrapperResources.ExitDialogServiceWebLink, port);
                serviceWebLink.Visible = true;

                description.Text = BootstrapperResources.ExitDialogSuccessDescription;
            }
            else
            {
                description.Text = BootstrapperResources.ExitDialogErrorDescription;
            }

            if (BootstrapperManager.Instance.SetupState == SetupState.FailedOnSqlSetup)
            {
                viewSqlLog.Visible = true;
                label2.Visible = true;
                label2.Text = BootstrapperResources.ExitDialogSqlFailureDescription;
            }
        }

        void viewLog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (!Directory.Exists(BootstrapperManager.Instance.LogFilePath))
                {
                    Directory.CreateDirectory(BootstrapperManager.Instance.LogFilePath);

                    foreach (string logVariable in BootstrapperManager.Instance.LogVariables)
                    {
                        if (BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables.Contains(logVariable))
                        {
                            string file = BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables[logVariable];

                            if (File.Exists(file))
                            {
                                FileInfo fileInfo = new FileInfo(file);
                                fileInfo.CopyTo(Path.Combine(BootstrapperManager.Instance.LogFilePath, fileInfo.Name), false);
                            }
                        }
                    }
                }

                Process.Start(BootstrapperManager.Instance.LogFilePath);
            }
            catch { }
        }

        void viewSqlLog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                string sqlLogPath = Environment.ExpandEnvironmentVariables(BootstrapperConstants.SqlSummaryLogFilePath);

                Process.Start(sqlLogPath);
            }
            catch { }
        }

        void finish_Click(object sender, System.EventArgs e)
        {
            this.Dispose();
            Application.Exit();
        }
    }
}

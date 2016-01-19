//----------------------------------------------------------------------------------------------
// <copyright file="CustomInstallationSummaryDialog.cs" company="SonarSource SA and Microsoft Corporation">
// Copyright (c) SonarSource SA and Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
//----------------------------------------------------------------------------------------------

using SonarQubeBootstrapper.Helpers;
using SonarQubeBootstrapper.Resources;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Wix.SonarQube.Dialogs
{
    /// <summary>
    /// The standard InstallDir dialog
    /// </summary>
    public partial class CustomInstallationSummaryDialog : BAForm
    {
        public CustomInstallationSummaryDialog()
        {
            InitializeComponent();
        }

        void CustomInstallationSummaryDialog_Load(object sender, System.EventArgs e)
        {
            banner.Image = BootstrapperResources.sonarqube_banner;
            this.Icon = BootstrapperResources.setup;
            this.Text = BootstrapperConstants.FormalProductName;
            this.next.Text = BootstrapperResources.InstallButtonText;
            this.cancel.Text = BootstrapperResources.CancelButtonText;
            this.back.Text = BootstrapperResources.BackButtonText;

            this.label1.Text = BootstrapperResources.InstallationSummaryTitle;
            this.label2.Text = BootstrapperResources.InstallationSummaryDescription;
            this.label3.Text = BootstrapperResources.InstallationSummaryInstallLocation;
            this.label4.Text = BootstrapperResources.InstallationSummaryHTTPPort;
            this.label5.Text = BootstrapperResources.InstallationSummaryInstallService;
            this.label6.Text = BootstrapperResources.InstallationSummaryDatabaseType;
            this.label8.Text = BootstrapperResources.CustomInstallationSummaryServerAddress;
            this.label9.Text = BootstrapperResources.CustomInstallationSummaryDatabaseName;
            this.label10.Text = BootstrapperResources.CustomInstallationSummaryUserName;

            this.label7.Text = BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["DBENGINE"];
            this.label11.Text = BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["INSTANCE"];
            this.label12.Text = BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["DATABASENAME"];
            this.label13.Text = BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["DATABASEUSERNAME"];

            this.installationDirTextBox.Text = Environment.ExpandEnvironmentVariables(BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["INSTALLDIR"]);
            this.sonarWebPortTextBox.Text = BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["PORT"];

            this.cancel.Click += new System.EventHandler(base.cancel_Click);
        }

        void next_Click(object sender, EventArgs e)
        {
            BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["INSTALLDIR"] = this.installationDirTextBox.Text;
            BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["PORT"] = this.sonarWebPortTextBox.Text;
            BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["INSTALLSERVICE"] = this.installServiceComboBox.Text;
            

            DialogManager.Instance.Go<ProgressDialog>();
        }

        void back_Click(object sender, EventArgs e)
        {
            DialogManager.Instance.Previous();
        }

        string GenerateSonarQubeDatabaseName()
        {
            string result = BootstrapperResources.InstallationSummaryDatabaseName + "_" + Guid.NewGuid().ToString();

            return result;
        }

        private void selectInstallationPathButton_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.installationDirTextBox.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
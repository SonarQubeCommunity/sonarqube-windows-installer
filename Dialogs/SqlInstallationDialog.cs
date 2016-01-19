//----------------------------------------------------------------------------------------------
// <copyright file="SqlInstallationDialog.cs" company="SonarSource SA and Microsoft Corporation">
// Copyright (c) SonarSource SA and Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
//----------------------------------------------------------------------------------------------

using SonarQubeBootstrapper.Helpers;
using SonarQubeBootstrapper.Resources;
using System;
using System.Drawing;

namespace Wix.SonarQube.Dialogs
{
    public partial class SqlInstallationDialog : BAForm
    {
        private string setupPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), @"SqlExpress.exe");
        private string configPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), @"ConfigurationFile.ini");

        public SqlInstallationDialog()
        {
            InitializeComponent();
        }

        void SQLInstallationDialog_Load(object sender, System.EventArgs e)
        {
            this.Text = BootstrapperConstants.FormalProductName;
            this.back.Text = BootstrapperResources.BackButtonText;
            this.next.Text = BootstrapperResources.NextButtonText;
            this.cancel.Text = BootstrapperResources.CancelButtonText;
            banner.Image = BootstrapperResources.sonarqube_banner;
            this.Icon = BootstrapperResources.setup;

            this.cancel.Click += new System.EventHandler(base.cancel_Click);

            this.label1.Text = BootstrapperResources.SqlInstallationTitle;
            this.label2.Text = BootstrapperResources.SqlInstallationDescription;
            this.label3.Text = BootstrapperResources.SqlInstallationInstallSqlDescription;
            this.label4.Text = BootstrapperResources.SqlInstallationExistingSqlDescription;
            this.label5.Text = BootstrapperResources.SqlInstallationSqlAlreadyInstalled;
            this.label5.Visible = false;
            this.informationImage.Visible = false;

            UpdateUI();
        }

        void next_Click(object sender, EventArgs e)
        {
            string setupType = BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["SETUPTYPE"];

            if (this.installSqlExpressRadioButton.Checked)
            {
                BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["INSTALLSQL"] = "TRUE";

                DialogManager.Instance.Go<SqlServerNewUserCredentialsDialog>();
            }
            else
            {
                DialogManager.Instance.Go<SqlServerInstanceValidateDialog>();
            }
        }

        void back_Click(object sender, EventArgs e)
        {
            DialogManager.Instance.Previous();
        }

        private void UpdateUI()
        {
            if (SqlServerHelper.IsSqlServerInstalled())
            {
                this.label5.Visible = true;
                this.installSqlExpressRadioButton.Enabled = false;
                this.installSqlExpressRadioButton.Checked = false;
                this.useExistingInstanceRadioButton.Checked = true;
                this.informationImage.Visible = true;
            }
        }
    }
}

//----------------------------------------------------------------------------------------------
// <copyright file="SqlServerNewUserCredentialsDialog.cs" company="SonarSource SA and Microsoft Corporation">
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
    public partial class SqlServerNewUserCredentialsDialog : BAForm
    {
        public SqlServerNewUserCredentialsDialog()
        {
            InitializeComponent();
        }

        void SqlServerNewUserCredentialsDialog_Load(object sender, System.EventArgs e)
        {
            banner.Image = BootstrapperResources.sonarqube_banner;

            this.Icon = BootstrapperResources.setup;
            this.Text = BootstrapperConstants.FormalProductName;
            this.back.Text = BootstrapperResources.BackButtonText;
            this.next.Text = BootstrapperResources.NextButtonText;
            this.cancel.Text = BootstrapperResources.CancelButtonText;
            this.cancel.Click += new System.EventHandler(base.cancel_Click);

            this.label1.Text = BootstrapperResources.SqlServerNewUserCredentialsTitle;
            this.label2.Text = BootstrapperResources.SqlServerNewUserCredentialsDescription;
            this.label5.Text = BootstrapperResources.SqlServerInstanceUserNameLabel;
            this.label7.Text = BootstrapperResources.SqlServerInstancePasswordLabel;
            
            this.authenticationTypeCheckBox.Text = BootstrapperResources.SqlServerInstanceWindowsCredentialsLabel;
            this.authenticationTypeCheckBox.CheckStateChanged += AuthenticationTypeCheckBox_CheckStateChanged;

            string authenticationType = BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["SQLAUTHTYPE"];

            if (AuthenticationType.Windows.Equals(authenticationType, StringComparison.InvariantCultureIgnoreCase))
            {
                this.authenticationTypeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            }
            else
            {
                this.authenticationTypeCheckBox.CheckState = System.Windows.Forms.CheckState.Unchecked;
            }
        }

        void next_Click(object sender, EventArgs e)
        {
            BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["DATABASEUSERNAME"] = this.userNameTextBox.Text;
            BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["DATABASEPASSWORD"] = this.passwordTextBox.Text;

            DialogManager.Instance.Go<SonarQubeOptionsEditDialog>();
        }

        void back_Click(object sender, EventArgs e)
        {
            DialogManager.Instance.Previous();
        }
        
        private void AuthenticationTypeCheckBox_CheckStateChanged(object sender, System.EventArgs e)
        {
            if (authenticationTypeCheckBox.CheckState == System.Windows.Forms.CheckState.Checked)
            {
                this.userNameTextBox.Enabled = false;
                this.passwordTextBox.Enabled = false;

                BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["SQLAUTHTYPE"] = AuthenticationType.Windows;
            }
            else
            {
                this.userNameTextBox.Enabled = true;
                this.passwordTextBox.Enabled = true;

                BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["SQLAUTHTYPE"] = AuthenticationType.Sql;
            }
        }
    }
}

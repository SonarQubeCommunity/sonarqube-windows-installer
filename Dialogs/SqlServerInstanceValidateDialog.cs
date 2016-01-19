//----------------------------------------------------------------------------------------------
// <copyright file="SqlServerInstanceValidateDialog.cs" company="SonarSource SA and Microsoft Corporation">
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
    public partial class SqlServerInstanceValidateDialog : BAForm
    {
        public SqlServerInstanceValidateDialog()
        {
            InitializeComponent();
        }

        void SQLServerInstanceValidateDialog_Load(object sender, System.EventArgs e)
        {
            this.Text = BootstrapperConstants.FormalProductName;
            this.back.Text = BootstrapperResources.BackButtonText;
            this.next.Text = BootstrapperResources.NextButtonText;
            this.cancel.Text = BootstrapperResources.CancelButtonText;
            banner.Image = BootstrapperResources.sonarqube_banner;
            this.Icon = BootstrapperResources.setup;

            this.label1.Text = BootstrapperResources.SqlServerInstanceTitle;
            this.label6.Text = BootstrapperResources.SqlServerInstanceSubtitle;
            this.label2.Text = BootstrapperResources.SqlServerInstanceDescription;
            this.label3.Text = BootstrapperResources.SqlServerInstanceNameLabel;
            this.label5.Text = BootstrapperResources.SqlServerInstanceUserNameLabel;
            this.label7.Text = BootstrapperResources.SqlServerInstancePasswordLabel;
            this.label8.Text = BootstrapperResources.SqlServerInstanceFailedAuthenticationMessage;
            this.label8.Visible = false;

            this.authenticationTypeCheckBox.Text = BootstrapperResources.SqlServerInstanceWindowsCredentialsLabel;
            this.authenticationTypeCheckBox.CheckStateChanged += AuthenticationTypeCheckBox_CheckStateChanged;
            this.checkConnectionLink.Click += CheckConnectionLink_Click;
            this.sqlInstanceTextBox.TextChanged += TextBox_TextChanged;
            this.userNameTextBox.TextChanged += TextBox_TextChanged;
            this.passwordTextBox.TextChanged += TextBox_TextChanged;
            this.cancel.Click += new System.EventHandler(base.cancel_Click);

            this.sqlConnectionInfo.Image = (System.Drawing.Image)BootstrapperResources.ResourceManager.GetObject("sqlConnectionInfo");
            this.sqlConnectionInfo.MouseHover += SqlConnectionInfo_MouseHover;

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

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (this.connectionStatusImage.Image != null)
            {
                this.connectionStatusImage.Image = null;
            }
			
			if (this.sqlInstanceTextBox.Text.Equals("", StringComparison.InvariantCultureIgnoreCase))
            {
                this.next.Enabled = false;
            }
            else
            {
                this.next.Enabled = true;
            }
        }

        void next_Click(object sender, EventArgs e)
        {
            BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["INSTANCE"] = this.sqlInstanceTextBox.Text;
            BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["DATABASEUSERNAME"] = this.userNameTextBox.Text;
            BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["DATABASEPASSWORD"] = this.passwordTextBox.Text;

            DialogManager.Instance.Go<SonarQubeOptionsEditDialog>();
        }

        void back_Click(object sender, EventArgs e)
        {
            DialogManager.Instance.Previous();
        }
        
        private void CheckConnectionLink_Click(object sender, System.EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (authenticationTypeCheckBox.Checked)
            {
                if (SqlServerHelper.TestSqlServerConnectionWithWindowsLogin(this.sqlInstanceTextBox.Text))
                {
                    this.connectionStatusImage.Image = BootstrapperResources.succeeded;
                }
                else
                {
                    this.connectionStatusImage.Image = BootstrapperResources.failed;
                }
            }
            else
            {
                if (SqlServerHelper.TestSqlConnectionWithSqlServerLogin(this.sqlInstanceTextBox.Text,
                                                                         this.userNameTextBox.Text,
                                                                         this.passwordTextBox.Text))
                {
                    this.connectionStatusImage.Image = BootstrapperResources.succeeded;
                }
                else
                {
                    this.connectionStatusImage.Image = BootstrapperResources.failed;
                }
            }
            this.Cursor = Cursors.Arrow;
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

        private void SqlConnectionInfo_MouseHover(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(this.sqlConnectionInfo, BootstrapperConstants.SqlConnectionInfo);
        }
    }
}

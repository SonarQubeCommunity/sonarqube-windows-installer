//----------------------------------------------------------------------------------------------
// <copyright file="ConfigureOtherDatabaseConnection.cs" company="SonarSource SA and Microsoft Corporation">
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
    /// The standard Setup Type dialog
    /// </summary>
    public partial class ConfigureOtherDatabaseConnection : BAForm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetupTypeDialog"/> class.
        /// </summary>
        public ConfigureOtherDatabaseConnection()
        {
            InitializeComponent();

            this.Text = BootstrapperConstants.FormalProductName;
            this.next.Text = BootstrapperResources.NextButtonText;
            this.cancel.Text = BootstrapperResources.CancelButtonText;
            this.back.Text = BootstrapperResources.BackButtonText;
            this.Icon = BootstrapperResources.setup;

            this.label1.Text = BootstrapperResources.ConfigureOtherDatabaseConnectionDialogTitle;
            this.label2.Text = BootstrapperResources.ConfigureOtherDatabaseConnectionDialogSubtitle;
            this.label3.Text = BootstrapperResources.ConfigureOtherDatabaseConnectionDialogServerAddressLabel;
            this.label4.Text = BootstrapperResources.ConfigureOtherDatabaseConnectionDialogDatabaseNameLabel;
            this.label5.Text = BootstrapperResources.ConfigureOtherDatabaseConnectionDialogUsernameLabel;
            this.label6.Text = BootstrapperResources.ConfigureOtherDatabaseConnectionDialogPasswordLabel;
            this.label7.Text = BootstrapperResources.ConfigureOtherDatabaseConnectionDialogPortLabel;

            this.radioButton1.Text = BootstrapperResources.ConfigureOtherDatabaseConnectionDialogMySQLLabel;
            this.radioButton2.Text = BootstrapperResources.ConfigureOtherDatabaseConnectionDialogOracleLabel;
            this.radioButton3.Text = BootstrapperResources.ConfigureOtherDatabaseConnectionDialogPostgreLabel;

            this.radioButton1.CheckedChanged += DbEngineOptionChanged;
            this.radioButton2.CheckedChanged += DbEngineOptionChanged;
            this.radioButton3.CheckedChanged += DbEngineOptionChanged;

            this.radioButton1.Checked = true;

            this.cancel.Click += new System.EventHandler(base.cancel_Click);
        }

        private void DbEngineOptionChanged(object sender, EventArgs e)
        {
            var dbEngineOption = sender as RadioButton;

            if (dbEngineOption != null)
            {
                if (dbEngineOption == radioButton1)
                {
                    this.textBox5.Text = BootstrapperConstants.MySqlPortNumber.ToString();
                }
                else if (dbEngineOption == radioButton2)
                {
                    this.textBox5.Text = BootstrapperConstants.OraclePortNumber.ToString();
                }
                else if (dbEngineOption == radioButton3)
                {
                    this.textBox5.Text = BootstrapperConstants.PostGrePortNumber.ToString();
                }
            }
        }

        void back_Click(object sender, System.EventArgs e)
        {
            DialogManager.Instance.Previous();
        }

        void next_Click(object sender, System.EventArgs e)
        {
            BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["INSTANCE"] = this.textBox1.Text;
            BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["DATABASENAME"] = this.textBox2.Text;
            BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["DATABASEUSERNAME"] = this.textBox3.Text;
            BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["DATABASEPASSWORD"] = this.textBox4.Text;
            BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["DBPORT"] = this.textBox5.Text;

            if (this.radioButton1.Checked)
            {
                BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["DBENGINE"] = DbEngine.MySql;
            }
            else if (this.radioButton2.Checked)
            {
                BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["DBENGINE"] = DbEngine.Oracle;
            }
            else if (this.radioButton3.Checked)
            {
                BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["DBENGINE"] = DbEngine.PostGre;
            }

            DialogManager.Instance.Go<CustomInstallationSummaryDialog>();
        }

        void ConfigureOtherDatabaseConnection_Load(object sender, System.EventArgs e)
        {
            SonarQubeLogo.Image = BootstrapperResources.sonarqube_banner;
        }
    }
}
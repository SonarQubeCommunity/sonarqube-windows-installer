//----------------------------------------------------------------------------------------------
// <copyright file="ConfigureNewDatabaseInstanceDialog.cs" company="SonarSource SA and Microsoft Corporation">
// Copyright (c) SonarSource SA and Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
//----------------------------------------------------------------------------------------------

using SonarQubeBootstrapper.Helpers;
using SonarQubeBootstrapper.Resources;
using System.Drawing;

namespace Wix.SonarQube.Dialogs
{
    /// <summary>
    /// The standard Setup Type dialog
    /// </summary>
    public partial class ConfigureNewDatabaseInstanceDialog : BAForm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetupTypeDialog"/> class.
        /// </summary>
        public ConfigureNewDatabaseInstanceDialog()
        {
            InitializeComponent();

            this.Text = BootstrapperConstants.FormalProductName;
            this.next.Text = BootstrapperResources.NextButtonText;
            this.cancel.Text = BootstrapperResources.CancelButtonText;
            this.back.Text = BootstrapperResources.BackButtonText;

            this.label1.Text = BootstrapperResources.ConfigureNewDatabaseInstanceDialogTitle;
            this.label2.Text = BootstrapperResources.ConfigureNewDatabaseInstanceDialogInstallingSQLLabel;
            this.label3.Text = BootstrapperResources.ConfigureNewDatabaseInstanceDialogDescription;
            this.label4.Text = BootstrapperResources.ConfigureNewDatabaseInstanceDialogDatabaseNameLabel;
            this.label5.Text = BootstrapperResources.ConfigureNewDatabaseInstanceDialogDatabaseUserLabel;
            this.label6.Text = BootstrapperResources.ConfigureNewDatabaseInstanceDialogDatabasePasswordLabel;
            this.label7.Text = BootstrapperResources.ConfigureNewDatabaseInstanceDialogConfirmPasswordLabel;
            
            this.checkBox1.Text = BootstrapperResources.ConfigureNewDatabaseInstanceDialogWindowsAuthenticationLabel;

            this.cancel.Click += new System.EventHandler(base.cancel_Click);
        }

        void back_Click(object sender, System.EventArgs e)
        {
            DialogManager.Instance.Previous();
        }

        void next_Click(object sender, System.EventArgs e)
        {
            DialogManager.Instance.Go<SqlServerInstanceValidateDialog>();
        }

        void ConfigureNewDatabaseInstanceDialog_Load(object sender, System.EventArgs e)
        {
            SonarQubeLogo.Image = BootstrapperResources.sonarqube_banner;
        }
    }
}
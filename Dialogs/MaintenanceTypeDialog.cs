//----------------------------------------------------------------------------------------------
// <copyright file="MaintenanceTypeDialog.cs" company="SonarSource SA and Microsoft Corporation">
// Copyright (c) SonarSource SA and Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
//----------------------------------------------------------------------------------------------

using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using SonarQubeBootstrapper.Helpers;
using SonarQubeBootstrapper.Resources;
using System.Drawing;

namespace Wix.SonarQube.Dialogs
{
    /// <summary>
    /// The standard Maintenance Type dialog
    /// </summary>
    public partial class MaintenanceTypeDialog : BAForm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaintenanceTypeDialog"/> class.
        /// </summary>
        public MaintenanceTypeDialog()
        {
            InitializeComponent();

            this.cancel.Text = BootstrapperResources.CancelButtonText;
            this.Text = BootstrapperConstants.FormalProductName;
            this.label1.Text = BootstrapperResources.MaintenanceDialogTitle;
            this.label2.Text = BootstrapperResources.MaintenanceDialogDescription;
            this.remove.Text = BootstrapperResources.MaintenanceRemoveButtonText;
            this.label5.Text = BootstrapperResources.MaintenanceRemoveDescription;
            this.Icon = BootstrapperResources.setup;
            this.label3.Text = BootstrapperResources.MaintenanceDialogSonarBatchRunningError;

            if (SonarProcessHelper.IsSonarQubeBatchRunning())
            {
                this.CrossImage.Visible = true;
                this.label3.Visible = true;
            }
            else
            {
                this.CrossImage.Visible = false;
                this.label3.Visible = false;
            }

            this.cancel.Click += new System.EventHandler(base.cancel_Click);
        }

        void repair_Click(object sender, System.EventArgs e)
        {
            BootstrapperManager.Instance.LaunchAction = LaunchAction.Repair;

            DialogManager.Instance.Next();
        }

        void remove_Click(object sender, System.EventArgs e)
        {
            DialogManager.Instance.Next();
        }

        void MaintenanceTypeDialog_Load(object sender, System.EventArgs e)
        {
            banner.Image = BootstrapperResources.sonarqube_banner;
        }
    }
}
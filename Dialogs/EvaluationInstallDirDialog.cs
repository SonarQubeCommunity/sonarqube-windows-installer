//----------------------------------------------------------------------------------------------
// <copyright file="EvaluationInstallDirDialog.cs" company="SonarSource SA and Microsoft Corporation">
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
    public partial class EvaluationInstallDirDialog : BAForm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InstallDirDialog"/> class.
        /// </summary>
        public EvaluationInstallDirDialog()
        {
            InitializeComponent();
        }

        void EvaluationInstallDirDialog_Load(object sender, EventArgs e)
        {
            this.Text = BootstrapperConstants.FormalProductName;
            this.back.Text = BootstrapperResources.BackButtonText;
            this.next.Text = BootstrapperResources.NextButtonText;
            this.cancel.Text = BootstrapperResources.CancelButtonText;

            banner.Image = BootstrapperResources.sonarqube_banner;
            this.Icon = BootstrapperResources.setup;
            label1.Text = BootstrapperResources.InstallDirTitle;
            label2.Text = BootstrapperResources.InstallDirInstallationLocation;
            label3.Text = BootstrapperResources.InstallDirDescription;

            string currentInstallDir = BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["INSTALLDIR"];

            BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["INSTALLDIR"] = Environment.ExpandEnvironmentVariables(currentInstallDir);
            label4.Text = BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["INSTALLDIR"];
            label5.Text = BootstrapperResources.InstallDirHTTPPort;
            label6.Text = "9000";
            label7.Text = BootstrapperResources.InstallDirDatabaseType;
            label8.Text = string.Format(BootstrapperResources.InstallDirDatabaseTypeValue, 9092);
            label9.Text = BootstrapperResources.InstallDirServiceInstall;

            label10.Text = BootstrapperResources.InstallDirInstallServiceValue;
            label11.Text = BootstrapperResources.InstallDirInformationEvaluation;

            this.cancel.Click += new System.EventHandler(base.cancel_Click);
        }

        void back_Click(object sender, EventArgs e)
        {
            DialogManager.Instance.Previous();
        }

        void next_Click(object sender, EventArgs e)
        {
            DialogManager.Instance.Go<ProgressDialog>();
        }
    }
}
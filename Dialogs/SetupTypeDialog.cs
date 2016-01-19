//----------------------------------------------------------------------------------------------
// <copyright file="SetupTypeDialog.cs" company="SonarSource SA and Microsoft Corporation">
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
    public partial class SetupTypeDialog : BAForm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetupTypeDialog"/> class.
        /// </summary>
        public SetupTypeDialog()
        {
            InitializeComponent();

            this.Text = BootstrapperConstants.FormalProductName;
            this.next.Text = BootstrapperResources.NextButtonText;
            this.cancel.Text = BootstrapperResources.CancelButtonText;
            this.back.Text = BootstrapperResources.BackButtonText;
            this.Icon = BootstrapperResources.setup;

            this.cancel.Click += new System.EventHandler(base.cancel_Click);
        }

        void back_Click(object sender, System.EventArgs e)
        {
            DialogManager.Instance.Previous();
        }

        void next_Click(object sender, System.EventArgs e)
        {
            if (this.evaluationSetupRadioButton.Checked)
            {
                BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["SETUPTYPE"] = SetupType.Evaluation;
                BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["DBENGINE"] = DbEngine.H2;

                DialogManager.Instance.Go<EvaluationInstallDirDialog>();
            }
            else if (this.expressSetupRadioButton.Checked)
            {
                BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["SETUPTYPE"] = SetupType.Express;
                BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["DBENGINE"] = DbEngine.MsSql;

                DialogManager.Instance.Go<SqlInstallationDialog>();
            }
            else if (this.customSetupRadioButton.Checked)
            {
                BootstrapperManager.Instance.Bootstrapper.Engine.StringVariables["SETUPTYPE"] = SetupType.Custom;

                DialogManager.Instance.Go<ConfigureOtherDatabaseConnection>();
            }
        }

        void SetupTypeDialog_Load(object sender, System.EventArgs e)
        {
            SonarQubeLogo.Image = BootstrapperResources.sonarqube_banner;
        }
    }
}
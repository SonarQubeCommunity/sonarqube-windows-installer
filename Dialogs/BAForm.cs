//----------------------------------------------------------------------------------------------
// <copyright file="BAForm.cs" company="SonarSource SA and Microsoft Corporation">
// Copyright (c) SonarSource SA and Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
//----------------------------------------------------------------------------------------------

using SonarQubeBootstrapper.Helpers;
using SonarQubeBootstrapper.Resources;
using System;
using System.Windows.Forms;

namespace Wix.SonarQube.Dialogs
{
    public class BAForm : Form
    {
        public BAForm()
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.FormClosing += new FormClosingEventHandler(Form_FormClosing);
        }

        protected override void OnShown(EventArgs e)
        {
            // This is useful when we show first dialog. We record its location
            // that time.
            DialogManager.Instance.DialogLocation = this.Location;

            base.OnShown(e);
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);

            // Whenever dialog location changes, persist it in DialogManager.
            DialogManager.Instance.DialogLocation = this.Location;
        }

        protected virtual void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show(BootstrapperResources.CancelSetupInformationText, BootstrapperConstants.FormalProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    e.Cancel = false;
                    this.Dispose();
                    Application.Exit();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        protected virtual void cancel_Click(object sender, System.EventArgs e)
        {
            DialogResult result = MessageBox.Show(BootstrapperResources.CancelSetupInformationText, BootstrapperConstants.FormalProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                this.Dispose();
                Application.Exit();
            }
        }
    }
}

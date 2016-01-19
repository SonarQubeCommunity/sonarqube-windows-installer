//----------------------------------------------------------------------------------------------
// <copyright file="LicenceDialog.cs" company="SonarSource SA and Microsoft Corporation">
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
    /// The standard Licence dialog
    /// </summary>
    public partial class LicenceDialog : BAForm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LicenceDialog"/> class.
        /// </summary>
        public LicenceDialog()
        {
            InitializeComponent();
        }

        void LicenceDialog_Load(object sender, EventArgs e)
        {
            this.Text = BootstrapperConstants.FormalProductName;
            this.next.Text = BootstrapperResources.NextButtonText;
            this.cancel.Text = BootstrapperResources.CancelButtonText;
            this.back.Text = BootstrapperResources.BackButtonText;
            this.print.Text = BootstrapperResources.PrintButtonText;

            this.label1.Text = BootstrapperResources.EndUserLicenseTitle;
            this.label2.Text = BootstrapperResources.EndUserLicenseSubtitle;
            this.accepted.Text = BootstrapperResources.EndUserLicenseAcceptCheckbox;

            banner.Image = BootstrapperResources.sonarqube_banner;
            this.Icon = BootstrapperResources.setup;
            agreement.Rtf = BootstrapperResources.License;
            accepted.Checked = false;

            this.cancel.Click += new System.EventHandler(base.cancel_Click);
            this.printDocument1.PrintPage += PrintDocument1_PrintPage;
        }

        private void PrintDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            string text = BootstrapperResources.License;
            System.Drawing.Font printFont = new Font("Arial", 14, FontStyle.Regular);

            e.Graphics.DrawString(text, printFont, Brushes.Black, 10, 10);
        }

        void back_Click(object sender, EventArgs e)
        {
            DialogManager.Instance.Previous();
        }

        void next_Click(object sender, EventArgs e)
        {
            DialogManager.Instance.Next();
        }

        void accepted_CheckedChanged(object sender, EventArgs e)
        {
            next.Enabled = accepted.Checked;
        }

        void print_Click(object sender, EventArgs e)
        {
            try
            {
                printDialog1.AllowSomePages = true;
                printDialog1.ShowHelp = true;

                printDialog1.Document = this.printDocument1;

                DialogResult result = printDialog1.ShowDialog();

                if (result == DialogResult.OK)
                {
                    printDocument1.Print();
                }
            }
            catch { }
        }

        void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var data = new DataObject();

                if (agreement.SelectedText.Length > 0)
                {
                    data.SetData(DataFormats.UnicodeText, agreement.SelectedText);
                    data.SetData(DataFormats.Rtf, agreement.SelectedRtf);
                }
                else
                {
                    data.SetData(DataFormats.Rtf, agreement.Rtf);
                    data.SetData(DataFormats.Text, agreement.Text);
                }

                Clipboard.SetDataObject(data);
            }
            catch { }
        }
    }
}

//----------------------------------------------------------------------------------------------
// <copyright file="WelcomeDialog.cs" company="SonarSource SA and Microsoft Corporation">
// Copyright (c) SonarSource SA and Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
//----------------------------------------------------------------------------------------------

using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using SonarQubeBootstrapper.Helpers;
using SonarQubeBootstrapper.Resources;
using System;
using System.Windows.Forms;

namespace Wix.SonarQube.Dialogs
{
    /// <summary>
    /// Welcome dialog for SonarQube setup
    /// </summary>
    public partial class WelcomeDialog : BAForm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WelcomeDialog"/> class.
        /// </summary>
        public WelcomeDialog()
        {
            InitializeComponent();

            this.Text = BootstrapperConstants.FormalProductName;
            this.next.Text = BootstrapperResources.NextButtonText;
            this.cancel.Text = BootstrapperResources.CancelButtonText;
            this.Icon = BootstrapperResources.setup;

            this.label1.Text = string.Format(BootstrapperResources.WelcomeDialogTitle, BootstrapperConstants.FormalProductName);
            this.label2.Text = string.Format(BootstrapperResources.WelcomeDialogDescription, BootstrapperConstants.FormalProductName);
            this.label3.Text = BootstrapperResources.WelcomeDialogAction;
            this.label4.Text = BootstrapperResources.WelcomeDialogSonarQubeRunningWarning;
            this.JavaWebLink.Text = BootstrapperResources.WelcomeDialogJavaVersionError;
            this.JavaWebLink.LinkClicked += JavaWebLink_LinkClicked;
            this.RetryLink.LinkClicked += RetryLink_LinkClicked;
            this.cancel.Click += new System.EventHandler(base.cancel_Click);

            this.JavaWebLink.LinkArea = new System.Windows.Forms.LinkArea(95, 4);
            this.JavaWebLink.Links[0].LinkData = "https://www.java.com/en/download/manual.jsp";
        }

        void WelcomeDialog_Load(object sender, EventArgs e)
        {
            OnJavaVersionCheck();
            OnSonarQubeInstanceRunning();
        }        

        void next_Click(object sender, EventArgs e)
        {
            DialogManager.Instance.Next();
        }

        void back_Click(object sender, EventArgs e)
        {
            DialogManager.Instance.Previous();
        }

        private void JavaWebLink_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
            }
            catch (Exception ex)
            {
                BootstrapperManager.Instance.Bootstrapper.Engine.Log(LogLevel.Error, string.Format("{0}", ex.Message));
            }
        }

        private void RetryLink_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
            OnJavaVersionCheck();
        }

        private void OnJavaVersionCheck()
        {
            JavaVersionInformation javaVersionInformation = new JavaVersionInformation();

            try
            {
                javaVersionInformation = JavaVersionHelper.GetJavaVersionInformation();
            }
            catch (Exception e)
            {
                BootstrapperManager.Instance.Bootstrapper.Engine.Log(LogLevel.Error, string.Format("{0}", e.Message));
            }
            
            if (JavaVersionHelper.IsExpectedJavaVersionInstalled(javaVersionInformation, BootstrapperConstants.ExpectedJavaVersion))
            {
                this.next.Enabled = true;
                this.next.Invalidate();
                this.JavaWebLink.Hide();
                this.RetryLink.Hide();
                this.JavaCheckFailImage.Hide();
            }
            else
            {
                this.next.Enabled = false;
                this.next.Invalidate();
                this.JavaWebLink.Show();
                this.RetryLink.Show();
                this.JavaCheckFailImage.Show();
            }

            Application.DoEvents();
        }

        private void OnSonarQubeInstanceRunning()
        {
            if (SonarProcessHelper.IsSonarQubeRunning())
            {
                this.label4.Visible = true;
                this.JavaCheckFailImage.Visible = true;
                this.next.Enabled = false;
                this.RetryLink.Visible = false;
            }
        }
    }
}
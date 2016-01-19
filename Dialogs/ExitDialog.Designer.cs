//----------------------------------------------------------------------------------------------
// <copyright file="ExitDialog.Designer.cs" company="SonarSource SA and Microsoft Corporation">
// Copyright (c) SonarSource SA and Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
//----------------------------------------------------------------------------------------------

using SonarQubeBootstrapper.Resources;

namespace Wix.SonarQube.Dialogs
{
    partial class ExitDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExitDialog));
            this.description = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.serviceWebLink = new System.Windows.Forms.LinkLabel();
            this.viewSqlLog = new System.Windows.Forms.LinkLabel();
            this.viewLog = new System.Windows.Forms.LinkLabel();
            this.finish = new System.Windows.Forms.Button();
            this.SonarQubeLogo = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SonarQubeLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // description
            // 
            this.description.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.description.Location = new System.Drawing.Point(177, 84);
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(305, 72);
            this.description.TabIndex = 7;
            this.description.Text = "[ExitDialogDescription]";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(176, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(303, 61);
            this.label1.TabIndex = 6;
            this.label1.Text = "[ExitDialogTitle]";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.serviceWebLink);
            this.panel1.Controls.Add(this.viewSqlLog);
            this.panel1.Controls.Add(this.viewLog);
            this.panel1.Controls.Add(this.finish);
            this.panel1.Location = new System.Drawing.Point(-4, 308);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(503, 57);
            this.panel1.TabIndex = 5;
            // 
            // serviceWebLink
            // 
            this.serviceWebLink.Location = new System.Drawing.Point(183, 21);
            this.serviceWebLink.Name = "serviceWebLink";
            this.serviceWebLink.Size = new System.Drawing.Size(218, 18);
            this.serviceWebLink.TabIndex = 10;
            this.serviceWebLink.TabStop = true;
            this.serviceWebLink.Text = "WebLink";
            this.serviceWebLink.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.serviceWebLink.Visible = false;
            // 
            // viewSqlLog
            // 
            this.viewSqlLog.AutoSize = true;
            this.viewSqlLog.Location = new System.Drawing.Point(87, 21);
            this.viewSqlLog.Name = "viewSqlLog";
            this.viewSqlLog.Size = new System.Drawing.Size(81, 13);
            this.viewSqlLog.TabIndex = 2;
            this.viewSqlLog.TabStop = true;
            this.viewSqlLog.Text = "[View SQL Log]";
            this.viewSqlLog.Visible = false;
            this.viewSqlLog.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.viewSqlLog_LinkClicked);
            // 
            // viewLog
            // 
            this.viewLog.AutoSize = true;
            this.viewLog.Location = new System.Drawing.Point(16, 21);
            this.viewLog.Name = "viewLog";
            this.viewLog.Size = new System.Drawing.Size(54, 13);
            this.viewLog.TabIndex = 1;
            this.viewLog.TabStop = true;
            this.viewLog.Text = "[ViewLog]";
            this.viewLog.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.viewLog_LinkClicked);
            // 
            // finish
            // 
            this.finish.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.finish.Location = new System.Drawing.Point(407, 16);
            this.finish.Name = "finish";
            this.finish.Size = new System.Drawing.Size(75, 23);
            this.finish.TabIndex = 0;
            this.finish.Text = "[WixUIFinish]";
            this.finish.UseVisualStyleBackColor = true;
            this.finish.Click += new System.EventHandler(this.finish_Click);
            // 
            // SonarQubeLogo
            // 
            this.SonarQubeLogo.Image = ((System.Drawing.Image)(resources.GetObject("SonarQubeLogo.Image")));
            this.SonarQubeLogo.Location = new System.Drawing.Point(6, 9);
            this.SonarQubeLogo.Name = "SonarQubeLogo";
            this.SonarQubeLogo.Size = new System.Drawing.Size(171, 46);
            this.SonarQubeLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.SonarQubeLogo.TabIndex = 8;
            this.SonarQubeLogo.TabStop = false;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(177, 156);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(305, 72);
            this.label2.TabIndex = 9;
            this.label2.Text = "[ExitDialogDescription]";
            this.label2.Visible = false;
            // 
            // ExitDialog
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(494, 361);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SonarQubeLogo);
            this.Controls.Add(this.description);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Name = "ExitDialog";
            this.Text = "[WelcomeDlg_Title]";
            this.Load += new System.EventHandler(this.ExitDialog_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SonarQubeLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label description;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button finish;
        private System.Windows.Forms.LinkLabel viewLog;
        private System.Windows.Forms.PictureBox SonarQubeLogo;
        private System.Windows.Forms.LinkLabel viewSqlLog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel serviceWebLink;
    }
}
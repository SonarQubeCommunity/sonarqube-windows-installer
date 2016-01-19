//----------------------------------------------------------------------------------------------
// <copyright file="WelcomeDialog.Designer.cs" company="SonarSource SA and Microsoft Corporation">
// Copyright (c) SonarSource SA and Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
//----------------------------------------------------------------------------------------------

using System.Windows.Forms;

namespace Wix.SonarQube.Dialogs
{
    public partial class WelcomeDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WelcomeDialog));
            this.JavaCheckFailImage = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.next = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.SonarQubeLogo = new System.Windows.Forms.PictureBox();
            this.JavaWebLink = new System.Windows.Forms.LinkLabel();
            this.RetryLink = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.JavaCheckFailImage)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SonarQubeLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // JavaCheckFailImage
            // 
            this.JavaCheckFailImage.Image = ((System.Drawing.Image)(resources.GetObject("JavaCheckFailImage.Image")));
            this.JavaCheckFailImage.Location = new System.Drawing.Point(184, 158);
            this.JavaCheckFailImage.Name = "JavaCheckFailImage";
            this.JavaCheckFailImage.Size = new System.Drawing.Size(32, 32);
            this.JavaCheckFailImage.TabIndex = 5;
            this.JavaCheckFailImage.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(177, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(253, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Click Next to Continue, or Cancel to exit the Setup.";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(177, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(305, 40);
            this.label2.TabIndex = 3;
            this.label2.Text = "Dialog description text";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(176, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(303, 61);
            this.label1.TabIndex = 2;
            this.label1.Text = "Welcome to the SonarQube Server 5.2 Installation Wizard";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.next);
            this.panel1.Controls.Add(this.cancel);
            this.panel1.Location = new System.Drawing.Point(-4, 312);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(503, 57);
            this.panel1.TabIndex = 1;
            // 
            // next
            // 
            this.next.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.next.Location = new System.Drawing.Point(308, 12);
            this.next.Name = "next";
            this.next.Size = new System.Drawing.Size(75, 23);
            this.next.TabIndex = 0;
            this.next.Text = "Next";
            this.next.UseVisualStyleBackColor = true;
            this.next.Click += new System.EventHandler(this.next_Click);
            // 
            // cancel
            // 
            this.cancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancel.Location = new System.Drawing.Point(404, 12);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 1;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // SonarQubeLogo
            // 
            this.SonarQubeLogo.Image = ((System.Drawing.Image)(resources.GetObject("SonarQubeLogo.Image")));
            this.SonarQubeLogo.Location = new System.Drawing.Point(3, 17);
            this.SonarQubeLogo.Name = "SonarQubeLogo";
            this.SonarQubeLogo.Size = new System.Drawing.Size(171, 46);
            this.SonarQubeLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.SonarQubeLogo.TabIndex = 0;
            this.SonarQubeLogo.TabStop = false;
            // 
            // JavaWebLink
            // 
            this.JavaWebLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.JavaWebLink.Location = new System.Drawing.Point(224, 155);
            this.JavaWebLink.Name = "JavaWebLink";
            this.JavaWebLink.Size = new System.Drawing.Size(256, 45);
            this.JavaWebLink.TabIndex = 4;
            this.JavaWebLink.TabStop = true;
            this.JavaWebLink.Text = "JavaWebLink";
            this.JavaWebLink.UseCompatibleTextRendering = true;
            // 
            // RetryLink
            // 
            this.RetryLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RetryLink.LinkArea = new System.Windows.Forms.LinkArea(0, 9);
            this.RetryLink.Location = new System.Drawing.Point(177, 210);
            this.RetryLink.Name = "RetryLink";
            this.RetryLink.Size = new System.Drawing.Size(80, 45);
            this.RetryLink.TabIndex = 4;
            this.RetryLink.TabStop = true;
            this.RetryLink.Text = "Try Again";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(222, 155);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(240, 55);
            this.label4.TabIndex = 6;
            this.label4.Text = "label4";
            this.label4.Visible = false;
            // 
            // WelcomeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(494, 361);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.JavaCheckFailImage);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.SonarQubeLogo);
            this.Controls.Add(this.JavaWebLink);
            this.Controls.Add(this.RetryLink);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "WelcomeDialog";
            this.Text = "[WelcomeDlg_Title]";
            this.Load += new System.EventHandler(this.WelcomeDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.JavaCheckFailImage)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SonarQubeLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.PictureBox SonarQubeLogo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button next;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel JavaWebLink;
        private System.Windows.Forms.LinkLabel RetryLink;
        private PictureBox JavaCheckFailImage;
        private Label label4;
    }
}
﻿//----------------------------------------------------------------------------------------------
// <copyright file="SqlServerInstanceValidateDialog.Designer.cs" company="SonarSource SA and Microsoft Corporation">
// Copyright (c) SonarSource SA and Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
//----------------------------------------------------------------------------------------------

using SonarQubeBootstrapper.Resources;

namespace Wix.SonarQube.Dialogs
{
    partial class SqlServerInstanceValidateDialog
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
            this.banner = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.back = new System.Windows.Forms.Button();
            this.next = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.checkConnectionLink = new System.Windows.Forms.LinkLabel();
            this.userNameTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.authenticationTypeCheckBox = new System.Windows.Forms.CheckBox();
            this.connectionStatusImage = new System.Windows.Forms.PictureBox();
            this.sqlInstanceTextBox = new System.Windows.Forms.TextBox();
            this.sqlConnectionInfo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.banner)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.connectionStatusImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sqlConnectionInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // banner
            // 
            this.banner.BackColor = System.Drawing.Color.White;
            this.banner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.banner.Location = new System.Drawing.Point(0, 0);
            this.banner.Name = "banner";
            this.banner.Size = new System.Drawing.Size(501, 59);
            this.banner.TabIndex = 0;
            this.banner.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.back);
            this.panel1.Controls.Add(this.next);
            this.panel1.Controls.Add(this.cancel);
            this.panel1.Location = new System.Drawing.Point(-3, 308);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(503, 57);
            this.panel1.TabIndex = 19;
            // 
            // back
            // 
            this.back.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.back.Location = new System.Drawing.Point(227, 12);
            this.back.Name = "back";
            this.back.Size = new System.Drawing.Size(75, 23);
            this.back.TabIndex = 3;
            this.back.Text = "[WixUIBack]";
            this.back.UseVisualStyleBackColor = true;
            this.back.Click += new System.EventHandler(this.back_Click);
            // 
            // next
            // 
            this.next.Enabled = false;
            this.next.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.next.Location = new System.Drawing.Point(315, 12);
            this.next.Name = "next";
            this.next.Size = new System.Drawing.Size(75, 23);
            this.next.TabIndex = 1;
            this.next.Text = "[WixUINext]";
            this.next.UseVisualStyleBackColor = true;
            this.next.Click += new System.EventHandler(this.next_Click);
            // 
            // cancel
            // 
            this.cancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancel.Location = new System.Drawing.Point(404, 12);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 2;
            this.cancel.Text = "[WixUICancel]";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select SQL Server Instance";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(501, 59);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(15, 67);
            this.label2.MaximumSize = new System.Drawing.Size(440, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(405, 26);
            this.label2.TabIndex = 18;
            this.label2.Text = "Select SQL Server Instance for SonarQube Server. Select SQL Server Instance for S" +
    "onarQube Server.";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Location = new System.Drawing.Point(-5, -5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(503, 61);
            this.panel2.TabIndex = 20;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.White;
            this.label6.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(16, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(254, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Select SQL Server Instance for SonarQube Server. ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(16, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Select SQL Server Instance";
            // 
            // checkConnectionLink
            // 
            this.checkConnectionLink.AutoSize = true;
            this.checkConnectionLink.Location = new System.Drawing.Point(416, 116);
            this.checkConnectionLink.Name = "checkConnectionLink";
            this.checkConnectionLink.Size = new System.Drawing.Size(28, 13);
            this.checkConnectionLink.TabIndex = 23;
            this.checkConnectionLink.TabStop = true;
            this.checkConnectionLink.Text = "Test";
            // 
            // userNameTextBox
            // 
            this.userNameTextBox.Location = new System.Drawing.Point(178, 171);
            this.userNameTextBox.Name = "userNameTextBox";
            this.userNameTextBox.Size = new System.Drawing.Size(215, 20);
            this.userNameTextBox.TabIndex = 26;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(28, 172);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "User Name";
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(178, 197);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(215, 20);
            this.passwordTextBox.TabIndex = 28;
            this.passwordTextBox.UseSystemPasswordChar = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(28, 198);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 27;
            this.label7.Text = "Password";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(15, 237);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(108, 13);
            this.label8.TabIndex = 29;
            this.label8.Text = "Authentication Failed";
            // 
            // authenticationTypeCheckBox
            // 
            this.authenticationTypeCheckBox.AutoSize = true;
            this.authenticationTypeCheckBox.Location = new System.Drawing.Point(15, 142);
            this.authenticationTypeCheckBox.Name = "authenticationTypeCheckBox";
            this.authenticationTypeCheckBox.Size = new System.Drawing.Size(80, 17);
            this.authenticationTypeCheckBox.TabIndex = 30;
            this.authenticationTypeCheckBox.Text = "checkBox1";
            this.authenticationTypeCheckBox.UseVisualStyleBackColor = true;
            // 
            // connectionStatusImage
            // 
            this.connectionStatusImage.Location = new System.Drawing.Point(450, 107);
            this.connectionStatusImage.Name = "connectionStatusImage";
            this.connectionStatusImage.Size = new System.Drawing.Size(32, 32);
            this.connectionStatusImage.TabIndex = 31;
            this.connectionStatusImage.TabStop = false;
            // 
            // sqlInstanceTextBox
            // 
            this.sqlInstanceTextBox.Location = new System.Drawing.Point(178, 113);
            this.sqlInstanceTextBox.Name = "sqlInstanceTextBox";
            this.sqlInstanceTextBox.Size = new System.Drawing.Size(215, 20);
            this.sqlInstanceTextBox.TabIndex = 0;
            // 
            // sqlConnectionInfo
            // 
            this.sqlConnectionInfo.Location = new System.Drawing.Point(399, 116);
            this.sqlConnectionInfo.Name = "sqlConnectionInfo";
            this.sqlConnectionInfo.Size = new System.Drawing.Size(16, 16);
            this.sqlConnectionInfo.TabIndex = 33;
            this.sqlConnectionInfo.TabStop = false;
            // 
            // SqlServerInstanceValidateDialog
            // 
            this.ClientSize = new System.Drawing.Size(494, 361);
            this.Controls.Add(this.sqlConnectionInfo);
            this.Controls.Add(this.sqlInstanceTextBox);
            this.Controls.Add(this.connectionStatusImage);
            this.Controls.Add(this.authenticationTypeCheckBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.userNameTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.checkConnectionLink);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel2);
            this.Name = "SqlServerInstanceValidateDialog";
            this.Text = "[WelcomeDlg_Title]";
            this.Load += new System.EventHandler(this.SQLServerInstanceValidateDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.banner)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.connectionStatusImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sqlConnectionInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox banner;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button back;
        private System.Windows.Forms.Button next;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel checkConnectionLink;
        private System.Windows.Forms.TextBox userNameTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox authenticationTypeCheckBox;
        private System.Windows.Forms.PictureBox connectionStatusImage;
        private System.Windows.Forms.TextBox sqlInstanceTextBox;
        private System.Windows.Forms.PictureBox sqlConnectionInfo;
    }
}
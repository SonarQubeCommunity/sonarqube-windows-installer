//----------------------------------------------------------------------------------------------
// <copyright file="SetupTypeDialog.Designer.cs" company="SonarSource SA and Microsoft Corporation">
// Copyright (c) SonarSource SA and Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// </copyright>
//----------------------------------------------------------------------------------------------

using SonarQubeBootstrapper.Resources;

namespace Wix.SonarQube.Dialogs
{
    partial class SetupTypeDialog
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
            this.SonarQubeLogo = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cancel = new System.Windows.Forms.Button();
            this.next = new System.Windows.Forms.Button();
            this.back = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.evaluationSetupRadioButton = new System.Windows.Forms.RadioButton();
            this.expressSetupRadioButton = new System.Windows.Forms.RadioButton();
            this.customSetupRadioButton = new System.Windows.Forms.RadioButton();
            this.panel3 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.SonarQubeLogo)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // SonarQubeLogo
            // 
            this.SonarQubeLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SonarQubeLogo.BackColor = System.Drawing.Color.White;
            this.SonarQubeLogo.Location = new System.Drawing.Point(3, 0);
            this.SonarQubeLogo.Name = "SonarQubeLogo";
            this.SonarQubeLogo.Size = new System.Drawing.Size(498, 59);
            this.SonarQubeLogo.TabIndex = 0;
            this.SonarQubeLogo.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(270, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "Choose the appropriate installation option";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(27, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(173, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Please choose one of the following";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.SonarQubeLogo);
            this.panel2.Location = new System.Drawing.Point(-5, -5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(503, 61);
            this.panel2.TabIndex = 13;
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
            // next
            // 
            this.next.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.next.Location = new System.Drawing.Point(315, 12);
            this.next.Name = "next";
            this.next.Size = new System.Drawing.Size(75, 23);
            this.next.TabIndex = 1;
            this.next.Text = "[WixUINext]";
            this.next.UseVisualStyleBackColor = true;
            this.next.Click += new System.EventHandler(this.next_Click);
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
            this.panel1.TabIndex = 12;
            // 
            // evaluationSetupRadioButton
            // 
            this.evaluationSetupRadioButton.AutoSize = true;
            this.evaluationSetupRadioButton.Checked = true;
            this.evaluationSetupRadioButton.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.evaluationSetupRadioButton.Location = new System.Drawing.Point(3, 3);
            this.evaluationSetupRadioButton.Name = "evaluationSetupRadioButton";
            this.evaluationSetupRadioButton.Size = new System.Drawing.Size(346, 17);
            this.evaluationSetupRadioButton.TabIndex = 14;
            this.evaluationSetupRadioButton.TabStop = true;
            this.evaluationSetupRadioButton.Text = global::SonarQubeBootstrapper.Resources.BootstrapperResources.SetupTypeEvaluation;
            this.evaluationSetupRadioButton.UseVisualStyleBackColor = true;
            // 
            // expressSetupRadioButton
            // 
            this.expressSetupRadioButton.AutoSize = true;
            this.expressSetupRadioButton.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.expressSetupRadioButton.Location = new System.Drawing.Point(3, 38);
            this.expressSetupRadioButton.Name = "expressSetupRadioButton";
            this.expressSetupRadioButton.Size = new System.Drawing.Size(311, 17);
            this.expressSetupRadioButton.TabIndex = 15;
            this.expressSetupRadioButton.Text = global::SonarQubeBootstrapper.Resources.BootstrapperResources.SetupTypeExpress;
            this.expressSetupRadioButton.UseVisualStyleBackColor = true;
            // 
            // customSetupRadioButton
            // 
            this.customSetupRadioButton.AutoSize = true;
            this.customSetupRadioButton.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.customSetupRadioButton.Location = new System.Drawing.Point(3, 72);
            this.customSetupRadioButton.Name = "customSetupRadioButton";
            this.customSetupRadioButton.Size = new System.Drawing.Size(299, 17);
            this.customSetupRadioButton.TabIndex = 16;
            this.customSetupRadioButton.Text = global::SonarQubeBootstrapper.Resources.BootstrapperResources.SetupTypeCustom;
            this.customSetupRadioButton.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.evaluationSetupRadioButton);
            this.panel3.Controls.Add(this.customSetupRadioButton);
            this.panel3.Controls.Add(this.expressSetupRadioButton);
            this.panel3.Location = new System.Drawing.Point(30, 106);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(447, 151);
            this.panel3.TabIndex = 0;
            // 
            // SetupTypeDialog
            // 
            this.ClientSize = new System.Drawing.Size(494, 361);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "SetupTypeDialog";
            this.Text = "[WelcomeDlg_Title]";
            this.Load += new System.EventHandler(this.SetupTypeDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SonarQubeLogo)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox SonarQubeLogo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button next;
        private System.Windows.Forms.Button back;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton evaluationSetupRadioButton;
        private System.Windows.Forms.RadioButton expressSetupRadioButton;
        private System.Windows.Forms.RadioButton customSetupRadioButton;
        private System.Windows.Forms.Panel panel3;
    }
}
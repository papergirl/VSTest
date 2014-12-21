using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace KittyAltruistic.CPlusPlusTestRunner
{
    partial class GSettings
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
        [ContractVerification(false)]
        private void InitializeComponent()
        {
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdSet = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.chkDefaults = new System.Windows.Forms.CheckBox();
            this.dlgTestExe = new System.Windows.Forms.OpenFileDialog();
            this.cmdGetExe = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtListTests = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRunTests = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Location = new System.Drawing.Point(411, 175);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 9;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.CmdCancelClick);
            // 
            // cmdSet
            // 
            this.cmdSet.Location = new System.Drawing.Point(15, 175);
            this.cmdSet.Name = "cmdSet";
            this.cmdSet.Size = new System.Drawing.Size(75, 23);
            this.cmdSet.TabIndex = 8;
            this.cmdSet.Text = "OK";
            this.cmdSet.UseVisualStyleBackColor = true;
            this.cmdSet.Click += new System.EventHandler(this.CmdSetClick1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(327, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Set the executable which contains the tests (optional, uses projects)";
            // 
            // txtFile
            // 
            this.txtFile.Enabled = false;
            this.txtFile.Location = new System.Drawing.Point(18, 137);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(435, 20);
            this.txtFile.TabIndex = 6;
            // 
            // chkDefaults
            // 
            this.chkDefaults.AutoSize = true;
            this.chkDefaults.Checked = true;
            this.chkDefaults.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDefaults.Location = new System.Drawing.Point(12, 12);
            this.chkDefaults.Name = "chkDefaults";
            this.chkDefaults.Size = new System.Drawing.Size(87, 17);
            this.chkDefaults.TabIndex = 10;
            this.chkDefaults.Text = "Use Defaults";
            this.chkDefaults.UseVisualStyleBackColor = true;
            this.chkDefaults.CheckedChanged += new System.EventHandler(this.DefaultsChanged);
            // 
            // dlgTestExe
            // 
            this.dlgTestExe.FileName = "openFileDialog1";
            // 
            // cmdGetExe
            // 
            this.cmdGetExe.Enabled = false;
            this.cmdGetExe.Location = new System.Drawing.Point(459, 134);
            this.cmdGetExe.Name = "cmdGetExe";
            this.cmdGetExe.Size = new System.Drawing.Size(27, 23);
            this.cmdGetExe.TabIndex = 5;
            this.cmdGetExe.Text = "...";
            this.cmdGetExe.UseVisualStyleBackColor = true;
            this.cmdGetExe.Click += new System.EventHandler(this.CmdGetExeClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "List test command line";
            // 
            // txtListTests
            // 
            this.txtListTests.Enabled = false;
            this.txtListTests.Location = new System.Drawing.Point(18, 59);
            this.txtListTests.Name = "txtListTests";
            this.txtListTests.Size = new System.Drawing.Size(435, 20);
            this.txtListTests.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Run tests command line";
            // 
            // txtRunTests
            // 
            this.txtRunTests.Enabled = false;
            this.txtRunTests.Location = new System.Drawing.Point(18, 98);
            this.txtRunTests.Name = "txtRunTests";
            this.txtRunTests.Size = new System.Drawing.Size(435, 20);
            this.txtRunTests.TabIndex = 13;
            // 
            // GSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 210);
            this.ControlBox = false;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtRunTests);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtListTests);
            this.Controls.Add(this.chkDefaults);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdSet);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFile);
            this.Controls.Add(this.cmdGetExe);
            this.Name = "GSettings";
            this.ShowInTaskbar = false;
            this.Text = "C++ Test Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdSet;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.CheckBox chkDefaults;
        private System.Windows.Forms.OpenFileDialog dlgTestExe;
        private System.Windows.Forms.Button cmdGetExe;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtListTests;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRunTests;
    }
}
namespace KittyAltruistic.CPlusPlusTestRunner
{
    partial class TestFailureCtrl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.lblTestName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblAssertNum = new System.Windows.Forms.Label();
            this.txtFailures = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Test:";
            // 
            // lblTestName
            // 
            this.lblTestName.AutoSize = true;
            this.lblTestName.Location = new System.Drawing.Point(49, 17);
            this.lblTestName.Name = "lblTestName";
            this.lblTestName.Size = new System.Drawing.Size(0, 13);
            this.lblTestName.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(125, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Number of failed assertion";
            // 
            // lblAssertNum
            // 
            this.lblAssertNum.AutoSize = true;
            this.lblAssertNum.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblAssertNum.Location = new System.Drawing.Point(260, 17);
            this.lblAssertNum.Name = "lblAssertNum";
            this.lblAssertNum.Size = new System.Drawing.Size(13, 13);
            this.lblAssertNum.TabIndex = 7;
            this.lblAssertNum.Text = "0";
            // 
            // txtFailures
            // 
            this.txtFailures.Location = new System.Drawing.Point(3, 37);
            this.txtFailures.Name = "txtFailures";
            this.txtFailures.Size = new System.Drawing.Size(280, 258);
            this.txtFailures.TabIndex = 8;
            this.txtFailures.Text = "";
            // 
            // TestFailureCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtFailures);
            this.Controls.Add(this.lblAssertNum);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblTestName);
            this.Controls.Add(this.label1);
            this.Name = "TestFailureCtrl";
            this.Size = new System.Drawing.Size(287, 295);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTestName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblAssertNum;
        private System.Windows.Forms.RichTextBox txtFailures;

    }
}

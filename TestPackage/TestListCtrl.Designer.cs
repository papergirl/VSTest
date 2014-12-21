using System.Diagnostics.Contracts;

namespace KittyAltruistic.CPlusPlusTestRunner
{
    partial class TestListCtrl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose( disposing );
        }


        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        [ContractVerification(false)]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestListCtrl));
            this.GridTests = new System.Windows.Forms.DataGridView();
            this.runCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.TestNameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Failures = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Passed = new System.Windows.Forms.DataGridViewImageColumn();
            this.TaskListMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.runTestsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugTestsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllTestsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deselectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cboFilter = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.picPass = new System.Windows.Forms.PictureBox();
            this.cboProject = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lblProject = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTestDetails = new System.Windows.Forms.Label();
            this.lblPassingTests = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblFailingTests = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTestNum = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.GridTests)).BeginInit();
            this.TaskListMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPass)).BeginInit();
            this.SuspendLayout();
            // 
            // GridTests
            // 
            this.GridTests.AllowUserToAddRows = false;
            this.GridTests.AllowUserToDeleteRows = false;
            this.GridTests.AllowUserToOrderColumns = true;
            this.GridTests.AllowUserToResizeRows = false;
            this.GridTests.BackgroundColor = System.Drawing.SystemColors.Window;
            this.GridTests.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.GridTests.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.GridTests.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridTests.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.runCol,
            this.TestNameCol,
            this.Failures,
            this.TimeCol,
            this.Passed});
            this.GridTests.ContextMenuStrip = this.TaskListMenu;
            this.GridTests.GridColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.GridTests, "GridTests");
            this.GridTests.Name = "GridTests";
            this.GridTests.RowHeadersVisible = false;
            this.GridTests.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.GridTests.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GridTests.ShowCellToolTips = false;
            this.GridTests.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridTests_ColumnHeaderMouseClick);
            // 
            // runCol
            // 
            this.runCol.DataPropertyName = "TestRan";
            this.runCol.FalseValue = "false";
            resources.ApplyResources(this.runCol, "runCol");
            this.runCol.Name = "runCol";
            this.runCol.TrueValue = "true";
            // 
            // TestNameCol
            // 
            this.TestNameCol.DataPropertyName = "Name";
            resources.ApplyResources(this.TestNameCol, "TestNameCol");
            this.TestNameCol.Name = "TestNameCol";
            this.TestNameCol.ReadOnly = true;
            this.TestNameCol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.TestNameCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Failures
            // 
            this.Failures.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Failures.DataPropertyName = "Errors";
            resources.ApplyResources(this.Failures, "Failures");
            this.Failures.Name = "Failures";
            this.Failures.ReadOnly = true;
            this.Failures.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Failures.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TimeCol
            // 
            this.TimeCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.TimeCol.DataPropertyName = "Time";
            resources.ApplyResources(this.TimeCol, "TimeCol");
            this.TimeCol.Name = "TimeCol";
            this.TimeCol.ReadOnly = true;
            // 
            // Passed
            // 
            this.Passed.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Passed.DataPropertyName = "GetRelatedImage";
            resources.ApplyResources(this.Passed, "Passed");
            this.Passed.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.Passed.Name = "Passed";
            this.Passed.ReadOnly = true;
            this.Passed.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // TaskListMenu
            // 
            this.TaskListMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runTestsToolStripMenuItem,
            this.debugTestsToolStripMenuItem,
            this.selectAllTestsToolStripMenuItem,
            this.deselectAllToolStripMenuItem});
            this.TaskListMenu.Name = "TaskListMenu";
            resources.ApplyResources(this.TaskListMenu, "TaskListMenu");
            // 
            // runTestsToolStripMenuItem
            // 
            this.runTestsToolStripMenuItem.Name = "runTestsToolStripMenuItem";
            resources.ApplyResources(this.runTestsToolStripMenuItem, "runTestsToolStripMenuItem");
            this.runTestsToolStripMenuItem.Click += new System.EventHandler(this.RunTestsToolStripMenuItem_Click);
            // 
            // debugTestsToolStripMenuItem
            // 
            this.debugTestsToolStripMenuItem.Name = "debugTestsToolStripMenuItem";
            resources.ApplyResources(this.debugTestsToolStripMenuItem, "debugTestsToolStripMenuItem");
            this.debugTestsToolStripMenuItem.Click += new System.EventHandler(this.RunTestsToolStripMenuItem_Click);
            // 
            // selectAllTestsToolStripMenuItem
            // 
            this.selectAllTestsToolStripMenuItem.Name = "selectAllTestsToolStripMenuItem";
            resources.ApplyResources(this.selectAllTestsToolStripMenuItem, "selectAllTestsToolStripMenuItem");
            this.selectAllTestsToolStripMenuItem.Click += new System.EventHandler(this.SelectAllTestsToolStripMenuItem_Click);
            // 
            // deselectAllToolStripMenuItem
            // 
            this.deselectAllToolStripMenuItem.Name = "deselectAllToolStripMenuItem";
            resources.ApplyResources(this.deselectAllToolStripMenuItem, "deselectAllToolStripMenuItem");
            this.deselectAllToolStripMenuItem.Click += new System.EventHandler(this.DeselectAllToolStripMenuItem_Click);
            // 
            // cboFilter
            // 
            this.cboFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFilter.FormattingEnabled = true;
            this.cboFilter.Items.AddRange(new object[] {
            resources.GetString("cboFilter.Items")});
            resources.ApplyResources(this.cboFilter, "cboFilter");
            this.cboFilter.Name = "cboFilter";
            this.cboFilter.SelectedIndexChanged += new System.EventHandler(this.CboFilter_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // picPass
            // 
            resources.ApplyResources(this.picPass, "picPass");
            this.picPass.Name = "picPass";
            this.picPass.TabStop = false;
            // 
            // cboProject
            // 
            this.cboProject.FormattingEnabled = true;
            resources.ApplyResources(this.cboProject, "cboProject");
            this.cboProject.Name = "cboProject";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // lblProject
            // 
            resources.ApplyResources(this.lblProject, "lblProject");
            this.lblProject.Name = "lblProject";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // lblTestDetails
            // 
            resources.ApplyResources(this.lblTestDetails, "lblTestDetails");
            this.lblTestDetails.Name = "lblTestDetails";
            // 
            // lblPassingTests
            // 
            resources.ApplyResources(this.lblPassingTests, "lblPassingTests");
            this.lblPassingTests.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lblPassingTests.Name = "lblPassingTests";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // lblFailingTests
            // 
            resources.ApplyResources(this.lblFailingTests, "lblFailingTests");
            this.lblFailingTests.ForeColor = System.Drawing.Color.Red;
            this.lblFailingTests.Name = "lblFailingTests";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // lblTestNum
            // 
            resources.ApplyResources(this.lblTestNum, "lblTestNum");
            this.lblTestNum.Name = "lblTestNum";
            // 
            // lblTime
            // 
            resources.ApplyResources(this.lblTime, "lblTime");
            this.lblTime.Name = "lblTime";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Name";
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Errors";
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Time";
            resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // TestListCtrl
            // 
            resources.ApplyResources(this, "$this");
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.lblProject);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cboProject);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblTestNum);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.GridTests);
            this.Controls.Add(this.lblFailingTests);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblPassingTests);
            this.Controls.Add(this.lblTestDetails);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.picPass);
            this.Controls.Add(this.cboFilter);
            this.Controls.Add(this.label1);
            this.Name = "TestListCtrl";
            ((System.ComponentModel.ISupportInitialize)(this.GridTests)).EndInit();
            this.TaskListMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picPass)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private System.Windows.Forms.DataGridView GridTests;
        private System.Windows.Forms.ContextMenuStrip TaskListMenu;
        private System.Windows.Forms.ToolStripMenuItem runTestsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectAllTestsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deselectAllToolStripMenuItem;
        private System.Windows.Forms.ComboBox cboFilter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox picPass;
        private System.Windows.Forms.ComboBox cboProject;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblProject;
        private System.Windows.Forms.ToolStripMenuItem debugTestsToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTestDetails;
        private System.Windows.Forms.Label lblPassingTests;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblFailingTests;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblTestNum;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn runCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn TestNameCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn Failures;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeCol;
        private System.Windows.Forms.DataGridViewImageColumn Passed;

    }
}

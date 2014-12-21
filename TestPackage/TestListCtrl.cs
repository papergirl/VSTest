using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Windows.Forms;

namespace KittyAltruistic.CPlusPlusTestRunner
{
    /// <summary>
    /// Displays a list of tests with their results
    /// from an executable.
    /// </summary>
    public partial class TestListCtrl : UserControl
    {
        private const int GridTestMargin = 25;
        private delegate void UpdateSuitePicture(Image image);
        private readonly GTestRunner _gTestRunner = new GTestRunner();
        private delegate void UpdateFilter(string filter);

        private delegate void SetGridDatasouce(object data);

        private void SetDatasource(object data)
        {
            GridTests.DataSource = data;
        }

        private void AddFilterItem(string s)
        {
            cboFilter.Items.Add(s);
        }

        public TestListCtrl()
        {
            InitializeComponent();
            GridTests.CellDoubleClick += GridTests_CellDoubleClick;
            GridTests.RowPrePaint += GridTests_RowPrePaint;
            _gTestRunner.OnTestsUpdated += updateGTestInfomation;
            GridTests.AutoGenerateColumns = false;
            GridTests.Columns[1].DataPropertyName = "Name";
            GridTests.Columns[2].DataPropertyName = "FailuresSingleLine";
            GridTests.Columns[3].DataPropertyName = "Time";
            cboProject.DisplayMember = "Name";
            cboProject.ValueMember = "TestExe";
        }

        #region "GridCallBacks"

        private void GridTests_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            DataGridViewRow row = GridTests.Rows[e.RowIndex];
            GTestResult result = row.DataBoundItem as GTestResult;

            if (result == null || result.Disabled != 1 || result.Errors.Count <= 0) return;
            foreach (DataGridViewCell c in row.Cells)
            {
                if (result.Disabled == 1)
                {
                    c.Style.ForeColor = Color.Gray;
                    if (string.IsNullOrEmpty(c.ToolTipText))
                        c.ToolTipText = "Disabled";
                }
                else if (result.Errors.Count > 0)
                    c.Style.ForeColor = Color.Red;
            }
        }

        private void GridTests_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                DataGridViewRow row = GridTests.Rows[e.RowIndex];
                if (row.Cells[2].Value == null || (string)row.Cells[2].Value == "")
                    return;
                GTestResult test = (GTestResult)row.DataBoundItem;
                TestFailure failure = TestPackage.GetTestFailureWindow();
                failure.ShowTest(test.Name, test.Failures);
            }
        }

        #endregion "GridCallBacks"

        /// <summary>
        /// Let this control process the mnemonics.
        /// </summary>
        [UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
        protected override bool ProcessDialogChar(char charCode)
        {
            // If we're the top-level form or control, we need to do the mnemonic handling
            if (charCode != ' ' && ProcessMnemonic(charCode))
            {
                return true;
            }
            return base.ProcessDialogChar(charCode);
        }

        /// <summary>
        /// Enable the IME status handling for this control.
        /// </summary>
        protected override bool CanEnableIme
        {
            get
            {
                return true;
            }
        }

        #region "ThreadSafeGUIUpdateProp"

        private int NumberOfTests
        {
            get { return Convert.ToInt32(lblTestNum.Text); }
            set { lblTestNum.SetPropertyThreadSafe(() => lblTestNum.Text, value.ToString()); }
        }

        private int NumberOfFailingTests
        {
            get { return Convert.ToInt32(lblFailingTests.Text); }
            set { lblFailingTests.SetPropertyThreadSafe(() => lblFailingTests.Text, value.ToString()); }
        }

        protected int NumberOfPassingTests
        {
            get { return Convert.ToInt32(lblPassingTests.Text); }
            set { lblPassingTests.SetPropertyThreadSafe(() => lblPassingTests.Text, value.ToString()); }
        }

        private float TimeTakenForTests
        {
            set { lblTime.SetPropertyThreadSafe(() => lblTime.Text, value.ToString()); }
        }

        #endregion "ThreadSafeGUIUpdateProp"

        /// <summary>
        /// Selects all the tests in the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectAllTestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in GridTests.Rows)
                row.Cells[0].Value = "true";
        }

        /// <summary>
        /// Deselects all the tests in the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeselectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in GridTests.Rows)
                row.Cells[0].Value = "false";
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunTestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cboProject.Items.Count < 1 || GridTests.Rows.Count < 1)
            {
                MessageBox.Show(Resources.NoTests);
                return;
            }
            string filterString = GetGTestFilter();
            ConfiguredProject test = (ConfiguredProject)(cboProject.Items.Count == 1 ? cboProject.Items[0] : cboProject.SelectedItem);
            ToolStripMenuItem selectedItem = (ToolStripMenuItem)sender;
            try
            {
                if (selectedItem.Text == Resources.ctxTestListMenu_Run)
                    _gTestRunner.RunTests(test, filterString, false);
                else
                    _gTestRunner.RunTests(test, filterString, true);
            }
            catch (TestRunnerException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public string GetGTestFilter()
        {
            string filterString = "";
            if (GridTests.Rows.Count != 0)
            {
                foreach (DataGridViewRow row in GridTests.Rows)
                {
                    GTestResult test = (GTestResult)row.DataBoundItem;
                    if (test.TestRan)
                        if (filterString == "")
                            filterString = test.Fullname;
                        else
                            filterString += "*" + test.Fullname;
                }
            }
            return filterString;
        }

        public void AddProject(ConfiguredProject project)
        {
            Contract.Requires(project != null);
            if (!cboProject.Items.Contains(project))
                cboProject.Items.Add(project);
            if (_gTestRunner.TestsInRunner.Count > 1)
            {
                cboProject.SetPropertyThreadSafe(() => cboProject.Visible, true);
                lblProject.SetPropertyThreadSafe(() => lblProject.Visible, false);
            }
            else
            {
                lblProject.SetPropertyThreadSafe(() => lblProject.Text, project.Name);
                lblProject.SetPropertyThreadSafe(() => lblProject.Visible, true);
            }
        }

        public void ListTests(ConfiguredProject configuredProject)
        {
            Contract.Requires(configuredProject != null);
            _gTestRunner.ListTests(configuredProject);
        }

        /// <summary>
        /// Updates the Ctrl's GUI with the test infomation inside testInfo
        /// </summary>
        public void updateGTestInfomation(ConfiguredProject project, GTestResultCollection testInfo)
        {
            Contract.Requires(testInfo != null);

            UpdateSuitePicture updateSuitePicture = SuitePictureChange;
            Image image;
            AddProject(project);
            NumberOfTests = testInfo.TotalNumberOfTests;
            NumberOfFailingTests = testInfo.TotalNumberOfFailures;
            NumberOfPassingTests = NumberOfTests - NumberOfFailingTests;
            if (NumberOfPassingTests == 0 && NumberOfFailingTests == 0)
                image = Resources.TestNotRun;
            else
                image = NumberOfFailingTests == 0 ? Resources.TestPassed : Resources.TestFail;

            cboFilter.SetPropertyThreadSafe(() => cboFilter.SelectedIndex, 0);
            TimeTakenForTests = testInfo.TotalTime;
            if (picPass.InvokeRequired)
                picPass.Invoke(updateSuitePicture, new object[] { image });
            else
                updateSuitePicture(image);
            BindingSource bindingSource = new BindingSource();
            UpdateFilter addFilterItem = AddFilterItem;
            foreach (GTestSuite testSuite in testInfo.GetList())
            {
                if (cboFilter.InvokeRequired)
                    cboFilter.Invoke(addFilterItem, new object[] { testSuite.Name });
                else
                    cboFilter.Items.Add(testSuite.Name);
                foreach (GTestResult test in testSuite.GetList())
                    bindingSource.Add(test);
            }
            SetGridDatasouce datasouce = SetDatasource;
            if (GridTests.InvokeRequired)
                GridTests.Invoke(datasouce, new object[] { bindingSource });
            else
                GridTests.DataSource = bindingSource;

        }

        public void ClearTests()
        {
            BindingSource binding = new BindingSource();
            binding.AddNew();

            SetGridDatasouce datasouce = SetDatasource;
            if (GridTests.InvokeRequired)
                GridTests.Invoke(datasouce, new object[] { binding });
            else
                GridTests.DataSource = binding;
            cboFilter.Items.Clear();
            cboFilter.Items.Add(Resources.FilterByAllTestsText);
            cboFilter.SelectedIndex = 0;
            lblTime.Text = "";
            lblTestNum.Text = "";
            lblTestDetails.Text = "";
            lblProject.Text = "";
            lblPassingTests.Text = "";
            lblFailingTests.Text = "";
            //check if a failurewindow exists and if it does clear it.
            TestFailure failureWindow = TestPackage.GetTestFailureWindow();
            if (failureWindow != null)
                failureWindow.Clear();

        }

        private void SuitePictureChange(Image image)
        {
            picPass.Image = image;
        }

        private void CboFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_gTestRunner.TestsInRunner.Count == 0)
                return;

            GTestResultCollection resultCollection = _gTestRunner.TestsInRunner.Count == 1 ? _gTestRunner.TestsInRunner.First().Value : _gTestRunner.TestsInRunner[((ConfiguredProject)cboProject.SelectedItem).TestExe];
            BindingSource boundTests = new BindingSource();

            if (cboFilter.Text == Resources.FilterByAllTestsText)
            {
                TimeTakenForTests = resultCollection.TotalTime;
                NumberOfFailingTests = resultCollection.TotalNumberOfFailures;
                NumberOfPassingTests = resultCollection.TotalNumberOfTests - resultCollection.TotalNumberOfFailures;
                NumberOfTests = resultCollection.TotalNumberOfTests;
                foreach (GTestSuite suite in resultCollection.TestResults)
                    foreach (var test in suite.Results)
                        boundTests.Add(test);
                if (NumberOfPassingTests == 0 && NumberOfFailingTests == 0)
                    picPass.Image = Resources.TestNotRun;
                else
                    picPass.Image = NumberOfFailingTests == 0 ? Resources.TestPassed : Resources.TestFail;
            }
            else
            {
                foreach (GTestSuite suite in resultCollection.TestResults)
                {
                    if (suite.Name == cboFilter.Text)
                    {
                        TimeTakenForTests = suite.Time;
                        NumberOfFailingTests = suite.NumberOfFailures;
                        NumberOfPassingTests = suite.NumberOfTests - suite.NumberOfFailures;
                        NumberOfTests = suite.NumberOfTests;
                        if (NumberOfPassingTests == 0 && NumberOfFailingTests == 0)
                            picPass.Image = Resources.TestNotRun;
                        else
                            picPass.Image = NumberOfFailingTests == 0 ? Resources.TestPassed : Resources.TestFail;
                        foreach (var test in suite.Results)
                            boundTests.Add(test);
                        break;

                    }
                }
            }
            GridTests.DataSource = boundTests;
        }

        private void GridTests_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (0 == e.ColumnIndex)
            {
                bool setToTrue = false;
                foreach (DataGridViewRow row in GridTests.Rows)
                {
                    if ("true" != row.Cells[0].Value.ToString().ToLower())
                        setToTrue = true;
                }
                foreach (DataGridViewRow row in GridTests.Rows)
                    row.Cells[0].Value = setToTrue.ToString().ToLower();

            }
        }


   
    }
}

using System;
using System.Linq;
using System.Windows.Forms;

namespace KittyAltruistic.CPlusPlusTestRunner
{
    public partial class TestFailureCtrl : UserControl
    {
        public TestFailureCtrl()
        {
            InitializeComponent();
            txtFailures.Width = this.Width -5;
            txtFailures.Height = this.Height - 5;
            lblAssertNum.Width = this.Width - 5;
            label2.Width = this.Width - label2.Text.Length - 10;
            
            this.Resize += TestFailureCtrl_Resize;
        }

        void TestFailureCtrl_Resize(object sender, EventArgs e)
        {
            txtFailures.Width = this.Width - 5;
            txtFailures.Height = this.Height - 5;
            label2.Width = this.Width - label2.Text.Length - 10;    
        }



        public void ShowTest(string testName, string testErrors)
        {
            lblTestName.Text = testName;
            string rtf = @"{\rtf1\ansi{\fonttbl\f0\fswiss Helvetica;}\f0\pard " +
                         testErrors.Replace("\n", @"\par \pard ")
                             .Replace("Expected", @"{\b Expected}")
                             .Replace("Actual", @"{\b Actual}")
                             .Replace("Value of",@"{\b Value of}")
                         + @"\par }";

            txtFailures.Rtf = rtf;
            int numberOfAssertions = txtFailures.Lines.Count(line => line.Contains("Value of"));
            lblAssertNum.Text = numberOfAssertions.ToString();
        }
        public void Clear()
        {
            txtFailures.Rtf = "";
        }

    }
}

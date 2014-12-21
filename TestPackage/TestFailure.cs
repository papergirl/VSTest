using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace KittyAltruistic.CPlusPlusTestRunner
{
    [Guid(GuidList.GUIDTestFailureID)]
    public class TestFailure : ToolWindowPane
    {
        // This is the user control hosted by the tool window; it is exposed to the base class 
        // using the Window property. Note that, even if this class implements IDispose, we are
        // not calling Dispose on this object. This is because ToolWindowPane calls Dispose on 
        // the object returned by the Window property.
        private readonly TestFailureCtrl Control;

        /// <summary>
        /// Standard constructor for the tool window.
        /// </summary>
        public TestFailure() :
            base(null)
        {
            // Set the window title reading it from the resources.
            this.Caption = "Test Errors";
            // Set the image that will appear on the tab of the window frame
            // when docked with an other window
            // The resource ID correspond to the one defined in the resx file
            // while the Index is the offset in the bitmap strip. Each image in
            // the strip being 16x16.
            this.BitmapResourceID = 301;
            this.BitmapIndex = 1;

            Control = new TestFailureCtrl {Dock = DockStyle.Fill};
        }

     

        /// <summary>
        /// This property returns the handle to the user control that should
        /// be hosted in the Tool Window.
        /// </summary>
        override public IWin32Window Window
        {
            get
            {
                return Control;
            }
        }
        public void ShowTest(string testName,string testErrors)
        {
            Control.ShowTest(testName, testErrors);
            IVsWindowFrame windowFrame = (IVsWindowFrame) this.Frame;
            ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }

        public void Clear()
        {
            Control.Clear();
        }
    }
}

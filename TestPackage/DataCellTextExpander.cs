using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KittyAltruistic.CPlusPlusTestRunner
{
    class DataCellTextExpander : DataGridViewTextBoxCell
    {
        public override Type EditType
        {
            get
            {
                // Return null since no editing control is used for the editing experience.
                return null;
            }
        }
    }
}

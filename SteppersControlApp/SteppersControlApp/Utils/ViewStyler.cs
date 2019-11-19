using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace SteppersControlApp.Utils
{
    class ViewStyler
    {
        public static void styleGrid(DataGridView view)
        {
            view.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            view.DefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            view.RowHeadersVisible = false;
            view.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            view.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            view.AllowUserToAddRows = false;
            view.AllowUserToDeleteRows = false;
            view.AllowUserToResizeRows = false;
            view.AllowUserToOrderColumns = false;
        }
    }
}


using System.Windows.Forms;

namespace PresentationWinForms.Utils
{
    class DataGridViewStyler
    {
        public static void StyleGrid(DataGridView view)
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

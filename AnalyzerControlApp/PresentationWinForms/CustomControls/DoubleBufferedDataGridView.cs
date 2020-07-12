
using System.Windows.Forms;

namespace PresentationWinForms.CustomControls
{
    public class DoubleBufferedDataGridView : DataGridView
    {
        public DoubleBufferedDataGridView() : base()
        {
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
        }
    }
}

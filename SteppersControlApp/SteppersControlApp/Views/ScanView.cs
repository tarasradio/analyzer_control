using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SteppersControlCore;

namespace SteppersControlApp.Views
{
    public partial class ScanView : UserControl
    {
        string[] _columnHeaders = { "#", "Название", "Бар код" };

        Timer _updateTimer = new Timer();

        private System.Threading.Mutex _mutex;

        private String _barCode = null;

        public ScanView()
        {
            InitializeComponent();

            drawGrid();

            _mutex = new System.Threading.Mutex();

            _updateTimer.Interval = 100;
            _updateTimer.Tick += updateValues;
        }

        private void updateValues(object sender, EventArgs e)
        {

        }

        public void StartUpdate()
        {
            _updateTimer.Start();
        }

        public void StopUpdate()
        {
            _updateTimer.Stop();
        }

        private void drawGrid()
        {
            ViewStyler.styleGrid(tubesList);

            tubesList.DefaultCellStyle.SelectionBackColor = Color.White;
            tubesList.DefaultCellStyle.SelectionForeColor = Color.Black;
            tubesList.MultiSelect = false;
            tubesList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            tubesList.ColumnCount = _columnHeaders.Length;

            for (int i = 0; i < _columnHeaders.Length; i++)
                tubesList.Columns[i].HeaderText = _columnHeaders[i];

            tubesList.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            tubesList.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tubesList.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            tubesList.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            tubesList.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            tubesList.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tubesList.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void fillGrid()
        {
            tubesList.RowCount = 54;

            for (int i = 0; i < Core.Settings.Sensors.Count; i++)
            {
                tubesList[0, i].Value = i + 1;
                tubesList[1, i].Value = $"Пробирка № {i + 1}";
                tubesList[2, i].Value = "Не задано";
            }
        }

        public void UpdateInformation()
        {
            fillGrid();
        }
    }
}

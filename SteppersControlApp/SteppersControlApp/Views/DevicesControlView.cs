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
    public partial class DevicesControlView : UserControl
    {
        string[] _columnHeaders = { "#", "Название"};
        
        Timer updateTimer = new Timer();

        private System.Threading.Mutex mutex; // Заменить на lock

        public DevicesControlView()
        {
            InitializeComponent();

            drawGrid();

            devicesList.RowCount = 10;

            mutex = new System.Threading.Mutex();

            updateTimer.Interval = 100;
            updateTimer.Tick += UpdateTimer_Tick;
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {

        }

        public void StartUpdate()
        {
            updateTimer.Start();
        }

        public void StopUpdate()
        {
            updateTimer.Stop();
        }

        private void drawGrid()
        {
            DataGridViewButtonColumn column = new DataGridViewButtonColumn();

            column.Name = "change_state_column";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            column.FlatStyle = FlatStyle.Popup;
            column.HeaderText = "Управление";

            ViewStyler.styleGrid(ref devicesList);

            devicesList.DefaultCellStyle.SelectionBackColor = Color.White;
            devicesList.DefaultCellStyle.SelectionForeColor = Color.Black;
            devicesList.MultiSelect = false;
            devicesList.SelectionMode = DataGridViewSelectionMode.CellSelect;
            devicesList.SelectionChanged += DevicesList_SelectionChanged;

            devicesList.ColumnCount = _columnHeaders.Length;

            int columnIndex = 2;
            if (devicesList.Columns["change_state_column"] == null)
            {
                devicesList.Columns.Insert(columnIndex, column);
            }

            for (int i = 0; i < _columnHeaders.Length; i++)
                devicesList.Columns[i].HeaderText = _columnHeaders[i];

            devicesList.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            devicesList.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            devicesList.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            devicesList.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void DevicesList_SelectionChanged(object sender, EventArgs e)
        {
            if(devicesList.SelectedCells.Count > 0)
            devicesList.SelectedCells[0].Selected = false;
        }

        private void devicesList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        public void UpdateInformation()
        {
            fillGrid();
        }

        private void fillGrid()
        {
            if (Core.GetConfig() == null)
                return;

            devicesList.RowCount = Core.GetConfig().Devices.Count;

            for (int i = 0; i < Core.GetConfig().Devices.Count; i++)
            {
                devicesList[0, i].Value = Core.GetConfig().Devices[i].Number;
                devicesList[1, i].Value = Core.GetConfig().Devices[i].Name;
                devicesList[2, i].Value = "Включить";
            }
        }

        private void devicesList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void devicesList_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == devicesList.Columns["change_state_column"].Index)
            {
                if ((string)devicesList[e.ColumnIndex, e.RowIndex].Value == "Включить")
                {
                    devicesList[e.ColumnIndex, e.RowIndex].Value = "Выключить";
                    devicesList[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.GreenYellow;
                }
                else
                {
                    devicesList[e.ColumnIndex, e.RowIndex].Value = "Включить";
                    devicesList[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.White;
                }

            }
        }
    }
}

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
using SteppersControlCore.CommunicationProtocol.AdditionalCommands;
using SteppersControlCore.SerialCommunication;
using SteppersControlApp.Utils;

namespace SteppersControlApp.Views
{
    public partial class DevicesControlView : UserControl
    {
        string[] _columnHeaders = { "#", "Название"};
        
        public DevicesControlView()
        {
            InitializeComponent();
            drawGrid();
        }
        
        private void drawGrid()
        {
            DataGridViewButtonColumn column = new DataGridViewButtonColumn();

            column.Name = "change_state_column";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            column.FlatStyle = FlatStyle.Popup;
            column.HeaderText = "Управление";

            ViewStyler.styleGrid((DataGridView)devicesList2);

            devicesList2.DefaultCellStyle.SelectionBackColor = Color.White;
            devicesList2.DefaultCellStyle.SelectionForeColor = Color.Black;
            devicesList2.MultiSelect = false;
            devicesList2.SelectionMode = DataGridViewSelectionMode.CellSelect;
            devicesList2.SelectionChanged += devicesList2_SelectionChanged;
            devicesList2.CellMouseDown += devicesList2_CellMouseDown;

            devicesList2.ColumnCount = _columnHeaders.Length;

            int columnIndex = 2;
            if (devicesList2.Columns["change_state_column"] == null)
            {
                devicesList2.Columns.Insert(columnIndex, column);
            }

            for (int i = 0; i < _columnHeaders.Length; i++)
                devicesList2.Columns[i].HeaderText = _columnHeaders[i];

            devicesList2.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            devicesList2.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            devicesList2.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            devicesList2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void devicesList2_SelectionChanged(object sender, EventArgs e)
        {
            if(devicesList2.SelectedCells.Count > 0)
            devicesList2.SelectedCells[0].Selected = false;
        }


        public void UpdateInformation()
        {
            fillGrid();
        }

        private void fillGrid()
        {
            if (Core.Settings == null)
                return;

            devicesList2.RowCount = Core.Settings.Devices.Count;

            for (int i = 0; i < Core.Settings.Devices.Count; i++)
            {
                devicesList2[0, i].Value = Core.Settings.Devices[i].Number;
                devicesList2[1, i].Value = Core.Settings.Devices[i].Name;
                devicesList2[2, i].Value = "Включить";
            }
        }

        private void devicesList2_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == devicesList2.Columns["change_state_column"].Index)
            {
                int device = (int)devicesList2[0, e.RowIndex].Value;
                SetDeviceStateCommand.DeviseState state;

                if ((string)devicesList2[e.ColumnIndex, e.RowIndex].Value == "Включить")
                {
                    devicesList2[e.ColumnIndex, e.RowIndex].Value = "Выключить";
                    devicesList2[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.GreenYellow;

                    state = SetDeviceStateCommand.DeviseState.DEVICE_ON;
                }
                else
                {
                    devicesList2[e.ColumnIndex, e.RowIndex].Value = "Включить";
                    devicesList2[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.White;

                    state = SetDeviceStateCommand.DeviseState.DEVICE_OFF;
                }

                Core.Serial.SendPacket(new SetDeviceStateCommand(device, state).GetBytes());
            }
        }
    }
}

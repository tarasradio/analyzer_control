using AnalyzerCommunication.CommunicationProtocol.AdditionalCommands;
using AnalyzerControlCore;
using PresentationWinForms.Utils;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PresentationWinForms.Views
{
    public partial class DevicesView : UserControl
    {
        string[] columnHeaders = { "#", "Название"};
        
        public DevicesView()
        {
            InitializeComponent();
            DrawGrid();
        }
        
        private void DrawGrid()
        {
            DataGridViewButtonColumn column = new DataGridViewButtonColumn();

            column.Name = "change_state_column";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            column.FlatStyle = FlatStyle.Popup;
            column.HeaderText = "Управление";

            DataGridViewStyler.StyleGrid(DevicesGridView);

            DevicesGridView.DefaultCellStyle.SelectionBackColor = Color.White;
            DevicesGridView.DefaultCellStyle.SelectionForeColor = Color.Black;
            DevicesGridView.MultiSelect = false;
            DevicesGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
            DevicesGridView.SelectionChanged += devicesList2_SelectionChanged;
            DevicesGridView.CellMouseDown += devicesList2_CellMouseDown;

            DevicesGridView.ColumnCount = columnHeaders.Length;

            int columnIndex = 2;
            if (DevicesGridView.Columns["change_state_column"] == null)
            {
                DevicesGridView.Columns.Insert(columnIndex, column);
            }

            for (int i = 0; i < columnHeaders.Length; i++)
                DevicesGridView.Columns[i].HeaderText = columnHeaders[i];

            DevicesGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            DevicesGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DevicesGridView.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            DevicesGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void devicesList2_SelectionChanged(object sender, EventArgs e)
        {
            if(DevicesGridView.SelectedCells.Count > 0)
            DevicesGridView.SelectedCells[0].Selected = false;
        }


        public void UpdateInformation()
        {
            FillGrid();
        }

        private void FillGrid()
        {
            if (Core.AppConfig == null)
                return;

            DevicesGridView.RowCount = Core.AppConfig.Devices.Count;

            for (int i = 0; i < Core.AppConfig.Devices.Count; i++)
            {
                DevicesGridView[0, i].Value = Core.AppConfig.Devices[i].Number;
                DevicesGridView[1, i].Value = Core.AppConfig.Devices[i].Name;
                DevicesGridView[2, i].Value = "Включить";
            }
        }

        private void devicesList2_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == DevicesGridView.Columns["change_state_column"].Index)
            {
                int device = (int)DevicesGridView[0, e.RowIndex].Value;
                SetDeviceStateCommand.DeviseState state;

                if ((string)DevicesGridView[e.ColumnIndex, e.RowIndex].Value == "Включить")
                {
                    DevicesGridView[e.ColumnIndex, e.RowIndex].Value = "Выключить";
                    DevicesGridView[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.GreenYellow;

                    state = SetDeviceStateCommand.DeviseState.DEVICE_ON;
                }
                else
                {
                    DevicesGridView[e.ColumnIndex, e.RowIndex].Value = "Включить";
                    DevicesGridView[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.White;

                    state = SetDeviceStateCommand.DeviseState.DEVICE_OFF;
                }

                Core.Serial.SendPacket(new SetDeviceStateCommand(device, state).GetBytes());
            }
        }
    }
}

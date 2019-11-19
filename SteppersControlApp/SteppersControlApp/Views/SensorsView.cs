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
using SteppersControlApp.Utils;

namespace SteppersControlApp.Views
{
    public partial class SensorsView : UserControl
    {
        string[] _columnHeaders = { "#", "Название", "Значение" };
        
        Timer _updateTimer = new Timer();

        private static object _syncRoot = new object();

        public SensorsView()
        {
            InitializeComponent();

            drawGrid();

            _updateTimer.Interval = 100;
            _updateTimer.Tick += updateValues;
        }
        
        private void updateValues(object sender, EventArgs e)
        {
            ushort[] newValues = Core.GetSensorsValues();
            lock(_syncRoot)
            {
                for (int i = 0; i < Core.Settings.Sensors.Count; i++)
                {
                    if(i == 15)
                        sensorsList[2, i].Value = (double)newValues[i] * 0.00488281;
                    else
                        sensorsList[2, i].Value = newValues[i];
                }
            }
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
            ViewStyler.styleGrid(sensorsList);

            sensorsList.DefaultCellStyle.SelectionBackColor = Color.White;
            sensorsList.DefaultCellStyle.SelectionForeColor = Color.Black;
            sensorsList.MultiSelect = false;
            sensorsList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            sensorsList.ColumnCount = _columnHeaders.Length;
            
            for (int i = 0; i < _columnHeaders.Length; i++)
                sensorsList.Columns[i].HeaderText = _columnHeaders[i];

            sensorsList.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            sensorsList.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            sensorsList.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            sensorsList.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            sensorsList.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            sensorsList.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            sensorsList.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void fillGrid()
        {
            if (Core.Settings == null)
                return;

            sensorsList.RowCount = Core.Settings.Sensors.Count;

            for (int i = 0; i < Core.Settings.Sensors.Count; i++)
            {
                sensorsList[0, i].Value = Core.Settings.Sensors[i].Number;
                sensorsList[1, i].Value = Core.Settings.Sensors[i].Name;
                sensorsList[2, i].Value = "Не задано";
            }
        }

        public void UpdateInformation()
        {
            fillGrid();
        }
    }
}

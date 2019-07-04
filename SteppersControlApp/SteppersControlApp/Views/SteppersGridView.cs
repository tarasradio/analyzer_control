using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SteppersControlCore.CommunicationProtocol.Responses;
using SteppersControlCore;

namespace SteppersControlApp.Views
{
    public partial class SteppersGridView : UserControl
    {
        string[] GridHeaders = { "#", "Название", "Статус", "Концевик" };

        int Columns = 4;

        Configuration _configuration;

        Timer updateTimer = new Timer();

        public SteppersGridView()
        {
            InitializeComponent();
            drawGrid();

            mutex = new System.Threading.Mutex();

            updateTimer.Interval = 100;
            updateTimer.Tick += UpdateTimer_Tick;
            //updateTimer.Start();
        }

        public void StartUpdate()
        {
            updateTimer.Start();
        }

        public void StopUpdate()
        {
            updateTimer.Stop();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            ShowStates();
        }

        private void drawGrid()
        {
            ViewStyler.styleGrid(ref steppersGrid);

            steppersGrid.ColumnCount = Columns;

            for (int i = 0; i < Columns; i++)
                steppersGrid.Columns[i].HeaderText = GridHeaders[i];
            
            steppersGrid.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            steppersGrid.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            steppersGrid.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            steppersGrid.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            steppersGrid.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            steppersGrid.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            steppersGrid.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            steppersGrid.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void fillGrid()
        {
            steppersGrid.RowCount = _configuration.Steppers.Count;

            for(int i = 0; i < _configuration.Steppers.Count; i++)
            {
                steppersGrid[0, i].Value = _configuration.Steppers[i].Number;
                steppersGrid[1, i].Value = _configuration.Steppers[i].Name;
            }
        }

        public void UpdateInformation()
        {
            fillGrid();
        }

        private System.Threading.Mutex mutex;

        private ushort[] _states = new ushort[18];

        public void ShowStates()
        {
            mutex.WaitOne();

            ushort[] localStates = new ushort[18];
            Array.Copy(_states, localStates, _states.Length);

            mutex.ReleaseMutex();

            for (int i = 0; i < 18; i++)
            {
                string stateStr = "Stopped";
                if ((localStates[i] & (ushort)DriverState.STATUS_MOT_STATUS) == (ushort)StepperState.ACCELERATION)
                    stateStr = "Acceleration";
                if ((localStates[i] & (ushort)DriverState.STATUS_MOT_STATUS) == (ushort)StepperState.DECELERATION)
                    stateStr = "Deceleration";
                if ((localStates[i] & (ushort)DriverState.STATUS_MOT_STATUS) == (ushort)StepperState.CONSTANT_SPEED)
                    stateStr = "Constant speed";
                steppersGrid[2, i].Value = stateStr;

                if ((localStates[i] & (ushort)DriverState.STATUS_SW_F) != 0)
                {
                    steppersGrid[3, i].Value = "PRESSED";
                    steppersGrid[3, i].Style.BackColor = Color.Red;
                }
                else
                {
                    steppersGrid[3, i].Value = "LEAVE";
                    steppersGrid[3, i].Style.BackColor = Color.GreenYellow;
                }
            }
        }

        public void UpdateSteppersStatus(ushort[] states)
        {
            if (states.Length != 18)
                return;
            mutex.WaitOne();

            Array.Copy(states, _states, states.Length);

            mutex.ReleaseMutex();
        }

        public void SetConfiguration(Configuration configuration)
        {
            _configuration = configuration;
        }
    }
}

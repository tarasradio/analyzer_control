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
    public delegate void SelectionChangedDelegate(int position);

    public partial class SteppersGridView : UserControl
    {
        public event SelectionChangedDelegate StepperChanged;

        string[] GridHeaders = { "#", "Название", "Статус", "Концевик" };

        int Columns = 4;

        Timer updateTimer = new Timer();

        private System.Threading.Mutex mutex;

        bool gridFilled = false;

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
            if (Core.GetConfig() == null)
                return;

            steppersGrid.RowCount = Core.GetConfig().Steppers.Count;

            for(int i = 0; i < Core.GetConfig().Steppers.Count; i++)
            {
                steppersGrid[0, i].Value = Core.GetConfig().Steppers[i].Number;
                steppersGrid[1, i].Value = Core.GetConfig().Steppers[i].Name;
            }

            gridFilled = true;
        }

        public void UpdateInformation()
        {
            fillGrid();
        }
        
        private ushort[] _states = new ushort[18];

        public void ShowStates()
        {
            mutex.WaitOne();

            ushort[] localStates = new ushort[18];
            Array.Copy(_states, localStates, _states.Length);

            mutex.ReleaseMutex();

            for (int i = 0; i < 18; i++)
            {
                string stateStr = "Остановлен";
                if ((localStates[i] & (ushort)DriverState.STATUS_MOT_STATUS) == (ushort)StepperState.ACCELERATION)
                    stateStr = "Ускорение";
                if ((localStates[i] & (ushort)DriverState.STATUS_MOT_STATUS) == (ushort)StepperState.DECELERATION)
                    stateStr = "Замедление";
                if ((localStates[i] & (ushort)DriverState.STATUS_MOT_STATUS) == (ushort)StepperState.CONSTANT_SPEED)
                    stateStr = "В движении";
                steppersGrid[2, i].Value = stateStr;

                if ((localStates[i] & (ushort)DriverState.STATUS_SW_F) != 0)
                {
                    steppersGrid[3, i].Value = "Нажат";
                    steppersGrid[3, i].Style.BackColor = Color.Red;
                }
                else
                {
                    steppersGrid[3, i].Value = "Отпущен";
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
        
        private void steppersGrid_SelectionChanged(object sender, EventArgs e)
        {
            int index = steppersGrid.CurrentRow.Index;
            if (gridFilled == false)
                return;
            
            int stepper = (int)(steppersGrid[0, steppersGrid.CurrentRow.Index].Value);
            StepperChanged(stepper);
        }
    }
}

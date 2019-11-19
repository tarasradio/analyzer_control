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
using SteppersControlApp.Utils;

namespace SteppersControlApp.Views
{
    public delegate void SelectionChangedDelegate(int position);

    public partial class SteppersGridView : UserControl
    {
        public event SelectionChangedDelegate StepperChanged;

        string[] GridHeaders = { "#", "Название", "Статус", "Концевик" };

        int Columns = 4;

        Timer updateTimer = new Timer();

        private static object _syncRoot = new object();

        bool gridFilled = false;

        public SteppersGridView()
        {
            InitializeComponent();
            drawGrid();

            updateTimer.Interval = 100;
            updateTimer.Tick += UpdateTimer_Tick;
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
            ViewStyler.styleGrid(steppersGrid);

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
            if (Core.Settings == null)
                return;

            steppersGrid.RowCount = Core.Settings.Steppers.Count;

            for(int i = 0; i < Core.Settings.Steppers.Count; i++)
            {
                steppersGrid[0, i].Value = Core.Settings.Steppers[i].Number;
                steppersGrid[1, i].Value = Core.Settings.Steppers[i].Name;
            }

            gridFilled = true;
        }

        public void UpdateInformation()
        {
            fillGrid();
        }

        public void ShowStates()
        {
            ushort[] states = Core.GetSteppersStates();

            for (int i = 0; i < 18; i++)
            {
                string stateStr = "Остановлен";
                if ((states[i] & (ushort)DriverState.STATUS_MOT_STATUS) == (ushort)StepperState.ACCELERATION)
                    stateStr = "Ускорение";
                if ((states[i] & (ushort)DriverState.STATUS_MOT_STATUS) == (ushort)StepperState.DECELERATION)
                    stateStr = "Замедление";
                if ((states[i] & (ushort)DriverState.STATUS_MOT_STATUS) == (ushort)StepperState.CONSTANT_SPEED)
                    stateStr = "В движении";
                steppersGrid[2, i].Value = stateStr;

                if ((states[i] & (ushort)DriverState.STATUS_SW_F) != 0)
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

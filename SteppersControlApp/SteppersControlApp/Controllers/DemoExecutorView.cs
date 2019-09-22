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
using SteppersControlCore.Elements;
using SteppersControlApp.Views;

namespace SteppersControlApp.Controllers
{
    public partial class DemoExecutorView : UserControl
    {
        TubeInfo selectedTube = null;

        string[] _columnHeaders = { "#", "Штихкод", "Состояние"};

        Timer _updateTimer = new Timer();

        public DemoExecutorView()
        {
            InitializeComponent();

            buttonRemoveTube.Enabled = false;

            _updateTimer.Interval = 100;
            _updateTimer.Tick += updateValues;

            drawGrid();
        }

        private void updateValues(object sender, EventArgs e)
        {
            updateGrid();
        }

        public void StartUpdate()
        {
            _updateTimer.Start();
        }

        public void StopUpdate()
        {
            _updateTimer.Stop();
        }

        private void buttonAddTube_Click(object sender, EventArgs e)
        {
            Core.Demo.Tubes.Add(new TubeInfo());

            buttonRemoveTube.Enabled = true;
        }

        private void buttonRemoveTube_Click(object sender, EventArgs e)
        {
            Core.Demo.Tubes.Remove(selectedTube);

            if (Core.Demo.Tubes.Count == 0)
            {
                buttonRemoveTube.Enabled = false;
                selectedTube = null;
            }  
        }

        private void tubesList_SelectionChanged(object sender, EventArgs e)
        {
            if (Core.Demo.Tubes.Count == 0)
                return;
            selectedTube = Core.Demo.Tubes[tubesList.CurrentRow.Index];
            propertyGrid.SelectedObject = selectedTube;
        }

        private void updateGrid()
        {
            tubesList.RowCount = Core.Demo.Tubes.Count;

            for (int i = 0; i < Core.Demo.Tubes.Count; i++)
            {
                tubesList[0, i].Value = i + 1;
                tubesList[1, i].Value = $"{Core.Demo.Tubes[i].BarCode}";

                String state = "Не найдена";

                if(Core.Demo.Tubes[i].IsFind)
                {
                    state = $"{Core.Demo.Tubes[i].CurrentStage} из {Core.Demo.Tubes[i].Stages.Count}";
                }

                tubesList[2, i].Value = state;
            }
        }

        private void drawGrid()
        {
            ViewStyler.styleGrid(tubesList);
            
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
    }
}

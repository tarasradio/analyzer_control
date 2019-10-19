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

        string[] _tubesColumnHeaders = { "#", "Штихкод", "Состояние", "Осталось"};
        string[] _stagesColumnHeaders = { "#", "Ячейка", "Время выполнения" };

        public string[] CartridgeCellTitles = { "Белая", "Первая", "Вторая", "Третья" };

        Timer _updateTimer = new Timer();

        public DemoExecutorView()
        {
            InitializeComponent();

            buttonRemoveTube.Enabled = false;

            _updateTimer.Interval = 100;
            _updateTimer.Tick += updateValues;

            drawTubesGrid();
            drawStagesList();
        }

        private void updateValues(object sender, EventArgs e)
        {
            updateTubesGrid();
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

            showSelectedTubeProperties();
        }

        private void updateTubesGrid()
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
                tubesList[3, i].Value = Core.Demo.Tubes[i].TimeToStageComplete + " мин.";
            }
        }

        private void showSelectedTubeProperties()
        {
            updateStagesList();
            updateBarcodeField();
        }

        private void updateBarcodeField()
        {
            if (selectedTube != null)
            {
                editBarcode.Text = selectedTube.BarCode;
                buttonUpdateBarcode.Enabled = true;
            }
            else
            {
                editBarcode.Text = "";
                buttonUpdateBarcode.Enabled = false;
            }
        }

        private void updateStagesList()
        {
            if (selectedTube == null)
            {
                editStageButton.Enabled = false;
                removeStageButton.Enabled = false;
                return;
            }

            if (selectedTube.Stages.Count == 0)
            {
                editStageButton.Enabled = false;
                removeStageButton.Enabled = false;
            }
            else
            {
                editStageButton.Enabled = true;
                removeStageButton.Enabled = true;
            }

            stagesList.RowCount = selectedTube.Stages.Count;

            for (int i = 0; i < selectedTube.Stages.Count; i++)
            {
                stagesList[0, i].Value = i + 1;
                string title = CartridgeCellTitles[(int)selectedTube.Stages[i].Cell];
                stagesList[1, i].Value = title;
                stagesList[2, i].Value = $"{ selectedTube.Stages[i].TimeToPerform} минут";
            }
        }

        void showStageFields()
        {
            if (selectedTube == null || selectedTube.Stages.Count == 0)
            {
                editTimeToPerform.Value = 0;
                editCartridgePosition.Value = 0;
                selectCellType.SelectedIndex = -1;

                editTimeToPerform.Enabled = false;
                editCartridgePosition.Enabled = false;
                selectCellType.Enabled = false;

                return;
            }
            else
            {
                editTimeToPerform.Enabled = true;
                editCartridgePosition.Enabled = true;
                selectCellType.Enabled = true;

                editTimeToPerform.Value = selectedTube.Stages[stagesList.CurrentRow.Index].TimeToPerform;
                editCartridgePosition.Value = selectedTube.Stages[stagesList.CurrentRow.Index].CartridgePosition;
                selectCellType.SelectedIndex = (int)selectedTube.Stages[stagesList.CurrentRow.Index].Cell - 1;
            }
        }

        private void drawTubesGrid()
        {
            ViewStyler.styleGrid(tubesList);
            
            tubesList.MultiSelect = false;
            tubesList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            tubesList.ColumnCount = _tubesColumnHeaders.Length;

            for (int i = 0; i < _tubesColumnHeaders.Length; i++)
                tubesList.Columns[i].HeaderText = _tubesColumnHeaders[i];

            tubesList.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            tubesList.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tubesList.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            tubesList.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            tubesList.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            tubesList.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tubesList.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void drawStagesList()
        {
            ViewStyler.styleGrid(stagesList);

            stagesList.MultiSelect = false;
            stagesList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            stagesList.ColumnCount = _stagesColumnHeaders.Length;

            for (int i = 0; i < _stagesColumnHeaders.Length; i++)
                stagesList.Columns[i].HeaderText = _stagesColumnHeaders[i];

            stagesList.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            stagesList.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            stagesList.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            stagesList.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            stagesList.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            stagesList.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            stagesList.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            stagesList.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            stagesList.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void addStageButton_Click(object sender, EventArgs e)
        {
            if (selectedTube == null) return;

            selectedTube.Stages.Add(new Stage() {
                CartridgePosition = 0,
                Cell = CartridgeCell.FirstCell,
                TimeToPerform = 5 });

            updateStagesList();
        }

        private void editStageButton_Click(object sender, EventArgs e)
        {
            if (selectedTube == null) return;
        }

        private void removeStageButton_Click(object sender, EventArgs e)
        {
            if (selectedTube == null) return;
            if (selectedTube.Stages.Count == 0) return;

            selectedTube.Stages.RemoveAt(stagesList.CurrentRow.Index);

            updateStagesList();
        }

        private void stagesList_SelectionChanged(object sender, EventArgs e)
        {
            showStageFields();
        }

        private void saveStageChangesButton_Click(object sender, EventArgs e)
        {
            selectedTube.Stages[stagesList.CurrentRow.Index].TimeToPerform = (int)editTimeToPerform.Value;
            selectedTube.Stages[stagesList.CurrentRow.Index].CartridgePosition = (int)editCartridgePosition.Value;
            selectedTube.Stages[stagesList.CurrentRow.Index].Cell = (CartridgeCell)(selectCellType.SelectedIndex + 1);

            updateStagesList();
        }

        private void buttonUpdateBarcode_Click(object sender, EventArgs e)
        {
            if (selectedTube == null) return;
            selectedTube.BarCode = editBarcode.Text;
        }
    }
}

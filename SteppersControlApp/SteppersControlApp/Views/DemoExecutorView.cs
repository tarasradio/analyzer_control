using SteppersControlApp.Forms;
using SteppersControlApp.Views;
using SteppersControlCore;
using SteppersControlCore.Elements;
using System;
using System.Windows.Forms;

namespace SteppersControlApp.ControllersViews
{
    public partial class DemoExecutorView : UserControl
    {
        TubeInfo selectedTube = null;

        string[] analisesColumnHeaders = { "#", "Штихкод", "Состояние", "Осталось"};

        Timer _updateTimer = new Timer();

        public DemoExecutorView()
        {
            InitializeComponent();
        }

        private void DemoExecutorView_Load(object sender, EventArgs e)
        {
            drawTubesGrid();
            
            buttonRemoveTube.Visible = false;

            _updateTimer.Interval = 100;
            _updateTimer.Tick += updateValues;
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
            Core.Demo.Properties.Tubes.Add(new TubeInfo());

            buttonRemoveTube.Visible = true;
        }

        private void buttonRemoveTube_Click(object sender, EventArgs e)
        {
            Core.Demo.Properties.Tubes.Remove(selectedTube);

            if (Core.Demo.Properties.Tubes.Count == 0)
            {
                buttonRemoveTube.Visible = false;
                selectedTube = null;
            }  
        }

        private void tubesList_SelectionChanged(object sender, EventArgs e)
        {
            if (Core.Demo.Properties.Tubes.Count == 0)
                return;
            selectedTube = Core.Demo.Properties.Tubes[tubesList.CurrentRow.Index];

            buttonRemoveTube.Visible = true;
        }

        private void updateTubesGrid()
        {
            if (Core.Demo.Properties.Tubes == null)
                return;
            tubesList.RowCount = Core.Demo.Properties.Tubes.Count;

            for (int i = 0; i < Core.Demo.Properties.Tubes.Count; i++)
            {
                tubesList[0, i].Value = i + 1;
                tubesList[1, i].Value = $"{Core.Demo.Properties.Tubes[i].BarCode}";

                string state = "Не найдена";

                if(Core.Demo.Properties.Tubes[i].IsFind)
                {
                    state = $"{Core.Demo.Properties.Tubes[i].CurrentStage} из {Core.Demo.Properties.Tubes[i].Stages.Count}";
                }

                tubesList[2, i].Value = state;
                tubesList[3, i].Value = Core.Demo.Properties.Tubes[i].TimeToStageComplete + " мин.";
            }
        }

        private void drawTubesGrid()
        {
            ViewStyler.styleGrid(tubesList);
            
            tubesList.MultiSelect = false;
            tubesList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            tubesList.ColumnCount = analisesColumnHeaders.Length;

            for (int i = 0; i < analisesColumnHeaders.Length; i++)
                tubesList.Columns[i].HeaderText = analisesColumnHeaders[i];

            tubesList.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            tubesList.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tubesList.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            tubesList.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            tubesList.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            tubesList.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            tubesList.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void buttonEditTube_Click(object sender, EventArgs e)
        {
            EditTubeDialogForm dialogForm = new EditTubeDialogForm();
            dialogForm.SetTube(selectedTube);
            dialogForm.StartPosition = FormStartPosition.CenterScreen;
            dialogForm.ShowDialog();
        }
    }
}

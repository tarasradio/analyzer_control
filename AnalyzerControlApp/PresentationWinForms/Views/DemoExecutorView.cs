using PresentationWinForms.Forms;
using PresentationWinForms.Utils;
using AnalyzerControlCore;
using System;
using System.Windows.Forms;
using AnalyzerConfiguration;

namespace PresentationWinForms.UnitsViews
{
    public partial class DemoExecutorView : UserControl
    {
        AnalysisInfo selectedTube = null;

        private readonly string[] analisesColumnHeaders = { "#", "Штихкод", "Состояние", "Осталось"};

        Timer updateTimer = new Timer();

        public DemoExecutorView()
        {
            InitializeComponent();
        }

        private void DemoExecutorView_Load(object sender, EventArgs e)
        {
            DrawTubesGrid();
            
            buttonRemoveTube.Visible = false;

            updateTimer.Interval = 100;
            updateTimer.Tick += updateValues;
        }

        private void updateValues(object sender, EventArgs e)
        {
            UpdateTubesGrid();
        }

        public void StartUpdate()
        {
            updateTimer.Start();
        }

        public void StopUpdate()
        {
            updateTimer.Stop();
        }

        private void buttonAddTube_Click(object sender, EventArgs e)
        {
            AnalyzerGateway.Demo.Options.AnalysisList.Add(new AnalysisInfo());

            buttonRemoveTube.Visible = true;
        }

        private void buttonRemoveTube_Click(object sender, EventArgs e)
        {
            AnalyzerGateway.Demo.Options.AnalysisList.Remove(selectedTube);

            if (AnalyzerGateway.Demo.Options.AnalysisList.Count == 0)
            {
                buttonRemoveTube.Visible = false;
                selectedTube = null;
            }  
        }

        private void tubesList_SelectionChanged(object sender, EventArgs e)
        {
            if (AnalyzerGateway.Demo.Options.AnalysisList.Count == 0)
                return;
            selectedTube = AnalyzerGateway.Demo.Options.AnalysisList[tubesList.CurrentRow.Index];

            buttonRemoveTube.Visible = true;
        }

        private void UpdateTubesGrid()
        {
            if (AnalyzerGateway.Demo.Options.AnalysisList == null)
                return;
            tubesList.RowCount = AnalyzerGateway.Demo.Options.AnalysisList.Count;

            for (int i = 0; i < AnalyzerGateway.Demo.Options.AnalysisList.Count; i++)
            {
                tubesList[0, i].Value = i + 1;
                tubesList[1, i].Value = $"{AnalyzerGateway.Demo.Options.AnalysisList[i].BarCode}";

                string state = "Не найдена";

                if(AnalyzerGateway.Demo.Options.AnalysisList[i].IsFind)
                {
                    state = $"{AnalyzerGateway.Demo.Options.AnalysisList[i].CurrentStage} из {AnalyzerGateway.Demo.Options.AnalysisList[i].Stages.Count}";
                }

                tubesList[2, i].Value = state;
                tubesList[3, i].Value = AnalyzerGateway.Demo.Options.AnalysisList[i].TimeToStageComplete + " мин.";
            }
        }

        private void DrawTubesGrid()
        {
            DataGridViewStyler.StyleGrid(tubesList);
            
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
            SelectedTubeShowDialog();
        }

        private void tubesList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectedTubeShowDialog();
        }

        private void SelectedTubeShowDialog()
        {
            if (selectedTube != null)
            {
                EditTubeDialogForm dialogForm = new EditTubeDialogForm();

                dialogForm.SetTube(selectedTube);
                dialogForm.StartPosition = FormStartPosition.CenterScreen;
                dialogForm.ShowDialog();
            }
        }
    }
}

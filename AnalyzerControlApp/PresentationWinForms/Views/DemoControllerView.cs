using AnalyzerService;
using AnalyzerDomain.Entyties;
using PresentationWinForms.Forms;
using PresentationWinForms.Utils;
using System;
using System.Windows.Forms;
using AnalyzerControl;

namespace PresentationWinForms.UnitsViews
{
    public partial class DemoControllerView : UserControl
    {
        public AnalyzerDemoController Controller { get; set; }
        AnalysisInfo selectedTube = null;
        
        Timer updateTimer = new Timer();

        private readonly string[] analisesColumnHeaders = { "#", "Штихкод", "Состояние", "Осталось"};

        public DemoControllerView()
        {
            InitializeComponent();
        }

        private void DemoControllerView_Load(object sender, EventArgs e)
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
            Controller.Options.Analyzes.Add(new AnalysisInfo());

            buttonRemoveTube.Visible = true;
        }

        private void buttonRemoveTube_Click(object sender, EventArgs e)
        {
            Controller.Options.Analyzes.Remove(selectedTube);

            if (Controller.Options.Analyzes.Count == 0)
            {
                buttonRemoveTube.Visible = false;
                selectedTube = null;
            }  
        }

        private void tubesList_SelectionChanged(object sender, EventArgs e)
        {
            if (Controller.Options.Analyzes.Count == 0)
                return;
            selectedTube = Controller.Options.Analyzes[tubesList.CurrentRow.Index];

            buttonRemoveTube.Visible = true;
        }

        private void UpdateTubesGrid()
        {
            if (Controller.Options.Analyzes == null)
                return;
            tubesList.RowCount = Controller.Options.Analyzes.Count;

            for (int i = 0; i < Controller.Options.Analyzes.Count; i++)
            {
                tubesList[0, i].Value = i + 1;
                tubesList[1, i].Value = $"{Controller.Options.Analyzes[i].BarCode}";

                string state = "Не найдена";

                if(Controller.Options.Analyzes[i].IsFind)
                {
                    state = $"{Controller.Options.Analyzes[i].CurrentStage} из {Controller.Options.Analyzes[i].Stages.Count}";
                }

                tubesList[2, i].Value = state;
                tubesList[3, i].Value = Controller.Options.Analyzes[i].TimeToStageComplete + " мин.";
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

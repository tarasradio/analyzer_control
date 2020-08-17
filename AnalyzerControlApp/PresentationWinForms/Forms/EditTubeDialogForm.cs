using AnalyzerConfiguration;
using PresentationWinForms.Utils;
using System;
using System.Windows.Forms;

namespace PresentationWinForms.Forms
{
    public partial class EditTubeDialogForm : Form
    {
        Tube tube;

        string[] stagesColumnHeaders = { "#", "Ячейка", "Время выполнения" };
        string[] cartridgeCellTitles = { "Белая", "Первая", "Вторая", "Третья" };

        public EditTubeDialogForm()
        {
            InitializeComponent();
        }

        public void SetTube(Tube tube)
        {
            this.tube = tube;
        }

        private void EditTubeDialogForm_Load(object sender, EventArgs e)
        {
            drawStagesList();
            updateStagesList();
            updateBarcodeField();
        }

        private void buttonUpdateBarcode_Click(object sender, EventArgs e)
        {
            if (tube == null) return;
            tube.BarCode = editBarcode.Text;
        }

        private void addStageButton_Click(object sender, EventArgs e)
        {
            if (tube == null) return;

            tube.Stages.Add(new Stage()
            {
                CartridgePosition = 0,
                Cell = CartridgeCell.FirstCell,
                TimeToPerform = 5
            });

            updateStagesList();
        }

        private void removeStageButton_Click(object sender, EventArgs e)
        {
            if (tube == null) return;
            if (tube.Stages.Count == 0) return;

            tube.Stages.RemoveAt(stagesList.CurrentRow.Index);

            updateStagesList();
        }

        private void saveStageChangesButton_Click(object sender, EventArgs e)
        {
            tube.Stages[stagesList.CurrentRow.Index].TimeToPerform = (int)editTimeToPerform.Value;
            tube.Stages[stagesList.CurrentRow.Index].CartridgePosition = (int)editCartridgePosition.Value;
            tube.Stages[stagesList.CurrentRow.Index].Cell = (CartridgeCell)(selectCellType.SelectedIndex + 1);

            updateStagesList();
        }

        private void stagesList_SelectionChanged(object sender, EventArgs e)
        {
            showStageFields();
        }

        private void updateBarcodeField()
        {
            if (tube != null)
            {
                editBarcode.Text = tube.BarCode;
                buttonUpdateBarcode.Enabled = Visible;
            }
            else
            {
                editBarcode.Text = "";
                buttonUpdateBarcode.Enabled = Visible;
            }
        }

        private void updateStagesList()
        {
            if (tube == null)
            {
                removeStageButton.Visible = false;
                return;
            }

            removeStageButton.Visible = tube.Stages.Count > 0 ? true : false;

            stagesList.RowCount = tube.Stages.Count;

            for (int i = 0; i < tube.Stages.Count; i++)
            {
                stagesList[0, i].Value = i + 1;
                string title = cartridgeCellTitles[(int)tube.Stages[i].Cell];
                stagesList[1, i].Value = title;
                stagesList[2, i].Value = $"{ tube.Stages[i].TimeToPerform} минут";
            }
        }

        void showStageFields()
        {
            if (tube == null || tube.Stages.Count == 0)
            {
                editTimeToPerform.Value = 0;
                editCartridgePosition.Value = 0;
                selectCellType.SelectedIndex = -1;

                editTimeToPerform.Enabled = false;
                editCartridgePosition.Enabled = false;
                selectCellType.Enabled = false;
                saveStageChangesButton.Visible = false;

                return;
            }
            else
            {
                editTimeToPerform.Enabled = true;
                editCartridgePosition.Enabled = true;
                selectCellType.Enabled = true;
                saveStageChangesButton.Visible = true;

                editTimeToPerform.Value = tube.Stages[stagesList.CurrentRow.Index].TimeToPerform;
                editCartridgePosition.Value = tube.Stages[stagesList.CurrentRow.Index].CartridgePosition;
                selectCellType.SelectedIndex = (int)tube.Stages[stagesList.CurrentRow.Index].Cell - 1;
            }
        }

        private void drawStagesList()
        {
            DataGridViewStyler.StyleGrid(stagesList);

            stagesList.MultiSelect = false;
            stagesList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            stagesList.ColumnCount = stagesColumnHeaders.Length;

            for (int i = 0; i < stagesColumnHeaders.Length; i++)
                stagesList.Columns[i].HeaderText = stagesColumnHeaders[i];

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
    }
}

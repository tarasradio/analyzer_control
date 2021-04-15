using AnalyzerService;
using PresentationWinForms.Utils;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PresentationWinForms.Views
{
    public partial class SensorsView : UserControl
    {
        private Analyzer analyzer;
        string[] columnHeaders = { "#", "Название", "Значение" };
        
        Timer updateTimer = new Timer();

        private static object locker = new object();

        public SensorsView()
        {
            InitializeComponent();

            DrawGrid();

            updateTimer.Interval = 100;
            updateTimer.Tick += UpdateValues;
        }

        public void Init(Analyzer analyzer)
        {
            this.analyzer = analyzer;

        }
        
        private void UpdateValues(object sender, EventArgs e)
        {
            ushort[] newValues = Analyzer.State.SensorsValues;

            lock(locker)
            {
                for (int i = 0; i < newValues.Length; i++)
                {
                    if(i == 15)
                        SensorsGridView[2, i].Value = newValues[i] * 0.00488281;
                    else
                        SensorsGridView[2, i].Value = newValues[i];
                }
            }
        }

        public void StartUpdate()
        {
            updateTimer.Start();
        }

        public void StopUpdate()
        {
            updateTimer.Stop();
        }
        
        private void DrawGrid()
        {
            DataGridViewStyler.StyleGrid(SensorsGridView);

            SensorsGridView.DefaultCellStyle.SelectionBackColor = Color.White;
            SensorsGridView.DefaultCellStyle.SelectionForeColor = Color.Black;
            SensorsGridView.MultiSelect = false;
            SensorsGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            SensorsGridView.ColumnCount = columnHeaders.Length;
            
            for (int i = 0; i < columnHeaders.Length; i++)
                SensorsGridView.Columns[i].HeaderText = columnHeaders[i];

            SensorsGridView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            SensorsGridView.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            SensorsGridView.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            SensorsGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            SensorsGridView.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            SensorsGridView.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            SensorsGridView.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void FillGrid()
        {
            if (analyzer.Options == null)
                return;

            SensorsGridView.RowCount = analyzer.Options.Sensors.Count;

            for (int i = 0; i < analyzer.Options.Sensors.Count; i++)
            {
                SensorsGridView[0, i].Value = analyzer.Options.Sensors[i].Number;
                SensorsGridView[1, i].Value = analyzer.Options.Sensors[i].Name;
                SensorsGridView[2, i].Value = "Не задано";
            }
        }

        public void UpdateInformation()
        {
            FillGrid();
        }
    }
}

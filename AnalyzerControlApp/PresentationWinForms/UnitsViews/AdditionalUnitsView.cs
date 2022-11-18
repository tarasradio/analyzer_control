using AnalyzerService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationWinForms.UnitsViews
{
    public partial class AdditionalUnitsView : UserControl
    {
        public AdditionalUnitsView()
        {
            InitializeComponent();
            if (Analyzer.AdditionalDevices != null)
                propertyGrid.SelectedObject = Analyzer.AdditionalDevices.Options;
        }

        private void buttonOpenScreen_Click(object sender, EventArgs e)
        {
            Analyzer.TaskExecutor.StartTask(
               () =>
               {
                   Analyzer.AdditionalDevices.OpenScreen();
               });
        }

        private void buttonCloseScreen_Click(object sender, EventArgs e)
        {
            Analyzer.TaskExecutor.StartTask(
               () =>
               {
                   Analyzer.AdditionalDevices.CloseScreen();
               });
        }

        private void buttonHomeWashBuffer_Click(object sender, EventArgs e)
        {
            Analyzer.TaskExecutor.StartTask(
               () =>
               {
                   Analyzer.AdditionalDevices.HomeWashBuffer();
               });
        }

        private void buttonPutDownWashBuffer_Click(object sender, EventArgs e)
        {
            Analyzer.TaskExecutor.StartTask(
               () =>
               {
                   Analyzer.AdditionalDevices.PutDownWashBuffer();
               });
        }
    }
}

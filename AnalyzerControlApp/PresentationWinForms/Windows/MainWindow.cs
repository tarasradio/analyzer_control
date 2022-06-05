using AnalyzerService;
using Infrastructure;
using System;
using System.Drawing;
using System.Windows.Forms;
using AnalyzerControl;
using AnalyzerControl.Services;

namespace PresentationWinForms.Forms
{
    public partial class MainWindow : Form
    {
        private Analyzer analyzer = null;
        private ConveyorService conveyorService = null;
        private AnalyzerDemoController demoController = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void Init(Analyzer analyzer, ConveyorService conveyor, AnalyzerDemoController controller)
        {
            this.analyzer = analyzer;

            devicesControlView.Init(analyzer);
            sensorsView.Init(analyzer);
            steppersGridView.Init(analyzer);

            steppersGridView.StartUpdate();
            sensorsView.StartUpdate();

            Analyzer.Serial.ConnectionChanged += Serial_ConnectionChanged;

            this.conveyorService = conveyor;

            this.demoController = controller;
            demoExecutorView.Controller = this.demoController;
        }

        private void Serial_ConnectionChanged(bool connected)
        {
            updateControlsState();
        }

        private void buttonShowControlPanel_Click(object sender, EventArgs e)
        {
            ControlPanelWindow controlPanel = new ControlPanelWindow(analyzer.Options.Steppers);
            controlPanel.StartPosition = FormStartPosition.CenterScreen;
            controlPanel.Show();

            controlPanel.Init(analyzer);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            updateControlsState();

            steppersGridView.UpdateInformation();
            devicesControlView.UpdateInformation();
            sensorsView.UpdateInformation();

            demoExecutorView.StartUpdate();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(
                "Сохранить настройки при выходе из программы?",
                "Сохранение настроек при выходе из программы",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Information,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);

            if(dialogResult == DialogResult.Yes)
            {
                analyzer.SaveConfiguration("AnalyzerServiceConfiguration");
            }
            else if(dialogResult == DialogResult.Cancel ||
                dialogResult == DialogResult.Abort)
            {
                e.Cancel = true;
                return;
            }

            demoExecutorView.StopUpdate();

            if (Analyzer.Serial.IsOpen())
            {
                Analyzer.Serial.Close();

                steppersGridView.StopUpdate();
                sensorsView.StopUpdate();
            }
        }

        private void abortExecutionButton_Click(object sender, EventArgs e)
        {
            Analyzer.AbortExecution();
        }

        private void buttonStartDemo_Click(object sender, EventArgs e)
        {
            demoController.StartWork();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F8)
            {
                Analyzer.AbortExecution();
            }
            if(e.KeyCode == Keys.F7)
            {
                demoController.StartWork();
            }
        }

        private void buttonConnection_Click(object sender, EventArgs e)
        {
            ConnectionSettingsWindow settingsWindow = new ConnectionSettingsWindow();
            settingsWindow.StartPosition = FormStartPosition.CenterScreen;
            settingsWindow.Show();
        }

        private void updateControlsState()
        {
            if (Analyzer.Serial.IsOpen())
            {
                connectionState.Text = $"Установленно соединение с { Analyzer.Serial.PortName }.";
                connectionState.ForeColor = Color.DarkGreen;
            }
            else
            {
                connectionState.Text = "Соединение не установлено.";
                connectionState.ForeColor = Color.Brown;
            }
            buttonStartDemo.Visible = Analyzer.Serial.IsOpen();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if(Analyzer.Serial.IsOpen())
            {
                openMonitor();
            }
            
        }

        private void openMonitor()
        {
            Analyzer.TaskExecutor.StartTask( () =>
            {
                Analyzer.AdditionalDevices.OpenScreen();
            });
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Analyzer.TaskExecutor.StartTask(() =>
            {
                Analyzer.AdditionalDevices.CloseScreen();
            });
        }
    }
}
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

using FastColoredTextBoxNS;
using System.Text.RegularExpressions;

using SteppersControlCore.MachineControl;

using SteppersControlCore.CommunicationProtocol;
using SteppersControlCore.CommunicationProtocol.AdditionalCommands;
using SteppersControlCore.CommunicationProtocol.CncCommands;
using SteppersControlCore.CommunicationProtocol.StepperCommands;
using SteppersControlCore.SerialCommunication;
using System.Threading;

namespace SteppersControlApp.Views
{
    public partial class CNCView : UserControl
    {
        static SerialHelper _helper;
        CncExecutor _cncExecutor;
        TaskExecutor _taskExecutor;
        
        CncProgram _program;

        public CNCView()
        {
            InitializeComponent();
            InitilizeProgramtextBox();
        }

        public void SetExecutor(CncExecutor executor)
        {
            _cncExecutor = executor;
            _taskExecutor = new TaskExecutor(_cncExecutor);
        }

        public void SetHelper(SerialHelper helper)
        {
            _helper = helper;
        }
        
        private void InitilizeProgramtextBox()
        {
            programTextBox.TextChanged += ProgramTextBox_TextChanged;
        }

        Style commentsStyle = new TextStyle(Brushes.Green, null, FontStyle.Italic);
        Style motorCommandNameStyle = new TextStyle(Brushes.Red, null, FontStyle.Bold);
        Style simpleCommandNameStyle = new TextStyle(Brushes.Green, null, FontStyle.Bold);
        Style unitCommandNameStyle = new TextStyle(Brushes.DarkOrange, null, FontStyle.Bold);
        Style deviceNumberStyle = new TextStyle(Brushes.Blue, null, FontStyle.Bold);
        Style commandsArgumentStyle = new TextStyle(Brushes.Purple, null, FontStyle.Bold);

        Style WaitCommandNameStyle = new TextStyle(Brushes.DarkOrange, null, FontStyle.Bold);

        Style digitStyle = new TextStyle(Brushes.Green, null, FontStyle.Regular);

        private void ProgramTextBox_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            e.ChangedRange.ClearStyle(motorCommandNameStyle);
            e.ChangedRange.ClearStyle(deviceNumberStyle);
            e.ChangedRange.ClearStyle(unitCommandNameStyle);
            e.ChangedRange.ClearStyle(commandsArgumentStyle);
            e.ChangedRange.ClearStyle(simpleCommandNameStyle);
            e.ChangedRange.ClearStyle(digitStyle);

            e.ChangedRange.ClearStyle(commentsStyle);

            e.ChangedRange.SetStyle(motorCommandNameStyle, @"\b(?<range>MOVE|HOME|RUN)\b", RegexOptions.IgnoreCase);
            e.ChangedRange.SetStyle(unitCommandNameStyle, @"\b(?<range>ON|OFF)\b", RegexOptions.IgnoreCase);
            e.ChangedRange.SetStyle(simpleCommandNameStyle, @"\b(?<range>SPEED|STOP)\b", RegexOptions.IgnoreCase);
            e.ChangedRange.SetStyle(deviceNumberStyle, @"\b(?<range>M|D)(\d)+\b", RegexOptions.IgnoreCase);
            e.ChangedRange.SetStyle(commandsArgumentStyle, @"\b(?<range>S|V|R|F)(-)?(\d)+\b", RegexOptions.IgnoreCase);
            e.ChangedRange.SetStyle(digitStyle, @"\b(M|S|D|V|R|F)(?<range>(-)?\d+)\b", RegexOptions.IgnoreCase);

            e.ChangedRange.SetStyle(WaitCommandNameStyle, @"\b(?<range>WAITF|WAITR|DELAY)\b", RegexOptions.IgnoreCase);

            e.ChangedRange.SetStyle(commentsStyle, @"//.*$", RegexOptions.Multiline);
        }

        bool testProgram()
        {
            CncParser parser = new CncParser();
            _program = parser.Parse(programTextBox.Text);

            Logger.AddMessage($"Программа содержит {_program.Commands.Count} команд.");

            return true;
        }

        private void buttonTestProgram_Click(object sender, EventArgs e)
        {
            testProgram();
        }

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            fileDialog.Multiselect = false;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    programTextBox.Text = System.IO.File.ReadAllText(fileDialog.FileName);
                }
                catch (System.IO.FileNotFoundException)
                {
                    Logger.AddMessage("Ошибка при открытии файла - Файл не найден.");
                }
            }
        }

        private void buttonSaveFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "txt files (*.txt)|*.txt";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    System.IO.File.WriteAllText(fileDialog.FileName, programTextBox.Text);
                }
                catch(System.IO.FileNotFoundException)
                {
                    Logger.AddMessage("Ошибка при сохранении файла.");
                }
            }
        }
        
        private void scanTubesTaskButton_Click(object sender, EventArgs e)
        {
            _taskExecutor.StartScanTubesTask();
        }
        
        private void buttonRunProgram_Click(object sender, EventArgs e)
        {
            if(testProgram())
            {
                executionStatusLabel.Text = "Выполнение программы запущено";
                executionProgressLabel.Text = $"Выполнено команд: {0} из {_program.Commands.Count}";
                executionProgressBar.Value = 0;

                _cncExecutor.StartExecution(_program.Commands);
            }
        }

        public void UpdateExecutionProgress(int executedCommandNumber)
        {
            double progress = 0;
            int commandsCount = 0;

            if (_taskExecutor.GetCommandsCount() > 0)
            {
                commandsCount = _taskExecutor.GetCommandsCount();
            }
            else
            {
                commandsCount = _program.Commands.Count;
            }

            progress = ((double)executedCommandNumber / commandsCount) * 100.0;
            executionProgressBar.Value = (int)progress;
            
            executionProgressLabel.Text = $"Выполнено команд: {executedCommandNumber} из {commandsCount}";

            if(executedCommandNumber == commandsCount)
            {
                executionStatusLabel.Text = "Выполнение программы завершено";
            }
        }

        private void buttonAbortExecution_Click(object sender, EventArgs e)
        {
            _taskExecutor.AbortExecution();
            _cncExecutor.AbortExecution();
            _helper.SendPacket(new AbortExecutionCommand(Protocol.GetPacketId()).GetBytes());
            executionStatusLabel.Text = "Выполнение программы было прерванно";
        }

        private void buttonClearFile_Click(object sender, EventArgs e)
        {
            programTextBox.Clear();
        }

        private void washingPompTaskButton_Click(object sender, EventArgs e)
        {
            _taskExecutor.StartWashingPompTask();
        }
    }
}

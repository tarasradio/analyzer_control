using AnalyzerCncControl;
using AnalyzerCommunication;
using AnalyzerService;
using AnalyzerService.ExecutionControl;
using FastColoredTextBoxNS;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace PresentationWinForms.Views
{
    public partial class CncControlView : UserControl
    {
        private List<ICommand> commands;

        public CncControlView()
        {
            InitializeComponent();
            InitilizeProgramtextBox();
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

        private void ProgramTextBox_TextChanged(object sender, TextChangedEventArgs e)
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

        private void ParseProgram(string programText)
        {
            commands = CommandsParser.Parse(programText);
        }

        private void buttonTestProgram_Click(object sender, EventArgs e)
        {
            ParseProgram(programTextBox.Text);
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
                    Logger.Debug("Ошибка при открытии файла - Файл не найден.");
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
                    Logger.Debug("Ошибка при сохранении файла.");
                }
            }
        }
        
        private void buttonRunProgram_Click(object sender, EventArgs e)
        {
            ParseProgram(programTextBox.Text);

            if(commands.Count > 0)
            {
                executionStatusLabel.Text = "Выполнение программы запущено";
                executionProgressLabel.Text = $"Выполнено команд: {0} из {commands.Count}";
                executionProgressBar.Value = 0;

                Analyzer.CommandExecutor.RunExecution(commands);
            }
        }

        public void UpdateExecutionProgress(int executedCommandNumber)
        {
            double progress = 0;
            int commandsCount = 0;

            if (Analyzer.TaskExecutor.GetState() == ThreadState.Running)
            {
                //commandsCount = _taskExecutor.GetCommandsCount();
            }
            else
            {
                commandsCount = commands.Count;
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
            Analyzer.AbortExecution();

            executionStatusLabel.Text = "Выполнение программы было прерванно";
        }

        private void buttonClearFile_Click(object sender, EventArgs e)
        {
            programTextBox.Clear();
        }
    }
}

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

namespace SteppersControlApp.Views
{
    public partial class CNCView : UserControl
    {
        static Logger _logger;
        static SerialHelper _helper;
        CncExecutor _cncExecutor;

        CncProgram _program;

        public CNCView()
        {
            InitializeComponent();
            InitilizeProgramtextBox();
        }

        public void SetControlThread(CncExecutor controlThread)
        {
            _cncExecutor = controlThread;
        }

        public void SetHelper(SerialHelper helper)
        {
            _helper = helper;
        }

        public void SetLogger(Logger logger)
        {
            _logger = logger;
        }

        AutocompleteMenu popupMenu;

        readonly List<string> regularWords = new List<string>()
        {
            "MOVE M",
            "SPEED M",
            "STOP M",
            "HOME M",
            "ON U",
            "OFF U"
        };

        private void InitilizeProgramtextBox()
        {
            programTextBox.TextChanged += ProgramTextBox_TextChanged;
            popupMenu = new AutocompleteMenu(programTextBox);
            popupMenu.AllowTabKey = true;
            popupMenu.Items.SetAutocompleteItems(regularWords);
        }

        Style commentsStyle = new TextStyle(Brushes.Green, null, FontStyle.Italic);
        Style motorCommandNameStyle = new TextStyle(Brushes.Red, null, FontStyle.Bold);
        Style simpleCommandNameStyle = new TextStyle(Brushes.Green, null, FontStyle.Bold);
        Style unitCommandNameStyle = new TextStyle(Brushes.DarkOrange, null, FontStyle.Bold);
        Style motorNumberStyle = new TextStyle(Brushes.Blue, null, FontStyle.Bold);
        Style commandsArgumentStyle = new TextStyle(Brushes.Purple, null, FontStyle.Bold);

        Style digitStyle = new TextStyle(Brushes.Green, null, FontStyle.Regular);

        private void ProgramTextBox_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            e.ChangedRange.ClearStyle(motorCommandNameStyle);
            e.ChangedRange.ClearStyle(motorNumberStyle);
            e.ChangedRange.ClearStyle(unitCommandNameStyle);
            e.ChangedRange.ClearStyle(commandsArgumentStyle);
            e.ChangedRange.ClearStyle(simpleCommandNameStyle);
            e.ChangedRange.ClearStyle(digitStyle);

            e.ChangedRange.ClearStyle(commentsStyle);

            e.ChangedRange.SetStyle(motorCommandNameStyle, @"\b(?<range>MOVE|SPEED)\b", RegexOptions.IgnoreCase);
            e.ChangedRange.SetStyle(unitCommandNameStyle, @"\b(?<range>ON|OFF)\b", RegexOptions.IgnoreCase);
            e.ChangedRange.SetStyle(simpleCommandNameStyle, @"\b(?<range>HOME|STOP)\b", RegexOptions.IgnoreCase);
            e.ChangedRange.SetStyle(motorNumberStyle, @"\b(?<range>M|D)(\d)+\b", RegexOptions.IgnoreCase);
            e.ChangedRange.SetStyle(commandsArgumentStyle, @"\b(?<range>S)(-)?(\d)+\b", RegexOptions.IgnoreCase);
            e.ChangedRange.SetStyle(digitStyle, @"\b(M|S)(?<range>(-)?\d+)\b", RegexOptions.IgnoreCase);

            //comment highlighting
            e.ChangedRange.SetStyle(commentsStyle, @"//.*$", RegexOptions.Multiline);
        }

        bool testProgram()
        {
            CncParser parser = new CncParser(_logger);
            _program = parser.Parse(programTextBox.Text);

            _logger.AddMessage($"Программа содержит {_program.Commands.Count} команд.");

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
                    _logger.AddMessage("Ошибка при открытии файла - Файл не найден.");
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
                    _logger.AddMessage("Ошибка при сохранении файла.");
                }
                
            }
        }

        static List<ICommand> commandsToSend = new List<ICommand>();

        void LEDSBlink()
        {
            commandsToSend.Clear();

            ICommand command;

            for (int k = 0; k < 2; k++)
            {
                for (int i = 0; i < 6; i++)
                {
                    command = new SetDeviceStateCommand(i, SetDeviceStateCommand.DeviseState.DEVICE_ON, Protocol.GetPacketId());
                    commandsToSend.Add(command);
                    command = new SetDeviceStateCommand(11 - i, SetDeviceStateCommand.DeviseState.DEVICE_ON, Protocol.GetPacketId());
                    commandsToSend.Add(command);
                }

                for (int i = 6; i >= 0; i--)
                {
                    command = new SetDeviceStateCommand(i, SetDeviceStateCommand.DeviseState.DEVICE_OFF, Protocol.GetPacketId());
                    commandsToSend.Add(command);
                    command = new SetDeviceStateCommand(11 - i, SetDeviceStateCommand.DeviseState.DEVICE_OFF, Protocol.GetPacketId());
                    commandsToSend.Add(command);
                }
            }

            for (int k = 0; k < 2; k++)
            {
                for (int i = 0; i < 12; i++)
                {
                    command = new SetDeviceStateCommand(i, SetDeviceStateCommand.DeviseState.DEVICE_ON, Protocol.GetPacketId());
                    commandsToSend.Add(command);
                }

                for (int i = 12; i >= 0; i--)
                {
                    command = new SetDeviceStateCommand(i, SetDeviceStateCommand.DeviseState.DEVICE_OFF, Protocol.GetPacketId());
                    commandsToSend.Add(command);
                }
            }

            for (int k = 0; k < 2; k++)
            {
                for (int i = 12; i >= 0; i--)
                {
                    command = new SetDeviceStateCommand(i, SetDeviceStateCommand.DeviseState.DEVICE_ON, Protocol.GetPacketId());
                    commandsToSend.Add(command);
                }

                for (int i = 0; i < 12; i++)
                {
                    command = new SetDeviceStateCommand(i, SetDeviceStateCommand.DeviseState.DEVICE_OFF, Protocol.GetPacketId());
                    commandsToSend.Add(command);
                }
            }
        }

        UInt32 HomePose = 1000;
        UInt32 Degrees90 = 6400;

        void Motor0Drive()
        {
            commandsToSend.Clear();
            ICommand command;
            
            for(int  k = 0; k < 20; k++)
            {
                command = new SetSpeedCommand(1, 300, Protocol.GetPacketId());
                commandsToSend.Add(command);

                //home start
                command = new GoUntilCommand(0, Protocol.Direction.REV, 100, Protocol.GetPacketId());
                commandsToSend.Add(command);

                command = new SetSpeedCommand(0, 200, Protocol.GetPacketId());
                commandsToSend.Add(command);

                command = new GoUntilCommand(1, Protocol.Direction.REV, 100, Protocol.GetPacketId());
                commandsToSend.Add(command);

                command = new SetSpeedCommand(1, 200, Protocol.GetPacketId());
                commandsToSend.Add(command);
                // home end

                command = new MoveCommand(0, Protocol.Direction.FWD, HomePose, Protocol.GetPacketId());
                commandsToSend.Add(command);

                command = new MoveCommand(1, Protocol.Direction.FWD, HomePose, Protocol.GetPacketId());
                commandsToSend.Add(command);

                //6400 = 90 degrees

                command = new MoveCommand(0, Protocol.Direction.FWD, Degrees90 * 2, Protocol.GetPacketId());
                commandsToSend.Add(command);

                command = new MoveCommand(0, Protocol.Direction.REV, Degrees90 * 2, Protocol.GetPacketId());
                commandsToSend.Add(command);

                command = new MoveCommand(1, Protocol.Direction.FWD, Degrees90 * 2, Protocol.GetPacketId());
                commandsToSend.Add(command);

                command = new MoveCommand(1, Protocol.Direction.REV, Degrees90 * 2, Protocol.GetPacketId());
                commandsToSend.Add(command);

                for (int i = 0; i < 2; i++)
                {
                    command = new MoveCommand(0, Protocol.Direction.FWD, Degrees90, Protocol.GetPacketId());
                    commandsToSend.Add(command);
                }

                for (int i = 0; i < 2; i++)
                {
                    command = new MoveCommand(1, Protocol.Direction.FWD, Degrees90, Protocol.GetPacketId());
                    commandsToSend.Add(command);
                }

                for (int  i = 0; i < 2; i++)
                {
                    command = new MoveCommand(0, Protocol.Direction.REV, Degrees90, Protocol.GetPacketId());
                    commandsToSend.Add(command);
                }

                for (int i = 0; i < 2; i++)
                {
                    command = new MoveCommand(1, Protocol.Direction.REV, Degrees90, Protocol.GetPacketId());
                    commandsToSend.Add(command);
                }

                for (int i = 0; i < 4; i++)
                {
                    command = new MoveCommand(0, Protocol.Direction.FWD, Degrees90 / 2, Protocol.GetPacketId());
                    commandsToSend.Add(command);
                }

                for (int i = 0; i < 4; i++)
                {
                    command = new MoveCommand(1, Protocol.Direction.FWD, Degrees90 / 2, Protocol.GetPacketId());
                    commandsToSend.Add(command);
                }

                for (int i = 0; i < 4; i++)
                {
                    command = new MoveCommand(0, Protocol.Direction.REV, Degrees90 / 2, Protocol.GetPacketId());
                    commandsToSend.Add(command);
                }

                for (int i = 0; i < 4; i++)
                {
                    command = new MoveCommand(1, Protocol.Direction.REV, Degrees90 / 2, Protocol.GetPacketId());
                    commandsToSend.Add(command);
                }
            }
            

            //command = new MoveCommand(1, Protocol.Direction.REV, 20000, Protocol.GetPacketId());
            //commandsToSend.Add(command);

            //command = new MoveCommand(0, Protocol.Direction.FWD, 20000, Protocol.GetPacketId());
            //commandsToSend.Add(command);
            //command = new MoveCommand(1, Protocol.Direction.REV, 20000, Protocol.GetPacketId());
            //commandsToSend.Add(command);

            //command = new SetSpeedCommand(1, 250, Protocol.GetPacketId());
            //commandsToSend.Add(command);

            //command = new MoveCommand(0, Protocol.Direction.FWD, 20000, Protocol.GetPacketId());
            //commandsToSend.Add(command);
            //command = new MoveCommand(1, Protocol.Direction.REV, 20000, Protocol.GetPacketId());
            //commandsToSend.Add(command);

            //command = new MoveCommand(0, Protocol.Direction.FWD, 20000, Protocol.GetPacketId());
            //commandsToSend.Add(command);
            //command = new MoveCommand(1, Protocol.Direction.REV, 20000, Protocol.GetPacketId());
            //commandsToSend.Add(command);
        }

        private void buttonTestCNCMove_Click(object sender, EventArgs e)
        {
            //LEDSBlink();
            Motor0Drive();
            _cncExecutor.StartExecution(commandsToSend);
        }
        
        private void buttonRunProgram_Click(object sender, EventArgs e)
        {
            if(testProgram())
            {
                _cncExecutor.StartExecution(_program.Commands);
            }
        }

        private void buttonAbortExecution_Click(object sender, EventArgs e)
        {
            _cncExecutor.AbortExecution();
            _helper.SendBytes(new AbortExecutionCommand(Protocol.GetPacketId()).GetBytes());
        }

        private void buttonClearFile_Click(object sender, EventArgs e)
        {
            programTextBox.Clear();
        }
    }
}

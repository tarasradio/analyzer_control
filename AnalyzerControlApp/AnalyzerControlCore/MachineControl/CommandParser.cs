using AnalyzerCommunication;
using AnalyzerCommunication.CommunicationProtocol;
using AnalyzerCommunication.CommunicationProtocol.AdditionalCommands;
using AnalyzerCommunication.CommunicationProtocol.CncCommands;
using AnalyzerControlCore.Utils;
using Infrastructure;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AnalyzerControlCore.MachineControl
{
    public class CncProgram
    {
        public List<ICommand> Commands { get; set; }

        public CncProgram()
        {
            Commands = new List<ICommand>();
        }
    }

    public class CommandParser
    {
        readonly Regex cncCommandPattern = new Regex(@"(MOVE|SPEED|STOP|HOME|ON|OFF|WAITR|WAITF|DELAY|RUN)(\s+\w+(-)?\d+)+", RegexOptions.IgnoreCase);

        readonly Regex moveCommandPattern = new Regex(@"MOVE(\s+\w+\d+)+", RegexOptions.IgnoreCase);
        readonly Regex moveArgumentPattern = new Regex(@"\s+M(?<motor>\d+)\s*S(?<steps>(-)?\d+)", RegexOptions.IgnoreCase);

        readonly Regex speedCommandPattern = new Regex(@"SPEED(\s+\w+\d+)+", RegexOptions.IgnoreCase);
        readonly Regex speedArgumentPattern = new Regex(@"\s+M(?<motor>\d+)\s*S(?<speed>(-)?\d+)", RegexOptions.IgnoreCase);

        readonly Regex stopCommandPattern = new Regex(@"STOP(\s+M\d+)+", RegexOptions.IgnoreCase);
        readonly Regex homeCommandPattern = new Regex(@"HOME(\s+M\d+)+", RegexOptions.IgnoreCase);

        readonly Regex simpleMotorArgumentPattern = new Regex(@"\s+M(?<motor>\d+)", RegexOptions.IgnoreCase);

        readonly Regex onCommandPattern = new Regex(@"ON(\s+D\d+)+", RegexOptions.IgnoreCase);
        readonly Regex offCommandPattern = new Regex(@"OFF(\s+D\d+)+", RegexOptions.IgnoreCase);

        readonly Regex devArgumentPattern = new Regex(@"\s+D(?<device>\d+)", RegexOptions.IgnoreCase);

        readonly Regex delayCommandPattern = new Regex(@"DELAY(\s+\w+\d+)+", RegexOptions.IgnoreCase);
        readonly Regex delayArgumentPattern = new Regex(@"\s+M(?<time>\d+)", RegexOptions.IgnoreCase);

        readonly Regex waitRisingCommandPattern = new Regex(@"WAITR(\s+\w+\d+)+", RegexOptions.IgnoreCase);
        readonly Regex waitFallingCommandPattern = new Regex(@"WAITF(\s+\w+\d+)+", RegexOptions.IgnoreCase);
        readonly Regex waitArgumentPattern = new Regex(@"\s+S(?<sensor>\d+)\s*V(?<value>(-)?\d+)", RegexOptions.IgnoreCase);

        readonly Regex runCommandPattern = new Regex(@"RUN(\s+\w+\d+)+", RegexOptions.IgnoreCase);
        readonly Regex sensorArgumentPattern = new Regex(@"\s+S(?<sensor>\d+)\s*(?<edgeType>R|F)(?<value>(-)?\d+)", RegexOptions.IgnoreCase);

        public CommandParser()
        {

        }
        
        private bool checkMotorNumber(int motor)
        {
            bool isSuccess = true;
            if (motor < 0)
            {
                Logger.Info("[Command parser] - Номер мотора не может быть меньше 0.");
                isSuccess = false; 
            }
            return isSuccess;
        }

        public List<ICommand> Parse(string programText)
        {
            List<ICommand> commands = new List<ICommand>();

            string parsedCommand = "";

            string noComments = RegexHelper.GetNoCommentString(programText);

            foreach (Match commandString in cncCommandPattern.Matches(noComments))
            {
                if (moveCommandPattern.IsMatch(commandString.Value))
                {
                    Dictionary<int, int> arguments = new Dictionary<int, int>();

                    parsedCommand = "MOTOR MOVE {";
                    foreach (Match moveArgStr in moveArgumentPattern.Matches(commandString.Value))
                    {
                        int motor = int.Parse(moveArgStr.Groups["motor"].Value);
                        int steps = int.Parse(moveArgStr.Groups["steps"].Value);

                        checkMotorNumber(motor);
                        arguments[motor] = steps;

                        parsedCommand += $" M{motor} S = {steps}";
                    }

                    commands.Add(new MoveCncCommand(arguments));
                }
                else if (speedCommandPattern.IsMatch(commandString.Value))
                {
                    Dictionary<int, int> arguments = new Dictionary<int, int>();

                    parsedCommand = "SET MOTOR SPEED {";
                    foreach (Match speedArgStr in speedArgumentPattern.Matches(commandString.Value))
                    {
                        int motor = int.Parse(speedArgStr.Groups["motor"].Value);
                        int speed = int.Parse(speedArgStr.Groups["speed"].Value);

                        checkMotorNumber(motor);

                        arguments[motor] = speed;

                        parsedCommand += $" M{motor} S = {speed}";
                    }

                    commands.Add(new SetSpeedCncCommand(arguments));
                }
                else if (stopCommandPattern.IsMatch(commandString.Value))
                {
                    List<int> arguments = new List<int>();

                    parsedCommand = "STOP MOTOR {";
                    foreach (Match motorArgStr in simpleMotorArgumentPattern.Matches(commandString.Value))
                    {
                        int motor = int.Parse(motorArgStr.Groups["motor"].Value);

                        checkMotorNumber(motor);

                        arguments.Add(motor);

                        parsedCommand += $" M{motor}";
                    }

                    // TODO: нужно ли стоп???
                }
                else if (homeCommandPattern.IsMatch(commandString.Value))
                {
                    Dictionary<int, int> arguments = new Dictionary<int, int>();

                    parsedCommand = "HOME MOTOR {";
                    foreach (Match speedArgStr in speedArgumentPattern.Matches(commandString.Value))
                    {
                        int motor = int.Parse(speedArgStr.Groups["motor"].Value);
                        int speed = int.Parse(speedArgStr.Groups["speed"].Value);

                        checkMotorNumber(motor);

                        arguments[motor] = speed;

                        parsedCommand += $" M{motor} S = {speed}";
                    }

                    commands.Add(new HomeCncCommand(arguments));
                }
                else if (onCommandPattern.IsMatch(commandString.Value))
                {
                    List<int> arguments = new List<int>();

                    parsedCommand = "ON DEV {";
                    foreach (Match devArgStr in devArgumentPattern.Matches(commandString.Value))
                    {
                        int device = int.Parse(devArgStr.Groups["device"].Value);

                        if (device < 0)
                        {
                            Logger.Info("[Command parser] - Номер устройства не может быть меньше 0.");
                        }

                        arguments.Add(device);

                        parsedCommand += $" D{device}";
                    }

                    commands.Add(new OnDeviceCncCommand(arguments));
                }
                else if (offCommandPattern.IsMatch(commandString.Value))
                {
                    List<int> arguments = new List<int>();

                    parsedCommand = "OFF DEV {";
                    foreach (Match devArgStr in devArgumentPattern.Matches(commandString.Value))
                    {
                        int device = int.Parse(devArgStr.Groups["device"].Value);

                        if (device < 0)
                        {
                            Logger.Info("[Command parser] - Номер устройства не может быть меньше 0.");
                        }

                        arguments.Add(device);

                        parsedCommand += $" D{device}";
                    }

                    commands.Add(new OffDeviceCncCommand(arguments));
                }
                else if (delayCommandPattern.IsMatch(commandString.Value))
                {
                    int timeMs = -1;
                    parsedCommand = "DELAY {";

                    Match arg = delayArgumentPattern.Match(commandString.Value);

                    if(arg.Success)
                    {
                        timeMs = int.Parse(arg.Groups["time"].Value);

                        if (timeMs < 0)
                        {
                            Logger.Info("[Command parser] - Время задержки не может быть меньше 0.");
                        }

                        parsedCommand += $"time = {timeMs} ms";
                    }

                    commands.Add(new WaitTimeCommand((uint)timeMs));
                }
                else if (runCommandPattern.IsMatch(commandString.Value))
                {
                    Protocol.ValueEdge edgeType = Protocol.ValueEdge.RisingEdge;
                    int sensor = -1;
                    int value = -1;

                    Dictionary<int, int> arguments = new Dictionary<int, int>();

                    parsedCommand = "Run {";

                    foreach (Match speedArgStr in speedArgumentPattern.Matches(commandString.Value))
                    {
                        int motor = int.Parse(speedArgStr.Groups["motor"].Value);
                        int speed = int.Parse(speedArgStr.Groups["speed"].Value);

                        checkMotorNumber(motor);

                        arguments[motor] = speed;

                        parsedCommand += $" M{motor} S = {speed}";
                    }

                    Match arg = sensorArgumentPattern.Match(commandString.Value);

                    if (arg.Success)
                    {
                        edgeType = arg.Groups["edgeType"].Value == "r" ? Protocol.ValueEdge.RisingEdge : Protocol.ValueEdge.FallingEdge;
                        sensor = int.Parse(arg.Groups["sensor"].Value);
                        value = int.Parse(arg.Groups["value"].Value);
                        
                        if (sensor < 0)
                        {
                            Logger.Info("[Command parser] - Номер датчика не может быть меньше 0.");
                        }

                        if (value < 0)
                        {
                            Logger.Info("[Command parser] - Значение датчика не может быть меньше 0.");
                        }

                        parsedCommand += $" | sensor = {sensor}, value = {value}, edge = {edgeType}";
                    }

                    commands.Add(new RunCncCommand(arguments, (uint)sensor, (uint)value, edgeType));
                }
                parsedCommand += "}";
                Logger.Info(parsedCommand);
            }

            Logger.Info($"Программа содержит { commands.Count } команд.");

            return commands;
        }
    }
}

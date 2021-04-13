using AnalyzerCncControl.Regexes;
using AnalyzerCommunication;
using AnalyzerCommunication.CommunicationProtocol.AdditionalCommands;
using AnalyzerCommunication.CommunicationProtocol.CncCommands;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AnalyzerCncControl
{
    public static class CommandsParser
    {
        public static List<ICommand> Parse(string programText)
        {
            List<ICommand> commands = new List<ICommand>();

            string noComments = RegexHelper.GetNoCommentString(programText);

            foreach (Match matchedString in ParserRegexes.CncCommandPattern.Matches(noComments))
            {
                ParseCommand(commands, matchedString);
            }

            Logger.Info($"Программа содержит { commands.Count } команд.");

            return commands;
        }

        private static string ParseCommand(IList<ICommand> commands, Match matchedString)
        {
            string parsedCommand = "";

            if (ParserRegexes.MoveCommandPattern.IsMatch(matchedString.Value))
            {
                parsedCommand = ParseMoveCommand(commands, matchedString);
            }
            else if (ParserRegexes.SpeedCommandPattern.IsMatch(matchedString.Value))
            {
                parsedCommand = ParseSpeedCommand(commands, matchedString);
            }
            else if (ParserRegexes.StopCommandPattern.IsMatch(matchedString.Value))
            {
                parsedCommand = ParseStopCommand(matchedString);
            }
            else if (ParserRegexes.HomeCommandPattern.IsMatch(matchedString.Value))
            {
                parsedCommand = ParseHomeCommand(commands, matchedString);
            }
            else if (ParserRegexes.OnCommandPattern.IsMatch(matchedString.Value))
            {
                parsedCommand = ParseOnDeviceCommand(commands, matchedString);
            }
            else if (ParserRegexes.OffCommandPattern.IsMatch(matchedString.Value))
            {
                parsedCommand = ParseOffDeviceCommand(commands, matchedString);
            }
            else if (ParserRegexes.DelayCommandPattern.IsMatch(matchedString.Value))
            {
                parsedCommand = ParseDelayCommand(commands, matchedString);
            }
            else if (ParserRegexes.RunCommandPattern.IsMatch(matchedString.Value))
            {
                parsedCommand = ParseRunCommand(commands, matchedString);
            }

            parsedCommand += "}";
            Logger.Info(parsedCommand);

            return parsedCommand;
        }

        private static string ParseRunCommand(IList<ICommand> commands, Match matchedString)
        {
            string parsedCommand;
            ValueEdge edgeType = ValueEdge.RisingEdge;
            int sensor = -1;
            int value = -1;

            Dictionary<int, int> arguments = new Dictionary<int, int>();

            parsedCommand = "Run {";
            parsedCommand += ParseSpeedArguments(matchedString, parsedCommand, arguments);

            Match arg = ParserRegexes.SensorArgumentPattern.Match(matchedString.Value);

            if (arg.Success)
            {
                edgeType = arg.Groups["edgeType"].Value == "r" ? ValueEdge.RisingEdge : ValueEdge.FallingEdge;
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
            return parsedCommand;
        }

        private static string ParseDelayCommand(IList<ICommand> commands, Match matchedString)
        {
            string parsedCommand;
            int timeMs = -1;
            parsedCommand = "DELAY {";

            Match arg = ParserRegexes.DelayArgumentPattern.Match(matchedString.Value);

            if (arg.Success)
            {
                timeMs = int.Parse(arg.Groups["time"].Value);

                if (timeMs < 0)
                {
                    Logger.Info("[Command parser] - Время задержки не может быть меньше 0.");
                }

                parsedCommand += $"time = {timeMs} ms";
            }

            commands.Add(new WaitTimeCommand((uint)timeMs));
            return parsedCommand;
        }

        private static string ParseOffDeviceCommand(IList<ICommand> commands, Match matchedString)
        {
            string parsedCommand;
            List<int> arguments = new List<int>();

            parsedCommand = "OFF DEV {";
            parsedCommand += ParseDeviceArguments(matchedString, parsedCommand, arguments);

            commands.Add(new OffDeviceCncCommand(arguments));
            return parsedCommand;
        }

        private static string ParseOnDeviceCommand(IList<ICommand> commands, Match matchedString)
        {
            string parsedCommand;
            List<int> arguments = new List<int>();

            parsedCommand = "ON DEV {";
            parsedCommand += ParseDeviceArguments(matchedString, parsedCommand, arguments);

            commands.Add(new OnDeviceCncCommand(arguments));
            return parsedCommand;
        }

        private static string ParseHomeCommand(IList<ICommand> commands, Match matchedString)
        {
            string parsedCommand;
            Dictionary<int, int> arguments = new Dictionary<int, int>();

            parsedCommand = "HOME MOTOR {";
            parsedCommand += ParseSpeedArguments(matchedString, parsedCommand, arguments);

            commands.Add(new HomeCncCommand(arguments));
            return parsedCommand;
        }

        private static string ParseStopCommand(Match matchedString)
        {
            string parsedCommand;
            List<int> arguments = new List<int>();

            parsedCommand = "STOP MOTOR {";
            foreach (Match motorArgStr in ParserRegexes.SimpleMotorArgumentPattern.Matches(matchedString.Value))
            {
                int motor = int.Parse(motorArgStr.Groups["motor"].Value);

                checkMotorNumber(motor);

                arguments.Add(motor);

                parsedCommand += $" M{motor}";
            }

            return parsedCommand;
        }

        private static string ParseSpeedCommand(IList<ICommand> commands, Match matchedString)
        {
            string parsedCommand;
            Dictionary<int, int> arguments = new Dictionary<int, int>();

            parsedCommand = "SET MOTOR SPEED {";
            parsedCommand += ParseSpeedArguments(matchedString, parsedCommand, arguments);

            commands.Add(new SetSpeedCncCommand(arguments));
            return parsedCommand;
        }

        private static string ParseMoveCommand(IList<ICommand> commands, Match matchedString)
        {
            string parsedCommand;
            Dictionary<int, int> arguments = new Dictionary<int, int>();

            parsedCommand = "MOTOR MOVE {";
            parsedCommand += ParseMoveArguments(matchedString, parsedCommand, arguments);

            commands.Add(new MoveCncCommand(arguments));
            return parsedCommand;
        }

        private static string ParseDeviceArguments(Match matchedString, string parsedCommand, List<int> arguments)
        {
            foreach (Match devArgStr in ParserRegexes.DeviceArgumentPattern.Matches(matchedString.Value))
            {
                int device = int.Parse(devArgStr.Groups["device"].Value);

                if (device < 0)
                {
                    Logger.Info("[Command parser] - Номер устройства не может быть меньше 0.");
                }

                arguments.Add(device);

                parsedCommand += $" D{device}";
            }

            return parsedCommand;
        }

        private static string ParseMoveArguments(Match matchedString, string parsedCommand, Dictionary<int, int> arguments)
        {
            foreach (Match moveArgStr in ParserRegexes.MoveArgumentPattern.Matches(matchedString.Value))
            {
                int motor = int.Parse(moveArgStr.Groups["motor"].Value);
                int steps = int.Parse(moveArgStr.Groups["steps"].Value);

                checkMotorNumber(motor);
                arguments[motor] = steps;

                parsedCommand += $" M{motor} S = {steps}";
            }

            return parsedCommand;
        }

        private static string ParseSpeedArguments(Match matchedString, string parsedCommand, Dictionary<int, int> arguments)
        {
            foreach (Match speedArgStr in ParserRegexes.SpeedArgumentPattern.Matches(matchedString.Value))
            {
                int motor = int.Parse(speedArgStr.Groups["motor"].Value);
                int speed = int.Parse(speedArgStr.Groups["speed"].Value);

                checkMotorNumber(motor);

                arguments[motor] = speed;

                parsedCommand += $" M{motor} S = {speed}";
            }

            return parsedCommand;
        }

        private static bool checkMotorNumber(int motor)
        {
            bool isSuccess = true;
            if (motor < 0)
            {
                Logger.Info("[Command parser] - Номер мотора не может быть меньше 0.");
                isSuccess = false;
            }
            return isSuccess;
        }
    }
}

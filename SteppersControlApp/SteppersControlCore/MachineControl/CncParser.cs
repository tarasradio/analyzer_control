using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;

using SteppersControlCore.CommunicationProtocol.CncCommands;

namespace SteppersControlCore.MachineControl
{
    public class CncParser
    {
        // BEGIN
        // MOVE M1S2000 M2S300 (frame)
        // SPEED M1S100 M2S300 (frame)
        // STOP M1 M2 (frame)
        // HOME M1 M2 (frame)
        // ON D1 D2 (frame)
        // OFF D1 D2 (frame)
        // END

        Logger _logger;

        readonly Regex cncCommandPattern = new Regex(@"(MOVE|SPEED|STOP|HOME|ON|OFF)(\s+\w+(-)?\d+)+", RegexOptions.IgnoreCase);

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

        public CncParser(Logger logger)
        {
            _logger = logger;
        }

        public CncProgram Parse(string programText)
        {
            CncProgram program = new CncProgram();

            string parsedCommand = "";

            foreach (Match commandString in cncCommandPattern.Matches(programText))
            {

                if (moveCommandPattern.IsMatch(commandString.Value))
                {
                    Dictionary<int, int> arguments = new Dictionary<int, int>();

                    parsedCommand = "MOTOR MOVE {";
                    foreach (Match moveArgStr in moveArgumentPattern.Matches(commandString.Value))
                    {
                        int motor = int.Parse(moveArgStr.Groups["motor"].Value);
                        int steps = int.Parse(moveArgStr.Groups["steps"].Value);

                        if(motor < 0)
                        {
                            _logger.AddMessage(" Номер мотора не может быть меньше 0");
                        }
                        arguments[motor] = steps;

                        parsedCommand += $" M{motor} S = {steps}";
                    }

                    program.Commands.Add(new MoveCncCommand(arguments, Core.GetPacketId()));
                }
                else if (speedCommandPattern.IsMatch(commandString.Value))
                {
                    Dictionary<int, int> arguments = new Dictionary<int, int>();

                    parsedCommand = "SET MOTOR SPEED {";
                    foreach (Match speedArgStr in speedArgumentPattern.Matches(commandString.Value))
                    {
                        int motor = int.Parse(speedArgStr.Groups["motor"].Value);
                        int speed = int.Parse(speedArgStr.Groups["speed"].Value);

                        if (motor < 0)
                        {
                            _logger.AddMessage(" Номер мотора не может быть меньше 0");
                        }

                        arguments[motor] = speed;

                        parsedCommand += $" M{motor} S = {speed}";
                    }

                    program.Commands.Add(new SetSpeedCncCommand(arguments, Core.GetPacketId()));
                }
                else if (stopCommandPattern.IsMatch(commandString.Value))
                {
                    List<int> arguments = new List<int>();

                    parsedCommand = "STOP MOTOR {";
                    foreach (Match motorArgStr in simpleMotorArgumentPattern.Matches(commandString.Value))
                    {
                        int motor = int.Parse(motorArgStr.Groups["motor"].Value);

                        if (motor < 0)
                        {
                            _logger.AddMessage(" Номер мотора не может быть меньше 0");
                        }

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

                        if (motor < 0)
                        {
                            _logger.AddMessage(" Номер мотора не может быть меньше 0");
                        }

                        arguments[motor] = speed;

                        parsedCommand += $" M{motor} S = {speed}";
                    }

                    program.Commands.Add(new HomeCncCommand(arguments, Core.GetPacketId()));
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
                            _logger.AddMessage(" Номер устройства не может быть меньше 0");
                        }

                        arguments.Add(device);

                        parsedCommand += $" D{device}";
                    }

                    program.Commands.Add(new OnDeviceCncCommand(arguments, Core.GetPacketId()));
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
                            _logger.AddMessage(" Номер устройства не может быть меньше 0");
                        }

                        arguments.Add(device);

                        parsedCommand += $" D{device}";
                    }

                    program.Commands.Add(new OffDeviceCncCommand(arguments, Core.GetPacketId()));
                }
                parsedCommand += "}";
                _logger.AddMessage(parsedCommand);
            }

            return program;
        }
    }
}

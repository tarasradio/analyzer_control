using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AnalyzerCncControl.Regexes
{
    public static class ParserRegexes
    {
        public static Regex CncCommandPattern = new Regex(@"(MOVE|SPEED|STOP|HOME|ON|OFF|DELAY|RUN)(\s+\w+(-)?\d+)+", RegexOptions.IgnoreCase);

        public static Regex MoveCommandPattern = new Regex(@"MOVE(\s+\w+\d+)+", RegexOptions.IgnoreCase);
        public static Regex MoveArgumentPattern = new Regex(@"\s+M(?<motor>\d+)\s*S(?<steps>(-)?\d+)", RegexOptions.IgnoreCase);

        public static Regex SpeedCommandPattern = new Regex(@"SPEED(\s+\w+\d+)+", RegexOptions.IgnoreCase);
        public static Regex SpeedArgumentPattern = new Regex(@"\s+M(?<motor>\d+)\s*S(?<speed>(-)?\d+)", RegexOptions.IgnoreCase);

        public static Regex StopCommandPattern = new Regex(@"STOP(\s+M\d+)+", RegexOptions.IgnoreCase);
        public static Regex HomeCommandPattern = new Regex(@"HOME(\s+M\d+)+", RegexOptions.IgnoreCase);

        public static Regex SimpleMotorArgumentPattern = new Regex(@"\s+M(?<motor>\d+)", RegexOptions.IgnoreCase);

        public static Regex OnCommandPattern = new Regex(@"ON(\s+D\d+)+", RegexOptions.IgnoreCase);
        public static Regex OffCommandPattern = new Regex(@"OFF(\s+D\d+)+", RegexOptions.IgnoreCase);

        public static Regex DeviceArgumentPattern = new Regex(@"\s+D(?<device>\d+)", RegexOptions.IgnoreCase);

        public static Regex DelayCommandPattern = new Regex(@"DELAY(\s+\w+\d+)+", RegexOptions.IgnoreCase);
        public static Regex DelayArgumentPattern = new Regex(@"\s+M(?<time>\d+)", RegexOptions.IgnoreCase);

        public static Regex WaitArgumentPattern = new Regex(@"\s+S(?<sensor>\d+)\s*V(?<value>(-)?\d+)", RegexOptions.IgnoreCase);

        public static Regex RunCommandPattern = new Regex(@"RUN(\s+\w+\d+)+", RegexOptions.IgnoreCase);
        public static Regex SensorArgumentPattern = new Regex(@"\s+S(?<sensor>\d+)\s*(?<edgeType>R|F)(?<value>(-)?\d+)", RegexOptions.IgnoreCase);
    }
}

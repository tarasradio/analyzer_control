using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerDomain.Services
{
    public class UnsignedFloatStringParcer
    {
        public static double Parse(string number)
        {
            // This is invariant
            NumberFormatInfo format = new NumberFormatInfo();
            // Set the decimal seperator
            format.NumberDecimalSeparator = ".";

            double result = 0;

            if(number.Length >= 2)
            {
                double n_1, n_2 = 0;

                int float_point_position = int.Parse(number[0].ToString());

                string int_str = $"0.{number.Substring(1, 1 + float_point_position)}";

                n_1 = double.Parse(int_str, format) * (Math.Pow(10, float_point_position));
                if(float_point_position + 1 < number.Length) {
                    n_2 = double.Parse($"0.{number.Substring(1 + float_point_position)}", format);
                }

                result = n_1 + n_2;
            }

            return result;
        }
    }
}

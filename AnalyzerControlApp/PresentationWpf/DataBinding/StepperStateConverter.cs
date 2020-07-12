using AnalyzerCommunication.CommunicationProtocol.Responses;
using System;
using System.Globalization;
using System.Windows.Data;

namespace PresentationWPF.DataBinding
{
    [ValueConversion(typeof(ushort), typeof(string))]
    public class StepperStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = "Остановлен";

            if (((ushort)value & (ushort)DriverState.STATUS_MOT_STATUS) == (ushort)StepperState.ACCELERATION)
                result = "Ускорение";
            if (((ushort)value & (ushort)DriverState.STATUS_MOT_STATUS) == (ushort)StepperState.DECELERATION)
                result = "Замедление";
            if (((ushort)value & (ushort)DriverState.STATUS_MOT_STATUS) == (ushort)StepperState.CONSTANT_SPEED)
                result = "В движении";

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

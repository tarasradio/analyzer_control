using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerCommunication
{
    public class AnalyzerContext : IAnalyzerContext
    {
        private object locker = new object();

        private ushort[] sensorsValues;
        private ushort[] steppersStates;

        private string tubeBarcode;
        private string cartridgeBarcode;

        private static string firmwareVersion;

        public AnalyzerContext(int sensorsNumber, int steppersNumber)
        {
            this.sensorsValues = new ushort[sensorsNumber];
            this.steppersStates = new ushort[steppersNumber];
        }

        public string TubeBarcode 
        {
            get
            {
                string barCode;

                lock (locker)
                {
                    barCode = tubeBarcode;
                    tubeBarcode = String.Empty;
                }

                return barCode;
            }
            set
            {
                lock (locker)
                {
                    tubeBarcode = value;
                    Logger.Info($"New tube barcode received: {value}");
                }
            }
        }

        public string CartridgeBarcode 
        { 
            get
            {
                string barCode;

                lock (locker)
                {
                    barCode = cartridgeBarcode;
                    cartridgeBarcode = String.Empty;
                }

                return barCode;
            }
            set
            {
                lock (locker)
                {
                    cartridgeBarcode = value;
                    Logger.Info($"New cartridge barcode received: {value}");
                }
            }
        }

        public ushort[] SensorsValues 
        { 
            get
            {
                ushort[] values = new ushort[sensorsValues.Length];

                lock (locker)
                {
                    Array.Copy(sensorsValues, values, sensorsValues.Length);
                }

                return values;
            }
            set
            {
                if (value.Length != sensorsValues.Length)
                    return;

                lock (locker)
                {
                    Array.Copy(value, sensorsValues, value.Length);
                }
            }
        }

        public ushort[] SteppersStates 
        { 
            get
            {
                ushort[] states = new ushort[steppersStates.Length];

                lock (locker)
                {
                    Array.Copy(steppersStates, states, steppersStates.Length);
                }

                return states;
            }
            set
            {
                if (value.Length != steppersStates.Length)
                    return;

                lock (locker)
                {
                    Array.Copy(value, steppersStates, value.Length);
                }
            }
        }

        public string FirmwareVersion 
        { 
            get
            {
                return firmwareVersion;
            }
            set
            {
                firmwareVersion = value;
            }
        }
    }
}

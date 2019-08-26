using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.Ports;

using SteppersControlCore;
using SteppersControlCore.SerialCommunication;

namespace SteppersControlApp
{
    public class OldSerialHelper
    {
        Logger _logger;
        PackageReceiver _packageReceiver;

        const byte startOfPacket = 0x24;
        const char endOfPacket = '\n';

        public enum PacketsTypes
        {
            STEPPERS_STATES = 0x10,
            LIMITS_SWITCH_STATES = 0x11,
            TEXT_MESSAGE = 0x12,
        }

        SerialPort _serialPort;

        public bool IsOpenConnect { get; set; }

        public delegate void MessageReceived(string message);
        public delegate void StateReceived(int[] states);

        public event MessageReceived messageReceived;
        public event StateReceived steppersStateReceived;
        public event StateReceived limitsSwitchStateReceived;

        public OldSerialHelper(Logger logger, PackageReceiver packageReceiver)
        {
            _logger = logger;
            _packageReceiver = packageReceiver;
            _serialPort = new SerialPort();
            _serialPort.DataReceived += Port_DataReceived;
        }

        public bool OpenConnection(string portName)
        {
            bool isOK = false;

            _serialPort = new System.IO.Ports.SerialPort(portName);
            _serialPort.BaudRate = 115200;
            _serialPort.DataBits = 8;
            _serialPort.DataReceived += Port_DataReceivedLine;

            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;

            _serialPort.NewLine = "\r";

            try
            {
                _serialPort.Open();
            }
            catch(UnauthorizedAccessException)
            {
                Logger.AddMessage($"Ошибка при открытии порта {portName}");
            }
            

            if (_serialPort.IsOpen)
            {
                IsOpenConnect = true;
                isOK = true;
            }

            return isOK;
        }

        public void Disconnect()
        {
            _serialPort.Close();
            IsOpenConnect = false;
        }

        public OldSerialHelper(string portName)
        {
            OpenConnection(portName);
        }

        public void CloseConnection()
        {
            _serialPort.Close();
        }

        public bool IsConnected()
        {
            return _serialPort.IsOpen;
        }

        byte[] buffer = new byte[1024];
        int offset = 0;
        
        List<string> recvStrings = new List<string>();
        int[] recvStates = new int[18];

        enum ParseStates
        {
            INIT_STATE,
            SOP_RECEIVED,
            TOP_RECEIVED,
            EOP_RECEIVED
        };

        int recvPacketType;
        byte[] packetBuffer = new byte[1024];
        int packetCounter = 0;

        private void Port_DataReceivedLine(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] buffer = new byte[_serialPort.BytesToRead];
            _serialPort.Read(buffer, 0, buffer.Length);
            _packageReceiver.HandleData(buffer);

            //try
            //{
            //    string resultMessage = _serialPort.ReadLine();
            //    messageReceived(resultMessage);
            //}
            //catch(TimeoutException )
            //{

            //}

            //int countBytes = _serialPort.BytesToRead;
            //_serialPort.Read(buffer, offset, countBytes);
            //offset += countBytes;

            //int lastByte = 0;

            //int i = 0;

            //packetCounter = 0;

            //while (i != offset)
            //{
            //    if(buffer[i] == '\r')
            //    {
            //        lastByte = i;
            //        string resultMessage = Encoding.ASCII.GetString(packetBuffer, 0, packetCounter);
            //        messageReceived(resultMessage);
            //        packetCounter = 0;
            //    }
            //    else
            //    {
            //        packetBuffer[packetCounter++] = buffer[i];
                    
            //    }
            //    i++;
            //}

            //if (lastByte != 0)
            //{
            //    int k = 0;
            //    for (int j = lastByte; j < offset; j++)
            //    {
            //        buffer[k] = buffer[j]; k++;
            //    }

            //    offset = k;
            //}
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int countBytes = _serialPort.BytesToRead;
            _serialPort.Read(buffer, offset, countBytes);
            offset += countBytes;

            int lastByte = 0;

            int i = 0;

            int currentState = (int)ParseStates.INIT_STATE;

            while (i != offset)
            {
                switch (currentState)
                {
                    case (int)ParseStates.INIT_STATE:
                        {
                            if (buffer[i] == startOfPacket)
                                currentState = (int)ParseStates.SOP_RECEIVED;
                            i++;
                        }
                        break;
                    case (int)ParseStates.SOP_RECEIVED:
                        {
                            currentState = (int)ParseStates.TOP_RECEIVED;
                            recvPacketType = buffer[i];
                            packetCounter = 0;
                            i++;
                        }
                        break;
                    case (int)ParseStates.TOP_RECEIVED:
                        {
                            if (buffer[i] == endOfPacket)
                                currentState = (int)ParseStates.EOP_RECEIVED;
                            else
                                packetBuffer[packetCounter++] = buffer[i];
                            i++;
                        }
                        break;
                    case (int)ParseStates.EOP_RECEIVED:
                        {
                            lastByte = i;
                            switch (recvPacketType)
                            {
                                case (int)PacketsTypes.STEPPERS_STATES:
                                    {
                                        if (packetCounter == 18)
                                        {
                                            for (int k = 0; k < 18; k++)
                                            {
                                                recvStates[k] = packetBuffer[k];
                                            }

                                            steppersStateReceived(recvStates);
                                        }
                                    }
                                    break;
                                case (int)PacketsTypes.LIMITS_SWITCH_STATES:
                                    {
                                        if (packetCounter == 18)
                                        {
                                            for (int k = 0; k < 18; k++)
                                            {
                                                recvStates[k] = packetBuffer[k];
                                            }

                                            limitsSwitchStateReceived(recvStates);
                                        }
                                    }
                                    break;
                                case (int)PacketsTypes.TEXT_MESSAGE:
                                    {
                                        string resultMessage = System.Text.Encoding.UTF8.GetString(packetBuffer, 0, packetCounter);
                                        messageReceived(resultMessage);
                                    }
                                    break;
                            }
                            packetCounter = 0;
                            currentState = (int)ParseStates.INIT_STATE;
                        }
                        break;
                }
            }

            if (lastByte != 0)
            {
                int k = 0;
                for (int j = lastByte; j < offset; j++)
                {
                    buffer[k] = buffer[j]; k++;
                }

                offset = k;
            }
        }
        
        public void SendLine(string line)
        {
            try
            {
                if(_serialPort.IsOpen)
                    _serialPort.Write(line + '\r');
            }
            catch (Exception)
            {
                Logger.AddMessage("Ошибка при попытке записи в порт");
            }
        }

        public void SendBytes(byte[] bytes)
        {
            try
            {
                _serialPort.Write(bytes, 0, bytes.Length);

                while (_serialPort.BytesToWrite != 0);
            }
            catch (Exception)
            {
                Logger.AddMessage("Ошибка при попытке записи в порт");
            }
        }

        public static string[] getPortsList()
        {
            return SerialPort.GetPortNames();
        }

        public bool getOpenPorts(ref List<string> ports)
        {
            List<string> Ports = new List<string>();
            bool available = false;

            foreach (string str in System.IO.Ports.SerialPort.GetPortNames())
            {
                try
                {
                    ports.Add(str);
                    available = true;
                }
                catch (Exception)
                {
                    Logger.AddMessage("Ошибка при сканировании!");
                }
            }

            return available;
        }
    }
}

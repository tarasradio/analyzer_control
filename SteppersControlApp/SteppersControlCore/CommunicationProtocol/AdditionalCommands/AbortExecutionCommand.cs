﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteppersControlCore.CommunicationProtocol.AdditionalCommands
{
    public class AbortExecutionCommand : AbstractCommand, IDeviceCommand, IAbstractCommand
    {
        public AbortExecutionCommand() : base()
        {

        }

        public byte[] GetBytes()
        {
            SendPacket packet = new SendPacket(1);
            packet.SetPacketId(_commandId);

            packet.SetData(0, (byte)Protocol.AdditionalCommands.ABORT_EXECUTION);

            return packet.GetBytes();
        }

        public new Protocol.CommandTypes GetType()
        {
            return Protocol.CommandTypes.SIMPLE_COMMAND;
        }
    }
}

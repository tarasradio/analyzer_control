#include "packet_manager.hpp"
#include "protocol.hpp"

uint8_t _incomingBuffer[64];
uint8_t _currentBufferByte = 0;

uint8_t _packetBuffer[64];

enum ReceiveState
{
    RECEIVING_HEADER,
    RECEIVING_BODY
};

PacketManager::PacketManager(CommandExecutor2 & commandExecutor)
{
    _commandExecutor = commandExecutor;
}

void PacketManager::ReadPacket()
{
    _currentBufferByte = 0;
    uint8_t countBytes = Serial.available();
    if (countBytes > 0)
    {
        while (countBytes != 0)
        {
            _incomingBuffer[_currentBufferByte++] = Serial.read();
            countBytes--;
        }
    }
}

void PacketManager::FindPacket()
{
    uint8_t i = 0;

    uint8_t state = RECEIVING_HEADER;
    uint8_t currentHeaderByte = 0;
    uint8_t currentEndByte = 0;
    uint8_t currentPacketByte = 0;

    while (i != _currentBufferByte)
    {
        switch (state)
        {
        case RECEIVING_HEADER:
        {
            if (_incomingBuffer[i] == packetHeader[currentHeaderByte])
                currentHeaderByte++;
            else
                currentHeaderByte = 0;

            if (currentHeaderByte == packetHeaderLength)
            {
                state = RECEIVING_BODY;
                currentEndByte = 0;
                currentPacketByte = 0;
            }
        }
        break;
        case RECEIVING_BODY:
        {
            _packetBuffer[currentPacketByte++] = _incomingBuffer[i];

            if (_incomingBuffer[i] == packetEnd[currentEndByte])
                currentEndByte++;
            else
                currentEndByte = 0;

            if (currentEndByte == packetEndLength)
            {
                currentHeaderByte = 0;
                state = RECEIVING_HEADER;

                uint8_t packetLength = currentPacketByte - packetEndLength;

                _commandExecutor.ExecuteCommand(_packetBuffer, packetLength);
            }
        }
        break;
        }
        i++;
    }
}
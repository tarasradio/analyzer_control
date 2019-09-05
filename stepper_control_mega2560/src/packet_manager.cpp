#include "packet_manager.hpp"
#include "protocol.hpp"

#define PACKET_SIZE 64

static uint8_t buffer[PACKET_SIZE * 2];
static uint8_t bufferTail = 0;

uint8_t packetBuffer[PACKET_SIZE];

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
    bufferTail = 0;
    uint8_t countBytes = Serial.available();
    if (countBytes > 0)
    {
        while (countBytes != 0)
        {
            buffer[bufferTail++] = Serial.read();
            countBytes--;
        }
    }
}

const uint8_t EscSymbol = 0x7D;
const uint8_t FlagSymbol = 0xDD;

static bool escapeFlag = false;

static uint8_t packetTail = 0;

void PacketManager::tryPacketBuild(uint8_t bufferPosition)
{
    packetBuffer[packetTail++] = buffer[bufferPosition];

    if(packetTail == PACKET_SIZE)
    {
#ifdef DEBUG
        String message = "Packet size overflow";
        printMessage(message);
#endif
        packetTail = 0;
    }
    escapeFlag = false;
}

void PacketManager::findByteStuffingPacket()
{
    uint8_t position = 0;

    while(position < bufferTail)
    {
        if(FlagSymbol == buffer[position])
        {
            if(escapeFlag)
            {
                tryPacketBuild(position);
            }
            else
            {
                _commandExecutor.ExecuteCommand(packetBuffer, packetTail-1);
                packetTail = 0;
            }
        }
        else if(EscSymbol == buffer[position])
        {
            if(escapeFlag)
            {
                tryPacketBuild(position);
            }
            else
            {
                escapeFlag = true;
            }
        }
        else
        {
            tryPacketBuild(position);
        }
        position++;
    }
}

void PacketManager::FindPacket()
{
    uint8_t i = 0;

    uint8_t state = RECEIVING_HEADER;
    uint8_t currentHeaderByte = 0;
    uint8_t currentEndByte = 0;
    uint8_t currentPacketByte = 0;

    while (i != bufferTail)
    {
        if(RECEIVING_HEADER == state)
        {
            if (buffer[i] == packetHeader[currentHeaderByte])
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
        else if(RECEIVING_BODY == state)
        {
            packetBuffer[currentPacketByte++] = buffer[i];

            if (buffer[i] == packetEnd[currentEndByte])
                currentEndByte++;
            else
                currentEndByte = 0;

            if (currentEndByte == packetEndLength)
            {
                currentHeaderByte = 0;
                state = RECEIVING_HEADER;

                uint8_t packetLength = currentPacketByte - packetEndLength;

                _commandExecutor.ExecuteCommand(packetBuffer, packetLength);
            }
        }
        i++;
    }
}

void PacketManager::printMessage(const String & messageText)
{
    Serial.write(packetHeader, packetHeaderLength);
    Serial.write(TEXT_MESSAGE);
    Serial.println(messageText);
    Serial.write(packetEnd, packetEndLength);
}

void PacketManager::printBarCode(const String & barCode)
{
    Serial.write(packetHeader, packetHeaderLength);
    Serial.write(BAR_CODE_MESSAGE);
    Serial.println(barCode);
    Serial.write(packetEnd, packetEndLength);
}
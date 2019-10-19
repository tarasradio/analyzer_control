#include "packet_manager.hpp"
#include "protocol.hpp"

#define PACKET_SIZE 64

static uint8_t buffer[PACKET_SIZE * 2];
static uint8_t bufferTail = 0;

uint8_t packetBuffer[PACKET_SIZE];

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
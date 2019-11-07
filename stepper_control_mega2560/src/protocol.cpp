#include "protocol.hpp"
#include "packet_manager.hpp"

void Protocol::SendFirmwareVersion(const char* version)
{
    PacketManager::WritePacketFlag();
    PacketManager::WritePacketData(FIRMWARE_VERSION);
    PacketManager::WritePacketData( 
        reinterpret_cast<const uint8_t *> (version), strlen(version) );
    PacketManager::WritePacketFlag();
}

void Protocol::SendMessage(const char* message)
{
    PacketManager::WritePacketFlag();
    PacketManager::WritePacketData(TEXT_MESSAGE);
    PacketManager::WritePacketData( 
        reinterpret_cast<const uint8_t *> (message), strlen(message) );
    PacketManager::WritePacketFlag();
}

void Protocol::SendBarCode(const char* barCode)
{
    PacketManager::WritePacketFlag();
    PacketManager::WritePacketData(BAR_CODE_MESSAGE);
    PacketManager::WritePacketData( 
        reinterpret_cast<const uint8_t *> (barCode), strlen(barCode) );
    PacketManager::WritePacketFlag();
}

void Protocol::SendSteppersStates(const uint16_t *steppersStates, uint8_t steppersCount)
{
    PacketManager::WritePacketFlag();
    PacketManager::WritePacketData(STEPPERS_STATES_MESSAGE);
    PacketManager::WritePacketData(
        reinterpret_cast<const uint8_t *> (steppersStates), steppersCount * sizeof(uint16_t));
    PacketManager::WritePacketFlag();
}

void Protocol::SendSensorsValues(const uint16_t *sensorsValues, uint8_t sensorsCount)
{
    PacketManager::WritePacketFlag();
    PacketManager::WritePacketData(SENSORS_VALUES_MESSAGE);
    PacketManager::WritePacketData(
        reinterpret_cast<const uint8_t *> (sensorsValues), sensorsCount * sizeof(uint16_t));

    PacketManager::WritePacketFlag();
}

void Protocol::SendCommandState(const uint32_t* commandId, uint8_t commandState)
{
    PacketManager::WritePacketFlag();
    PacketManager::WritePacketData(COMMAND_STATE_MESSAGE);
    PacketManager::WritePacketData(commandState);
    PacketManager::WritePacketData(
        reinterpret_cast<const uint8_t *> (commandId), sizeof(uint32_t));
        
    PacketManager::WritePacketFlag();
}
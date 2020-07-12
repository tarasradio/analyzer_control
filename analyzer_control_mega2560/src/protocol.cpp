#include "protocol.hpp"
#include "packet_manager.hpp"

void Protocol::sendFirmwareVersion(const char* version)
{
    PacketManager::writePacketFlag();
    PacketManager::writePacketData(FIRMWARE_VERSION);
    PacketManager::writePacketData( 
        reinterpret_cast<const uint8_t *> (version), strlen(version) );
    PacketManager::writePacketFlag();
}

void Protocol::sendMessage(const char* message)
{
    PacketManager::writePacketFlag();
    PacketManager::writePacketData(TEXT_MESSAGE);
    PacketManager::writePacketData( 
        reinterpret_cast<const uint8_t *> (message), strlen(message) );
    PacketManager::writePacketFlag();
}

void Protocol::sendBarCode(uint8_t id, const char* barCode)
{
    PacketManager::writePacketFlag();
    PacketManager::writePacketData(BAR_CODE_MESSAGE);
    PacketManager::writePacketData(id);
    PacketManager::writePacketData( 
        reinterpret_cast<const uint8_t *> (barCode), strlen(barCode) );
    PacketManager::writePacketFlag();
}

void Protocol::sendSteppersStates(const uint16_t *steppersStates, uint8_t steppersCount)
{
    PacketManager::writePacketFlag();
    PacketManager::writePacketData(STEPPERS_STATES_MESSAGE);
    PacketManager::writePacketData(
        reinterpret_cast<const uint8_t *> (steppersStates), steppersCount * sizeof(uint16_t));
    PacketManager::writePacketFlag();
}

void Protocol::sendSensorsValues(const uint16_t *sensorsValues, uint8_t sensorsCount)
{
    PacketManager::writePacketFlag();
    PacketManager::writePacketData(SENSORS_VALUES_MESSAGE);
    PacketManager::writePacketData(
        reinterpret_cast<const uint8_t *> (sensorsValues), sensorsCount * sizeof(uint16_t));

    PacketManager::writePacketFlag();
}

void Protocol::sendCommandState(const uint32_t* commandId, uint8_t commandState)
{
    PacketManager::writePacketFlag();
    PacketManager::writePacketData(COMMAND_STATE_MESSAGE);
    PacketManager::writePacketData(commandState);
    PacketManager::writePacketData(
        reinterpret_cast<const uint8_t *> (commandId), sizeof(uint32_t));
        
    PacketManager::writePacketFlag();
}
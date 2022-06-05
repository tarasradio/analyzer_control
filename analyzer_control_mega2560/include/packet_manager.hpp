#ifndef packet_manager_hpp
#define packet_manager_hpp

#include <Arduino.h>

class IPacketListener
{
public:
    virtual void listenPacket(uint8_t *packet, uint16_t packetLength) = 0;
};

class PacketManager
{
private:
    IPacketListener *_listener;
public:
    PacketManager(IPacketListener * );
    void readPacket();

    static void writePacketData(uint8_t byte);
    static void writePacketData(uint8_t const *bytes, uint16_t bytesNumber);
    static void writePacketFlag();

    void tryPacketBuild(uint16_t bufferPosition);
    void findByteStuffingPacket();
};

#endif


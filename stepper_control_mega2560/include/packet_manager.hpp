#ifndef packet_manager_hpp
#define packet_manager_hpp

#include <Arduino.h>
#include "command_executor2.hpp"

class PacketManager
{
private:
    CommandExecutor2 _commandExecutor;
public:
    PacketManager(CommandExecutor2 & );
    void ReadPacket();
    void FindPacket();
    static void printMessage(String messageText);
};

#endif
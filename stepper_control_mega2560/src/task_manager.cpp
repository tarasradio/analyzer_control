#include "task_manager.hpp"
#include "packet_manager.hpp"
#include "command_executor2.hpp"

CommandExecutor2 _commandExecutor = CommandExecutor2();
PacketManager _packetManager = PacketManager(_commandExecutor);

TaskManager::TaskManager()
{
    _commandExecutor = CommandExecutor2();
    _packetManager = PacketManager(_commandExecutor);
}

void TaskManager::TaskLoop()
{
    _packetManager.ReadPacket();
    _packetManager.FindPacket();
    _commandExecutor.UpdateState();
}
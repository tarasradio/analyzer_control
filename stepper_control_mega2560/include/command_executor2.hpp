#ifndef command_executor2_hpp
#define command_executor2_hpp

#include <Arduino.h>

class CommandExecutor2
{
private:
    bool checkStepper(uint8_t stepper);
    void printCommandStateResponse(uint32_t commandId, uint8_t commandState);
    void printSteppersStates();
    void printSensorsValues();

    uint8_t getSteppersInMove();

    uint32_t readLong(uint8_t *buffer);
    uint16_t readInt(uint8_t *buffer);

    void addCommandForWait(uint32_t commandId);

    void executeAbortCommand(uint8_t *packet, uint32_t packetId);
    void executeBarStartCommand(uint8_t *packet, uint32_t packetId);
    void executeHomeCommand(uint8_t *packet, uint32_t packetId);
    void executeRunCommand(uint8_t *packet, uint32_t packetId);
    void executeMoveCommand(uint8_t *packet, uint32_t packetId);
    void executeStopCommand(uint8_t *packet, uint32_t packetId);
    void executeSetSpeedCommand(uint8_t *packet, uint32_t packetId);
    void executeSetDeviceStateCommand(uint8_t *packet, uint32_t packetId);

    void executeCncMoveCommand(uint8_t *packet, uint32_t packetId);
    void executeCncRunCommand(uint8_t *packet, uint32_t packetId);
    void executeCncHomeCommand(uint8_t *packet, uint32_t packetId);
    void executeCncSetSpeedCommand(uint8_t *packet, uint32_t packetId);
    void executeCncSetDeviceStateCommand(uint8_t *packet, uint32_t packetId, uint8_t state);

    bool checkRepeatCommand(uint32_t commandId, uint8_t commandType);
    
public:
    CommandExecutor2();
    void UpdateState();
    void ExecuteCommand(uint8_t *packet, uint8_t packetLength);
};

#endif
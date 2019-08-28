#ifndef command_executor2_hpp
#define command_executor2_hpp

#include <Arduino.h>

class CommandExecutor2
{
private:
    bool checkStepper(uint8_t stepper);
    void printCommandStateResponse(uint32_t commandId, uint8_t commandState);
    void printMessage(String messageText);
    void printSteppersStates();
    void printSensorsValues();

    uint8_t getSteppersInHoming();
    uint8_t getSteppersInMove();
    uint8_t getStepperMoveState(uint8_t stepper);

    uint32_t readLong(uint8_t *buffer);
    uint16_t readInt(uint8_t *buffer);

    void addCommandForWait(uint32_t commandId);

    void executeWaitTimeCommand(uint8_t *packet, uint8_t packetLength);
    void executeAbortCommand(uint8_t *packet, uint8_t packetLength);
    void executeHomeCommand(uint8_t *packet, uint8_t packetLength);
    void executeRunCommand(uint8_t *packet, uint8_t packetLength);
    void executeMoveCommand(uint8_t *packet, uint8_t packetLength);
    void executeStopCommand(uint8_t *packet, uint8_t packetLength);
    void executeSetSpeedCommand(uint8_t *packet, uint8_t packetLength);
    void executeSetDeviceStateCommand(uint8_t *packet, uint8_t packetLength);

    void executeCncMoveCommand(uint8_t *packet, uint8_t packetLength);
    void executeCncRunCommand(uint8_t *packet, uint8_t packetLength);
    void executeCncHomeCommand(uint8_t *packet, uint8_t packetLength);
    void executeCncSetSpeedCommand(uint8_t *packet, uint8_t packetLength);
    void executeCncSetDeviceStateCommand(uint8_t *packet, uint8_t packetLength, uint8_t state);

    bool checkSameCommand(uint32_t commandId, uint8_t commandType);
public:
    CommandExecutor2();
    void UpdateState();
    void ExecuteCommand(uint8_t *packet, uint8_t packetLength);
};

#endif
#ifndef command_executor_hpp
#define command_executor_hpp

#include <Arduino.h>

#include "homing_controller.hpp"
#include "moving_controller.hpp"
#include "running_controller.hpp"
#include "barcode_scanner.hpp"

#include "packet_manager.hpp"

class CommandExecutor : public IPacketListener
{
private:
    bool checkStepper(uint8_t stepper);
    
    void sendSteppersStates();
    void sendSensorsValues();

    uint8_t getSteppersInMove();

    uint32_t readLong(uint8_t *buffer);
    uint16_t readInt(uint8_t *buffer);

    void addCommandForWait(uint32_t commandId);

    void executeGetFirmwareVersionCommand(uint8_t *packet, uint32_t packetId);

    void executeAbortCommand(uint8_t *packet, uint32_t packetId);
    void executeBarcodeScanCommand(uint8_t *packet, uint32_t packetId);
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

    HomingController * homingController;
    RunningController * runningController;
    MovingController * movingController;
    BarcodeScanner * tubeScanner;
    BarcodeScanner * cartridgeScanner;
    
public:
    CommandExecutor() {}
    CommandExecutor( 
        HomingController * homingController,
        RunningController * runningController,
        MovingController * movingController,
        BarcodeScanner * tubeScanner,
        BarcodeScanner * cartridgeScanner);

    void updateState();
    void listenPacket(uint8_t *packet, uint8_t packetLength);
};

#endif
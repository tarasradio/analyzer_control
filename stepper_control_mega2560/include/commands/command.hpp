#ifndef command_hpp
#define command_hpp

#include <Arduino.h>

#include "homing_controller.hpp"
#include "moving_controller.hpp"
#include "running_controller.hpp"

#include "bar_scanner.hpp"

class Command
{
public:
    Command() {}
    void Execute(uint8_t *packet, uint32_t packetId);

protected:
    uint32_t static readLong(uint8_t *buffer);
    uint16_t static readInt(uint8_t *buffer);
    bool static checkStepper(uint8_t stepper);
private:
    uint8_t _commandType;
    uint8_t _commandCode;
};

#endif
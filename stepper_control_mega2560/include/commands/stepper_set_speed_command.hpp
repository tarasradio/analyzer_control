#ifndef stepper_set_speed_command_hpp
#define stepper_set_speed_command_hpp

#include "command.hpp"

class StepperSetSpeedCommand : public Command
{
public:
    StepperSetSpeedCommand();
    void Execute(uint8_t *packet, uint32_t packetId);
};

#endif
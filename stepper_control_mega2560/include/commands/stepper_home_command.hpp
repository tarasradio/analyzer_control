#ifndef stepper_home_command_hpp
#define stepper_home_command_hpp

#include "command.hpp"

class StepperHomeCommand : public Command
{
public:
    StepperHomeCommand();
    void Execute(uint8_t *packet, uint32_t packetId);
};

#endif
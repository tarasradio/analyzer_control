#ifndef stepper_run_command_hpp
#define stepper_run_command_hpp

#include "command.hpp"

class StepperRunCommand : public Command
{
public:
    StepperRunCommand();
    void Execute(uint8_t *packet, uint32_t packetId);
};

#endif
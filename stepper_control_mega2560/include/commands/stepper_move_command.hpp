#ifndef stepper_move_command_hpp
#define stepper_move_command_hpp

#include "command.hpp"

class StepperMoveCommand : public Command
{
public:
    StepperMoveCommand();
    void Execute(uint8_t *packet, uint32_t packetId);
};

#endif

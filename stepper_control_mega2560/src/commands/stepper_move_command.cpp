#include "commands/stepper_move_command.hpp"

StepperMoveCommand::StepperMoveCommand() : Command()
{

}

void StepperMoveCommand::Execute(uint8_t *packet, uint32_t packetId)
{
    //if(checkRepeatCommand(packetId, WAITING_COMMAND)) return;

    int8_t stepper = packet[0];
    int32_t steps = readLong(packet + 1);

    if(!checkStepper(stepper)) return;

    MovingController::addStepperForMove(stepper, steps);

#ifdef DEBUG
    {
        String message = "[Move] ";
        message += "stepper = " + String(stepper);
        message += ", steps = " + String(steps);

        PacketManager::printMessage(message);
    }
#endif
}
#include "commands/stepper_home_command.hpp"

StepperHomeCommand::StepperHomeCommand() : Command()
{

}

void StepperHomeCommand::Execute(uint8_t *packet, uint32_t packetId)
{
    //if(checkRepeatCommand(packetId, WAITING_COMMAND)) return;
    
    int8_t stepper = packet[0];
    int32_t fullSpeed = readLong(packet + 1);

    if(!checkStepper(stepper)) return;

    HomingController::clearState();
    HomingController::addStepperForHoming(stepper, fullSpeed);

#ifdef DEBUG
    {
        String message = "[Home] ";
        message += "stepper = " + String(stepper);
        message += ", spd = " + String(fullSpeed);

        PacketManager::printMessage(message);
    }
#endif
}
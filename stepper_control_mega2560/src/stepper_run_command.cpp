#include "commands/stepper_run_command.hpp"

StepperRunCommand::StepperRunCommand() : Command()
{

}

void StepperRunCommand::Execute(uint8_t *packet, uint32_t packetId)
{
    //if(checkRepeatCommand(packetId, WAITING_COMMAND)) return;

    int8_t stepper = packet[0];
    int32_t fullSpeed = readLong(packet + 1);

    if(!checkStepper(stepper)) return;

    Steppers::get(stepper).run(fullSpeed > 0 ? 1 : 0, fullSpeed);

#ifdef DEBUG
    {
        String message = "[Run] ";
        message += "stepper = " + String(stepper);
        message += ", speed = " + String(fullSpeed);

        PacketManager::printMessage(message);
    }
#endif
}
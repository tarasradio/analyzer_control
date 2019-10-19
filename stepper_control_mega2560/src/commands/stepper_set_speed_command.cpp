#include "commands/stepper_set_speed_command.hpp"

#include "commands/stepper_move_command.hpp"

StepperSetSpeedCommand::StepperSetSpeedCommand() : Command()
{

}

void StepperSetSpeedCommand::Execute(uint8_t *packet, uint32_t packetId)
{
    
    //if(checkRepeatCommand(packetId, SIMPLE_COMMAND)) return;

    uint8_t stepper = packet[0];
    uint32_t fullSpeed = readLong(packet + 1);

    if(!checkStepper(stepper)) return;

    Steppers::get(stepper).setMaxSpeed(fullSpeed << 2);
    Steppers::get(stepper).setFullSpeed(fullSpeed);

#ifdef DEBUG
    {
        String message = "[Set speed] ";
        message += "stepper = " + String(stepper);
        message += ", spd = " + String(fullSpeed);

        PacketManager::printMessage(message);
    }
#endif
}
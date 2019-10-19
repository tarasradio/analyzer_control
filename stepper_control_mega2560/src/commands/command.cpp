#include "commands/command.hpp"

uint32_t Command::readLong(uint8_t *buffer)
{
    return *((unsigned long *)(buffer));
}

uint16_t Command::readInt(uint8_t *buffer)
{
    return *((unsigned int *)(buffer));
}

bool Command::checkStepper(uint8_t stepper)
{
    bool result = (stepper >= 0 && stepper < STEPPERS_COUNT) ? true : false;
    if(!result)
    {
        String message = "wrong stepper = " + String(stepper);
        PacketManager::printMessage(message);
    }
    else
    {
        Steppers::get(stepper).setMinSpeed(5);
    }
    
    return result;
}
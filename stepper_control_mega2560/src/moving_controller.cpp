#include "system.hpp"
#include "emulator.hpp"
#include "moving_controller.hpp"

uint8_t countMoveSteppers = 0;
uint8_t steppersForMove[STEPPERS_COUNT];

MovingController::MovingController()
{
    
}

uint8_t MovingController::getSteppersInMoving()
{
    uint8_t steppersInMove = 0;

    for (int i = 0; i < countMoveSteppers; i++)
    {
        uint8_t stepper = steppersForMove[i];

        if (0 != Steppers::getMoveState(stepper))
            steppersInMove++;
    }

    return steppersInMove;
}

void MovingController::addStepperForMove(int8_t stepper, int32_t steps)
{
    steppersForMove[countMoveSteppers++] = stepper;
    int8_t dir = steps > 0 ? 1 : 0;
    Steppers::get(stepper).move(dir, abs(steps));
}

uint8_t MovingController::updateState()
{

#ifdef EMULATOR

    if(Emulator::taskIsRunning() && countMoveSteppers > 0)
    {
#ifdef DEBUG
        {
            String message = "elapsed time = " + String(Emulator::GetElapsedMilliseconds());
            Protocol::sendMessage(message.c_str());
        }
#endif
        if(Emulator::getElapsedMilliseconds() >= MOVE_DELAY)
        {
#ifdef DEBUG
        {
            String message = "Stop task";
            Protocol::sendMessage(message.c_str());
        }
#endif
            Emulator::stopTask();
            countMoveSteppers = 0;
        }
    }
    return countMoveSteppers;
#endif
    
    if(0 == getSteppersInMoving())
    {
        countMoveSteppers = 0;
    }

    return countMoveSteppers;
}

void MovingController::clearState()
{
    countMoveSteppers = 0;
}
#include "moving_controller.hpp"

uint8_t countMoveSteppers = 0;
uint8_t steppersForMove[STEPPERS_COUNT];

#define EMULATOR

MovingController::MovingController()
{
    
}

uint8_t MovingController::getSteppersInMoving()
{
#ifdef EMULATOR
    return 0; // All steppers have been done move
#endif
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
    Steppers::get(stepper).move(dir, steps);
}

uint8_t MovingController::updateState()
{
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
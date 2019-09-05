#include "moving_controller.hpp"

#include "packet_manager.hpp"

#include "protocol.hpp"
#include "steppers.hpp"

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

void MovingController::addStepperForMove(uint8_t stepper, uint8_t direction, uint32_t steps)
{
    steppersForMove[countMoveSteppers++] = stepper;
    Steppers::get(stepper).move(direction, steps);
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
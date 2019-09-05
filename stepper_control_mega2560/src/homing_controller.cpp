#include "homing_controller.hpp"

#include "packet_manager.hpp"

#include "protocol.hpp"
#include "steppers.hpp"

enum HomingState
{
    HOMING_SUCCESS = 0x00,
    WAIT_SW_PRESSED,
    WAIT_SW_RELEASED
};

uint8_t countHomeSteppers = 0;

struct HomingSteppersParams {
    uint8_t stepper;
    uint8_t state;
    uint8_t direction;
    uint32_t speed;
} homingSteppers[STEPPERS_COUNT];

HomingController::HomingController()
{
    
}

uint8_t HomingController::getSteppersInHoming()
{
    uint8_t steppersInHoming = 0;

    for (int i = 0; i < countHomeSteppers; i++)
    {
        uint8_t stepper = homingSteppers[i].stepper;
        if(WAIT_SW_PRESSED == homingSteppers[i].state)
        {
            if (0 != Steppers::getMoveState(stepper))
                steppersInHoming++;
            else
                homingSteppers[i].state = HOMING_SUCCESS;
        }
        else if(WAIT_SW_RELEASED == homingSteppers[i].state)
        {
            steppersInHoming++;
            if (0 == Steppers::getMoveState(stepper))
            {
                Steppers::get(stepper).goUntil(RESET_ABSPOS, homingSteppers[i].direction, homingSteppers[i].speed);
                homingSteppers[i].state = WAIT_SW_PRESSED;
            }
        }
    }

    return steppersInHoming;
}

void HomingController::addStepperForHoming(uint8_t stepper, uint8_t direction, uint32_t speed)
{
    homingSteppers[countHomeSteppers].stepper = stepper;
    homingSteppers[countHomeSteppers].direction = direction;
    homingSteppers[countHomeSteppers].speed = speed;

    uint8_t sw_status = Steppers::get(stepper).getStatus() & STATUS_SW_F;
    if (0 == sw_status)
    {
        homingSteppers[countHomeSteppers].state = WAIT_SW_PRESSED;

        Steppers::get(stepper).goUntil(RESET_ABSPOS, direction, speed);
    }
    else
    {
        homingSteppers[countHomeSteppers].state = WAIT_SW_RELEASED;

        uint8_t inverseDir = (direction == FWD) ? REV : FWD;
        Steppers::get(stepper).setMinSpeed(30); //TODO: пофиксить
        Steppers::get(stepper).releaseSw(RESET_ABSPOS, inverseDir); // настроить MIN SPEED
    }

    countHomeSteppers++;
}

uint8_t HomingController::updateState()
{
    if(0 == getSteppersInHoming())
    {
        countHomeSteppers = 0;
    }
    return countHomeSteppers;
}

void HomingController::clearState()
{
    countHomeSteppers = 0;
}


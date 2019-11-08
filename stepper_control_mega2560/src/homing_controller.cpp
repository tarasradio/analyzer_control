#include "system.hpp"

#include "homing_controller.hpp"

#include "packet_manager.hpp"

#include "protocol.hpp"
#include "steppers.hpp"

enum HomingState
{
    HOMING_SUCCESS = 0x00,
    WAIT_SW_PRESSED
};

uint8_t countHomeSteppers = 0;

struct HomingSteppersParams
{
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
        if (WAIT_SW_PRESSED == homingSteppers[i].state)
        {
            if (0 != Steppers::getMoveState(stepper))
                steppersInHoming++;
            else
                homingSteppers[i].state = HOMING_SUCCESS;
        }
    }
    
    return steppersInHoming;
}

void HomingController::addStepperForHoming(int8_t stepper, int32_t speed)
{
    int8_t dir = speed > 0 ? 1 : 0;
    homingSteppers[countHomeSteppers].stepper = stepper;
    homingSteppers[countHomeSteppers].direction = dir;
    homingSteppers[countHomeSteppers].speed = abs(speed);

    uint8_t sw_status = Steppers::get(stepper).getStatus() & STATUS_SW_F;
    if (0 == sw_status)
    {
        homingSteppers[countHomeSteppers].state = WAIT_SW_PRESSED;
        Steppers::get(stepper).goUntil(RESET_ABSPOS, dir, abs(speed));
    }
    else
    {
        return;
    }

    countHomeSteppers++;
}

uint8_t HomingController::updateState()
{
    #ifdef EMULATOR
        return 0;
    #endif
    
    if (0 == getSteppersInHoming())
    {
        countHomeSteppers = 0;
    }
    return countHomeSteppers;
}

void HomingController::clearState()
{
    countHomeSteppers = 0;
}

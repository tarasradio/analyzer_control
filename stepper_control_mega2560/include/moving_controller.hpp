#ifndef moving_controller_hpp
#define moving_controller_hpp

#include <Arduino.h>

#include "packet_manager.hpp"

#include "protocol.hpp"
#include "steppers.hpp"

class MovingController
{
private:
    static uint8_t getSteppersInMoving();
public:
    MovingController();
    static void addStepperForMove(int8_t stepper, int32_t steps);
    static uint8_t updateState();
    static void clearState();
};

#endif
#ifndef homing_controller_hpp
#define homing_controller_hpp

#include <Arduino.h>

class HomingController
{
private:
    static uint8_t getSteppersInHoming();
public:
    HomingController();
    static void addStepperForHoming(int8_t stepper, int32_t speed);
    static uint8_t updateState();
    static void clearState();
};

#endif
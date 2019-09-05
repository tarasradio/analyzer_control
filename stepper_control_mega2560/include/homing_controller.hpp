#ifndef homing_controller_hpp
#define homing_controller_hpp

#include <Arduino.h>

class HomingController
{
private:
    uint8_t getSteppersInHoming();
public:
    HomingController();
    void addStepperForHoming(uint8_t stepper, uint8_t direction, uint32_t speed);
    uint8_t updateState();
    void clearState();
};

#endif
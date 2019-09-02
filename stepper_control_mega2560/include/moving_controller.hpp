#ifndef moving_controller_hpp
#define moving_controller_hpp

#include <Arduino.h>

class MovingController
{
private:
    uint8_t getSteppersInMoving();
    String messageToSend = "";
public:
    MovingController();
    void addStepperForMove(uint8_t stepper, uint8_t direction, uint32_t steps);
    uint8_t updateState();
    void clearState();
};

#endif
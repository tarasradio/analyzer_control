#ifndef running_controller_hpp
#define running_controller_hpp

#include <Arduino.h>

class RunningController
{
private:
    String messageToSend = "";
public:
    RunningController();
    void addStepperForRun(uint8_t stepper, uint8_t direction, uint32_t speed);
    void setRunParams(uint8_t sensor, uint16_t sensorValue, uint8_t edgeType);
    uint8_t updateState();
    void clearState();
};

#endif
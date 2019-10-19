#ifndef running_controller_hpp
#define running_controller_hpp

#include <Arduino.h>

class RunningController
{
private:

public:
    RunningController();
    static void addStepperForRun(int8_t stepper, int32_t speed);
    static void setRunParams(int8_t sensor, uint16_t sensorValue, uint8_t edgeType);
    static uint8_t updateState();
    static void clearState();
};

#endif
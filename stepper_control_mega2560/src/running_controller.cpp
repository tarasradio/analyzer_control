#include "system.hpp"
#include "running_controller.hpp"

#include "packet_manager.hpp"

#include "protocol.hpp"
#include "steppers.hpp"
#include "sensors.hpp"

#define FILTER_VALUE 5

enum EdgeTypes
{
    RisingEdge,
    FallingEdge
};

uint8_t countRunSteppers = 0;
uint8_t steppersForRun[STEPPERS_COUNT];

uint8_t waitSensorNumber = 0;
uint16_t waitSensorValue = 0;
uint8_t valueEdgeType = RisingEdge;

uint8_t acceptedNeedValueCount = 0;

RunningController::RunningController()
{
    
}

void RunningController::addStepperForRun(int8_t stepper, int32_t speed)
{
    int8_t dir = speed > 0 ? 1 : 0;
    steppersForRun[countRunSteppers++] = stepper;
    Steppers::get(stepper).setMaxSpeed(abs(speed));
    Steppers::get(stepper).run(dir, abs(speed));
}

void RunningController::setRunParams(int8_t sensor, uint16_t sensorValue, uint8_t edgeType)
{
    waitSensorNumber = sensor;
    waitSensorValue = sensorValue;
    valueEdgeType = edgeType;

#ifdef DEBUG
    {
        String message = "[sensor = " + String(sensor);
        message += ", value = " + String(sensorValue);
        message += ", edgeType = ";

        Protocol::SendMessage(message.c_str());
    }
#endif

    if(RisingEdge == valueEdgeType)
    {
#ifdef DEBUG
        {
            String message = "rising]";
            Protocol::SendMessage(message.c_str());
        }
#endif
    }
    else if(FallingEdge == valueEdgeType)
    {
#ifdef DEBUG
        {
            String message = "falling]";
            Protocol::SendMessage(message.c_str());
        }
#endif
    }
    acceptedNeedValueCount = 0;
}

uint8_t RunningController::updateState()
{
    #ifdef EMULATOR
        return 0;
    #endif
    
    if(0 != countRunSteppers)
    {
        uint16_t value = Sensors::getSensorValue(waitSensorNumber);
#ifdef DEBUG
        {
            String message = "Run: wait value = ";
            message += String(waitSensorValue);
            message += ", real value = ";
            message += String(value);
            message += ", Filter num = ";
            message += String(acceptedNeedValueCount);
            Protocol::SendMessage(message.c_str());
        }
#endif
            if (RisingEdge == valueEdgeType)
            {
                if (value > waitSensorValue)
                {
                    if (acceptedNeedValueCount == FILTER_VALUE)
                    {
                        clearState();
                    }
                    acceptedNeedValueCount++;
                }
            }
            else if (FallingEdge == valueEdgeType)
            {
                if (value < waitSensorValue)
                {
                    if (acceptedNeedValueCount == FILTER_VALUE)
                    {
                        clearState();
                    }
                    acceptedNeedValueCount++;
                }
            }
    }

    return countRunSteppers;
}

void RunningController::clearState()
{
    for (int i = 0; i < countRunSteppers; i++)
    {
        uint8_t stepper = steppersForRun[i];

        Steppers::get(stepper).softStop();
    }
    countRunSteppers = 0;
}
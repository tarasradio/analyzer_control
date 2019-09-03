#include "running_controller.hpp"

#include "packet_manager.hpp"

#include "protocol.hpp"
#include "steppers.hpp"
#include "sensors.hpp"

#define FILTER_VALUE 10

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

void RunningController::addStepperForRun(uint8_t stepper, uint8_t direction, uint32_t speed)
{
    steppersForRun[countRunSteppers++] = stepper;
    getStepper(stepper).setMaxSpeed(speed);
    getStepper(stepper).run(direction, speed);

}

void RunningController::setRunParams(uint8_t sensor, uint16_t sensorValue, uint8_t edgeType)
{
    waitSensorNumber = sensor;
    waitSensorValue = sensorValue;
    valueEdgeType = edgeType;

#ifdef DEBUG
    messageToSend = "[ sensor = " + String(sensorNumber);
    messageToSend += ", value = " + String(sensorValue);
    messageToSend += ", edgeType = ";
#endif

    if(RisingEdge == valueEdgeType)
    {
#ifdef DEBUG
        messageToSend += "rising]";
#endif
    }
    else if(FallingEdge == valueEdgeType)
    {
#ifdef DEBUG
        messageToSend += "falling]";
#endif
    }
#ifdef DEBUG
    PacketManager::printMessage(messageToSend);
#endif

    acceptedNeedValueCount = 0;
}

uint8_t RunningController::updateState()
{
    if(0 != countRunSteppers)
    {
        uint16_t value = Sensors::getSensorValue(waitSensorNumber);
#ifdef DEBUG
            messageToSend = "Run: wait value = ";
            messageToSend += String(waitSensorValue);
            messageToSend += ", real value = ";
            messageToSend += String(value);
            messageToSend += ", Filter num = ";
            messageToSend += String(acceptedNeedValueCount);
            printMessage(messageToSend);
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

        getStepper(stepper).softStop();
    }
    countRunSteppers = 0;
}
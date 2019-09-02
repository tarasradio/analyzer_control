#include "command_executor2.hpp"

#include "packet_manager.hpp"

#include "homing_controller.hpp"
#include "moving_controller.hpp"
#include "running_controller.hpp"

#include "protocol.hpp"
#include "steppers.hpp"
#include "sensors.hpp"
#include "devices.hpp"

#define DEBUG
//#define SEND_STATE_PERMANENTLY

enum StopType
{
    STOP_SOFT = 0x00,
    STOP_HARD,
    HiZ_SOFT,
    HiZ_HARD
};

enum CommandExecutionType
{
    SIMPLE_COMMAND = 0x00,
    WAITING_COMMAND
};

uint32_t lastCommandId = 0;
uint8_t lastCommandState = COMMAND_DONE;
uint8_t waitForCommandDone = 0;

HomingController _homingController;
MovingController _movingController;
RunningController _runningController;

CommandExecutor2::CommandExecutor2()
{
    _homingController = HomingController();
    _movingController = MovingController();
    _runningController = RunningController();
}

void CommandExecutor2::UpdateState()
{
    printSteppersStates();
    printSensorsValues();

    if (0 != waitForCommandDone) // Есть команды, ожидающие завершения
    {
        if (
            (0 == _movingController.updateState()) &&
            (0 == _homingController.updateState()) &&
            (0 == _runningController.updateState())  ) // Моторы завершили движение
        {
            lastCommandState = COMMAND_DONE;
            waitForCommandDone = 0;
            printCommandStateResponse(lastCommandId, lastCommandState);
        }
    }
#ifdef SEND_STATE_PERMANENTLY
    printCommandStateResponse(lastCommandId, lastCommandState);
#endif
}

void CommandExecutor2::ExecuteCommand(uint8_t *packet, uint8_t packetLength)
{
    byte commandType = packet[0];

    switch (commandType)
    {
        case CMD_HOME:
        {
            executeHomeCommand(packet + 1, packetLength - 1);
        }
        break;
        case CMD_RUN:
        {
            executeRunCommand(packet + 1, packetLength - 1);
        }
        break;
        case CMD_MOVE:
        {
            executeMoveCommand(packet + 1, packetLength - 1);
        }
        break;
        case CMD_STOP:
        {
            executeStopCommand(packet + 1, packetLength - 1);
        }
        break;
        case CMD_SET_SPEED:
        {
            executeSetSpeedCommand(packet + 1, packetLength - 1);
        }
        break;
        case CMD_SET_DEVICE_STATE:
        {
            executeSetDeviceStateCommand(packet + 1, packetLength - 1);
        }
        break;
        case CNC_MOVE:
        {
            executeCncMoveCommand(packet + 1, packetLength - 1);
        }
        break;
        case CNC_SET_SPEED:
        {
            executeCncSetSpeedCommand(packet + 1, packetLength - 1);
        }
        break;
        case CNC_HOME:
        {
            executeCncHomeCommand(packet + 1, packetLength - 1);
        }
        break;
        case CNC_ON_DEVICE:
        {
            executeCncSetDeviceStateCommand(packet + 1, packetLength - 1, 1);
        }
        break;
        case CNC_OFF_DEVICE:
        {
            executeCncSetDeviceStateCommand(packet + 1, packetLength - 1, 0);
        }
        break;
        case CNC_RUN:
        {
            executeCncRunCommand(packet + 1, packetLength - 1);
        }
        break;
        case CMD_ABORT:
        {
            executeAbortCommand(packet + 1, packetLength - 1);
        }
        break;
        case CMD_WAIT_TIME:
        {
            executeWaitTimeCommand(packet + 1, packetLength - 1);
        }
        break;
        default:
        {
            PacketManager::printMessage("Unknown command!");
        }
        break;
    }
}

bool CommandExecutor2::checkStepper(uint8_t stepper)
{
    bool result = (stepper >= 0 && stepper < STEPPERS_COUNT) ? true : false;
    if(!result)
    {
        messageToSend = "wrong stepper = " + String(stepper);
        PacketManager::printMessage(messageToSend);
    }
    return result;
}

uint32_t CommandExecutor2::readLong(uint8_t *buffer)
{
    return *((unsigned long *)(buffer));
}

uint16_t CommandExecutor2::readInt(uint8_t *buffer)
{
    return *((unsigned int *)(buffer));
}

bool CommandExecutor2::checkSameCommand(uint32_t commandId, uint8_t commandType)
{
#ifdef DEBUG
    messageToSend = "command id = " + String(commandId);
    PacketManager::printMessage(messageToSend);
#endif

    bool isSame = false;
    if(lastCommandId == commandId)
    {
        isSame = true;
    }
    else
    {
        lastCommandId = commandId;
        lastCommandState = COMMAND_OK;

        if(WAITING_COMMAND == commandType)
        {
            waitForCommandDone = 1;
        }
    }
    printCommandStateResponse(commandId, lastCommandState);
#ifdef DEBUG
    messageToSend = "Is same = " + String(isSame);
    PacketManager::printMessage(messageToSend);
#endif
    return isSame;
}

void CommandExecutor2::executeWaitTimeCommand(uint8_t *packet, uint8_t packetLength)
{
    uint32_t periodMs = readLong(packet + 0);
    uint32_t packetId = readLong(packet + 4);

    if(checkSameCommand(packetId, WAITING_COMMAND))
        return;

#ifdef DEBUG
    messageToSend = "[Wait time] ";
    messageToSend += "period = " + String(periodMs);
    PacketManager::printMessage(messageToSend);
#endif

    //TODO: реализовать задержку
}

void CommandExecutor2::executeAbortCommand(uint8_t *packet, uint8_t packetLength)
{
    waitForCommandDone = 0;

    _homingController.clearState();
    _movingController.clearState();
    _runningController.clearState();

    for (uint8_t i = 0; i < STEPPERS_COUNT; i++)
        getStepper(i).softHiZ();

    for (uint8_t i = 0; i < 12; i++)
        Devices::device_off(i);

#ifdef DEBUG
    messageToSend = "[Abort] ";
    PacketManager::printMessage(messageToSend);
#endif
}

//TODO: исправить
void CommandExecutor2::executeHomeCommand(uint8_t *packet, uint8_t packetLength)
{
    uint8_t stepper = packet[0];
    uint8_t direction = packet[1];
    uint32_t fullSpeed = readLong(packet + 2);
    uint32_t packetId = readLong(packet + 6);

    if(checkSameCommand(packetId, WAITING_COMMAND))
        return;

    if (!checkStepper(stepper))
        return;

    _homingController.clearState();
    _homingController.addStepperForHoming(stepper, direction, fullSpeed);

#ifdef DEBUG
    messageToSend = "[Home] ";
    messageToSend += "stepper = " + String(stepper);
    messageToSend += ", dir = " + String(direction);
    messageToSend += ", speed = " + String(fullSpeed);

    PacketManager::printMessage(messageToSend);
#endif
}

void CommandExecutor2::executeRunCommand(uint8_t *packet, uint8_t packetLength)
{
    uint8_t stepper = packet[0];
    uint8_t direction = packet[1];
    uint32_t fullSpeed = readLong(packet + 2);
    uint32_t packetId = readLong(packet + 6);

    if(checkSameCommand(packetId, WAITING_COMMAND))
        return;

    if (!checkStepper(stepper))
        return;

    getStepper(stepper).run(direction, fullSpeed);

#ifdef DEBUG
    messageToSend = "[Run] ";
    messageToSend += "stepper = " + String(stepper);
    messageToSend += ", dir = " + String(direction);
    messageToSend += ", speed = " + String(fullSpeed);

    PacketManager::printMessage(messageToSend);
#endif
}

void CommandExecutor2::executeMoveCommand(uint8_t *packet, uint8_t packetLength)
{
    uint8_t stepper = packet[0];
    uint8_t direction = packet[1];
    uint32_t steps = readLong(packet + 2);
    uint32_t packetId = readLong(packet + 6);

    if(checkSameCommand(packetId, WAITING_COMMAND))
        return;

    if (!checkStepper(stepper))
        return;

    _movingController.addStepperForMove(stepper, direction, steps);

#ifdef DEBUG
    messageToSend = "[Move] ";
    messageToSend += "stepper = " + String(stepper);
    messageToSend += ", dir = " + String(direction);
    messageToSend += ", steps = " + String(steps);

    PacketManager::printMessage(messageToSend);
#endif
}

void CommandExecutor2::executeStopCommand(uint8_t *packet, uint8_t packetLength)
{
    uint8_t stepper = packet[0];
    uint8_t stopType = packet[1];
    uint32_t packetId = readLong(packet + 2);

    if(checkSameCommand(packetId, SIMPLE_COMMAND))
        return;

    if (!checkStepper(stepper))
        return;

#ifdef DEBUG
    messageToSend = "[Stop] ";
    messageToSend += "stepper = " + String(stepper);
    messageToSend += ", type = ";
#endif

    if(STOP_SOFT == stopType)
    {
#ifdef DEBUG
        messageToSend += "SOFT";
#endif
        getStepper(stepper).softStop();
    }
    else if(STOP_HARD == stopType)
    {
#ifdef DEBUG
        messageToSend += "HARD";
#endif
        getStepper(stepper).hardStop();
    }
    else if(HiZ_SOFT == stopType)
    {
#ifdef DEBUG
        messageToSend += "HiZ SOFT";
#endif
        getStepper(stepper).softHiZ();
    }
    else if(HiZ_HARD == stopType)
    {
#ifdef DEBUG
        messageToSend += "HiZ HARD";
#endif
        getStepper(stepper).hardHiZ();
    }
    else
    {
        messageToSend += "Undefined";
    }
#ifdef DEBUG
    PacketManager::printMessage(messageToSend);
#endif
}

void CommandExecutor2::executeSetSpeedCommand(uint8_t *packet, uint8_t packetLength)
{
    uint8_t stepper = packet[0];
    uint32_t fullSpeed = readLong(packet + 1);
    uint32_t packetId = readLong(packet + 5);

    if(checkSameCommand(packetId, SIMPLE_COMMAND))
        return;

    if (!checkStepper(stepper))
        return;

    getStepper(stepper).setMaxSpeed(fullSpeed << 2);
    getStepper(stepper).setFullSpeed(fullSpeed);

#ifdef DEBUG
    messageToSend = "[Set speed] ";
    messageToSend += "stepper = " + String(stepper);
    messageToSend += ", speed = " + String(fullSpeed);

    PacketManager::printMessage(messageToSend);
#endif
}

void CommandExecutor2::executeSetDeviceStateCommand(uint8_t *packet, uint8_t packetLength)
{
    uint8_t device = packet[0];
    uint8_t state = packet[1];
    uint32_t packetId = readLong(packet + 2);

    if(checkSameCommand(packetId, SIMPLE_COMMAND))
        return;

    Devices::device_set_state(device, state);

#ifdef DEBUG
    messageToSend = "[Set device state] ";
    messageToSend += "device = " + String(device);
    messageToSend += ", state = " + String(state);

    PacketManager::printMessage(messageToSend);
#endif
}

void CommandExecutor2::executeCncMoveCommand(uint8_t *packet, uint8_t packetLength)
{
    uint8_t countOfSteppers = packet[0];
    uint32_t packetId = readLong(packet + countOfSteppers * 6 + 1);

    if(checkSameCommand(packetId, WAITING_COMMAND))
        return;

    _movingController.clearState();

#ifdef DEBUG
    messageToSend = "[CNC Move] ";
    messageToSend += "Steppers = " + String(countOfSteppers);
    PacketManager::printMessage(messageToSend);
#endif

    for (int i = 0; i < countOfSteppers; i++)
    {
        uint8_t stepper = packet[i * 6 + 1];
        uint8_t direction = packet[i * 6 + 2];
        uint32_t steps = readLong(packet + i * 6 + 3);

        if (checkStepper(stepper))
        {
            _movingController.addStepperForMove(stepper, direction, steps);
        }

#ifdef DEBUG
        messageToSend = "[ stepper = " + String(stepper);
        messageToSend += ", dir = " + String(direction);
        messageToSend += ", steps = " + String(steps) + "] ";

        PacketManager::printMessage(messageToSend);
#endif
    }
}

void CommandExecutor2::executeCncHomeCommand(uint8_t *packet, uint8_t packetLength)
{
    uint8_t countOfSteppers = packet[0];

    uint32_t packetId = readLong(packet + countOfSteppers * 6 + 1);

    if(checkSameCommand(packetId, WAITING_COMMAND))
        return;

    _homingController.clearState();

#ifdef DEBUG
    messageToSend = "[CNC Home] ";
    messageToSend += "Steppers = " + String(countOfSteppers);
    PacketManager::printMessage(messageToSend);
#endif

    for (int i = 0; i < countOfSteppers; i++)
    {
        uint8_t stepper = packet[i * 6 + 1];
        uint8_t direction = packet[i * 6 + 2];
        uint32_t fullSpeed = readLong(packet + i * 6 + 3);

        if (checkStepper(stepper))
        {
            _homingController.addStepperForHoming(stepper, direction, fullSpeed);
        }

#ifdef DEBUG
        messageToSend = "[ stepper = " + String(stepper);
        messageToSend += ", dir = " + String(direction);
        messageToSend += ", speed = " + String(fullSpeed) + "] ";

        PacketManager::printMessage(messageToSend);
#endif
    }
}

void CommandExecutor2::executeCncRunCommand(uint8_t *packet, uint8_t packetLength)
{
    uint8_t countOfSteppers = packet[0];

    uint32_t packetId = readLong(packet + countOfSteppers * 6 + 4 + 1);

    if(checkSameCommand(packetId, WAITING_COMMAND))
        return;

    _runningController.clearState();

#ifdef DEBUG
    messageToSend = "[CNC Run] ";
    messageToSend += "Steppers = " + String(countOfSteppers);
    PacketManager::printMessage(messageToSend);
#endif

    for (int i = 0; i < countOfSteppers; i++)
    {
        uint8_t stepper = packet[i * 6 + 1];
        uint8_t direction = packet[i * 6 + 2];
        uint32_t fullSpeed = readLong(packet + i * 6 + 3);

        if (checkStepper(stepper))
        {
            _runningController.addStepperForRun(stepper, direction, fullSpeed);
        }

#ifdef DEBUG
        messageToSend = "[ stepper = " + String(stepper);
        messageToSend += ", dir = " + String(direction);
        messageToSend += ", speed = " + String(fullSpeed) + "] ";

        PacketManager::printMessage(messageToSend);
#endif
    }

    uint8_t sensorNumber = packet[countOfSteppers * 6 + 1];
    uint16_t sensorValue = readInt(packet + countOfSteppers * 6 + 2);
    uint8_t valueEdgeType = packet[countOfSteppers * 6 + 4];

    _runningController.setRunParams(sensorNumber, sensorValue, valueEdgeType);
}

void CommandExecutor2::executeCncSetSpeedCommand(uint8_t *packet, uint8_t packetLength)
{
    uint8_t countOfSteppers = packet[0];

    uint32_t packetId = readLong(packet + countOfSteppers * 5 + 1);

    if(checkSameCommand(packetId, SIMPLE_COMMAND))
        return;

#ifdef DEBUG
    messageToSend = "[CNC Set speed] ";
    messageToSend += "Steppers = " + String(countOfSteppers);
    PacketManager::printMessage(messageToSend);
#endif

    for (int i = 0; i < countOfSteppers; i++)
    {
        uint8_t stepper = packet[i * 5 + 1];
        uint32_t fullSpeed = readLong(packet + i * 5 + 2);

        if (checkStepper(stepper))
        {
            getStepper(stepper).setMaxSpeed(fullSpeed);
            getStepper(stepper).setFullSpeed(fullSpeed);
        }

#ifdef DEBUG
        messageToSend = "[ stepper = " + String(stepper);
        messageToSend += ", speed = " + String(fullSpeed) + "] ";

        PacketManager::printMessage(messageToSend);
#endif
    }
}

void CommandExecutor2::executeCncSetDeviceStateCommand(uint8_t *packet, uint8_t packetLength, uint8_t state)
{
    uint8_t countOfDevices = packet[0];

    uint32_t packetId = readLong(packet + countOfDevices * 1 + 1);

    if(checkSameCommand(packetId, SIMPLE_COMMAND))
        return;

#ifdef DEBUG
    messageToSend = "[CNC Set dev state] ";
    messageToSend += "devs = " + String(countOfDevices);
    PacketManager::printMessage(messageToSend);
#endif

    for (int i = 0; i < countOfDevices; i++)
    {
        uint8_t device = packet[i + 1];

        Devices::device_set_state(device, state);
#ifdef DEBUG
        messageToSend = "[ dev = " + String(device) + "] ";

        PacketManager::printMessage(messageToSend);
#endif
    }
}

void CommandExecutor2::printCommandStateResponse(uint32_t commandId, uint8_t commandState)
{
    Serial.write(packetHeader, packetHeaderLength);
    Serial.write(COMMAND_STATE);
    Serial.write(commandState);
    Serial.write((byte *)&commandId, sizeof(commandId));
    Serial.write(packetEnd, packetEndLength);
}

void CommandExecutor2::printSteppersStates()
{
    Serial.write(packetHeader, packetHeaderLength);
    Serial.write(STEPPERS_STATES);
    for (uint8_t i = 0; i < 18; i++)
    {
        uint16_t stepperStatus = getStepper(i).getStatus();
        Serial.write((byte *)&stepperStatus, sizeof(stepperStatus));
    }
    Serial.write(packetEnd, packetEndLength);
}

void CommandExecutor2::printSensorsValues()
{
    Serial.write(packetHeader, packetHeaderLength);
    Serial.write(SENSORS_VALUES);
    for (uint8_t i = 0; i < 16; i++)
    {
        uint16_t sensorValue = Sensors::getSensorValue(i);
        Serial.write((byte *)&sensorValue, sizeof(sensorValue));
    }
    Serial.write(packetEnd, packetEndLength);
}
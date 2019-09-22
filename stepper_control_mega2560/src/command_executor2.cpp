#include "command_executor2.hpp"

#include "packet_manager.hpp"

#include "homing_controller.hpp"
#include "moving_controller.hpp"
#include "running_controller.hpp"

#include "bar_scanner.hpp"

#include "protocol.hpp"
#include "steppers.hpp"
#include "sensors.hpp"
#include "devices.hpp"

//#define DEBUG
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

BarScanner _barScanner;

CommandExecutor2::CommandExecutor2()
{
    // _homingController = HomingController();
    // _movingController = MovingController();
    // _runningController = RunningController();
    // _barScanner = BarScanner();
}

void CommandExecutor2::UpdateState()
{
    printSteppersStates();
    printSensorsValues();
    _barScanner.updateState();

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
    uint32_t packetId = readLong(packet + 0);
    byte commandType = packet[4];

    switch (commandType)
    {
        case CMD_HOME:
        {
            executeHomeCommand(packet + 5, packetId);
        }
        break;
        case CMD_RUN:
        {
            executeRunCommand(packet + 5, packetId);
        }
        break;
        case CMD_MOVE:
        {
            executeMoveCommand(packet + 5, packetId);
        }
        break;
        case CMD_STOP:
        {
            executeStopCommand(packet + 5, packetId);
        }
        break;
        case CMD_SET_SPEED:
        {
            executeSetSpeedCommand(packet + 5, packetId);
        }
        break;
        case CMD_SET_DEVICE_STATE:
        {
            executeSetDeviceStateCommand(packet + 5, packetId);
        }
        break;
        case CNC_MOVE:
        {
            executeCncMoveCommand(packet + 5, packetId);
        }
        break;
        case CNC_SET_SPEED:
        {
            executeCncSetSpeedCommand(packet + 5, packetId);
        }
        break;
        case CNC_HOME:
        {
            executeCncHomeCommand(packet + 5, packetId);
        }
        break;
        case CNC_ON_DEVICE:
        {
            executeCncSetDeviceStateCommand(packet + 5, packetId, 1);
        }
        break;
        case CNC_OFF_DEVICE:
        {
            executeCncSetDeviceStateCommand(packet + 5, packetId, 0);
        }
        break;
        case CNC_RUN:
        {
            executeCncRunCommand(packet + 5, packetId);
        }
        break;
        case CMD_ABORT:
        {
            executeAbortCommand(packet + 5, packetId);
        }
        break;
        case CMD_BAR_START:
        {
            executeBarStartCommand(packet + 5, packetId);
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
        String message = "wrong stepper = " + String(stepper);
        PacketManager::printMessage(message);
    }
    else
    {
        Steppers::get(stepper).setMinSpeed(5);
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

bool CommandExecutor2::checkRepeatCommand(uint32_t commandId, uint8_t commandType)
{
#ifdef DEBUG
    {
        String message = "[cmd id = " + String(commandId);
        PacketManager::printMessage(message);
    }
#endif

    bool isRepeat = false;
    if(lastCommandId == commandId)
    {
        isRepeat = true;
#ifdef DEBUG
        {
            String message = ", repeat command]";
            PacketManager::printMessage(message);
        }
#endif
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

    return isRepeat;
}

void CommandExecutor2::executeAbortCommand(uint8_t *packet, uint32_t packetId)
{
    waitForCommandDone = 0;

    _homingController.clearState();
    _movingController.clearState();
    _runningController.clearState();

    for (uint8_t i = 0; i < STEPPERS_COUNT; i++)
        Steppers::get(i).softHiZ();

    for (uint8_t i = 0; i < 12; i++)
        Devices::off(i);

#ifdef DEBUG
    {
        String message = "[Abort]";
        PacketManager::printMessage(message);
    }
#endif
}

void CommandExecutor2::executeBarStartCommand(uint8_t *packet, uint32_t packetId)
{
    if(checkRepeatCommand(packetId, SIMPLE_COMMAND)) return;

    _barScanner.startScan();
#ifdef DEBUG
    {
        String message = "[Bar start]";
        PacketManager::printMessage(message);
    }
#endif
}

//TODO: исправить
void CommandExecutor2::executeHomeCommand(uint8_t *packet, uint32_t packetId)
{
    if(checkRepeatCommand(packetId, WAITING_COMMAND)) return;
    
    uint8_t stepper = packet[0];
    uint8_t direction = packet[1];
    uint32_t fullSpeed = readLong(packet + 2);

    if(!checkStepper(stepper)) return;

    _homingController.clearState();
    _homingController.addStepperForHoming(stepper, direction, fullSpeed);

#ifdef DEBUG
    {
        String message = "[Home] ";
        message += "stepper = " + String(stepper);
        message += ", dir = " + String(direction);
        message += ", spd = " + String(fullSpeed);

        PacketManager::printMessage(message);
    }
#endif
}

void CommandExecutor2::executeRunCommand(uint8_t *packet, uint32_t packetId)
{
    if(checkRepeatCommand(packetId, WAITING_COMMAND)) return;

    uint8_t stepper = packet[0];
    uint8_t direction = packet[1];
    uint32_t fullSpeed = readLong(packet + 2);

    if(!checkStepper(stepper)) return;

    Steppers::get(stepper).run(direction, fullSpeed);

#ifdef DEBUG
    {
        String message = "[Run] ";
        message += "stepper = " + String(stepper);
        message += ", dir = " + String(direction);
        message += ", speed = " + String(fullSpeed);

        PacketManager::printMessage(message);
    }
#endif
}

void CommandExecutor2::executeMoveCommand(uint8_t *packet, uint32_t packetId)
{
    if(checkRepeatCommand(packetId, WAITING_COMMAND)) return;

    uint8_t stepper = packet[0];
    uint8_t direction = packet[1];
    uint32_t steps = readLong(packet + 2);

    if(!checkStepper(stepper)) return;

    _movingController.addStepperForMove(stepper, direction, steps);

#ifdef DEBUG
    {
        String message = "[Move] ";
        message += "stepper = " + String(stepper);
        message += ", dir = " + String(direction);
        message += ", steps = " + String(steps);

        PacketManager::printMessage(message);
    }
#endif
}

void CommandExecutor2::executeStopCommand(uint8_t *packet, uint32_t packetId)
{
    if(checkRepeatCommand(packetId, SIMPLE_COMMAND)) return;

    uint8_t stepper = packet[0];
    uint8_t stopType = packet[1];

    if(!checkStepper(stepper)) return;

    if(STOP_SOFT == stopType)
    {
        Steppers::get(stepper).softStop();
    }
    else if(STOP_HARD == stopType)
    {
        Steppers::get(stepper).hardStop();
    }
    else if(HiZ_SOFT == stopType)
    {
        Steppers::get(stepper).softHiZ();
    }
    else if(HiZ_HARD == stopType)
    {
        Steppers::get(stepper).hardHiZ();
    }

#ifdef DEBUG
    {
        String message = "[Stop]";
        message += " stepper = " + String(stepper);
        PacketManager::printMessage(message);
    }
#endif
}

void CommandExecutor2::executeSetSpeedCommand(uint8_t *packet, uint32_t packetId)
{
    if(checkRepeatCommand(packetId, SIMPLE_COMMAND)) return;

    uint8_t stepper = packet[0];
    uint32_t fullSpeed = readLong(packet + 1);

    if(!checkStepper(stepper)) return;

    Steppers::get(stepper).setMaxSpeed(fullSpeed << 2);
    Steppers::get(stepper).setFullSpeed(fullSpeed);

#ifdef DEBUG
    {
        String message = "[Set speed] ";
        message += "stepper = " + String(stepper);
        message += ", spd = " + String(fullSpeed);

        PacketManager::printMessage(message);
    }
#endif
}

void CommandExecutor2::executeSetDeviceStateCommand(uint8_t *packet, uint32_t packetId)
{
    if(checkRepeatCommand(packetId, SIMPLE_COMMAND)) return;

    uint8_t device = packet[0];
    uint8_t state = packet[1];

    Devices::setState(device, state);

#ifdef DEBUG
    {
        String message = "[Set dev state] ";
        message += "dev = " + String(device);
        message += ", state = " + String(state);

        PacketManager::printMessage(message);
    }
#endif
}

void CommandExecutor2::executeCncMoveCommand(uint8_t *packet, uint32_t packetId)
{
    if(checkRepeatCommand(packetId, WAITING_COMMAND)) return;

    uint8_t countOfSteppers = packet[0];

    _movingController.clearState();

#ifdef DEBUG
    {
        String message = "[CNC Move] ";
        message += "steppers = " + String(countOfSteppers);
        PacketManager::printMessage(message);
    }
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
        {
            String message = "[stepper = " + String(stepper);
            message += ", dir = " + String(direction);
            message += ", steps = " + String(steps) + "] ";

            PacketManager::printMessage(message);
        }
#endif
    }
}

void CommandExecutor2::executeCncHomeCommand(uint8_t *packet, uint32_t packetId)
{
    if(checkRepeatCommand(packetId, WAITING_COMMAND)) return;

    uint8_t countOfSteppers = packet[0];

    _homingController.clearState();

#ifdef DEBUG
    {
        String message = "[CNC Home] ";
        message += "steppers = " + String(countOfSteppers);
        PacketManager::printMessage(message);
    }
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
        {
            String message = "[stepper = " + String(stepper);
            message += ", dir = " + String(direction);
            message += ", speed = " + String(fullSpeed) + "] ";

            PacketManager::printMessage(message);
        }
#endif
    }
}

void CommandExecutor2::executeCncRunCommand(uint8_t *packet, uint32_t packetId)
{
    if(checkRepeatCommand(packetId, WAITING_COMMAND)) return;

    uint8_t countOfSteppers = packet[0];

    _runningController.clearState();

#ifdef DEBUG
    {
        String message = "[CNC Run] ";
        message += "Steppers = " + String(countOfSteppers);
        PacketManager::printMessage(message);
    }
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
        {
            String message = "[stepper = " + String(stepper);
            message += ", dir = " + String(direction);
            message += ", speed = " + String(fullSpeed) + "] ";

            PacketManager::printMessage(message);
        }
#endif
    }

    uint8_t sensorNumber = packet[countOfSteppers * 6 + 1];
    uint16_t sensorValue = readInt(packet + countOfSteppers * 6 + 2);
    uint8_t valueEdgeType = packet[countOfSteppers * 6 + 4];

    _runningController.setRunParams(sensorNumber, sensorValue, valueEdgeType);
}

void CommandExecutor2::executeCncSetSpeedCommand(uint8_t *packet, uint32_t packetId)
{
    if(checkRepeatCommand(packetId, SIMPLE_COMMAND)) return;
    
    uint8_t countOfSteppers = packet[0];

#ifdef DEBUG
    {
        String message = "[CNC Set speed] ";
        message += "steppers = " + String(countOfSteppers);
        PacketManager::printMessage(message);
    }
#endif

    for (int i = 0; i < countOfSteppers; i++)
    {
        uint8_t stepper = packet[i * 5 + 1];
        uint32_t fullSpeed = readLong(packet + i * 5 + 2);

        if (checkStepper(stepper))
        {
            Steppers::get(stepper).setMaxSpeed(fullSpeed << 2);
            Steppers::get(stepper).setFullSpeed(fullSpeed);
        }

#ifdef DEBUG
        {
            String message = "[ stepper = " + String(stepper);
            message += ", speed = " + String(fullSpeed) + "] ";

            PacketManager::printMessage(message);
        }
#endif
    }
}

void CommandExecutor2::executeCncSetDeviceStateCommand(uint8_t *packet, uint32_t packetId, uint8_t state)
{
    if(checkRepeatCommand(packetId, SIMPLE_COMMAND)) return;

    uint8_t countOfDevices = packet[0];

#ifdef DEBUG
    {
        String message = "[CNC Set dev state] ";
        message += "devs = " + String(countOfDevices);
        PacketManager::printMessage(message);
    }
#endif

    for (int i = 0; i < countOfDevices; i++)
    {
        uint8_t device = packet[i + 1];

        Devices::setState(device, state);
#ifdef DEBUG
        {
            String message = "[ dev = " + String(device) + "] ";
            PacketManager::printMessage(message);
        }
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
        uint16_t stepperStatus = Steppers::get(i).getStatus();
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
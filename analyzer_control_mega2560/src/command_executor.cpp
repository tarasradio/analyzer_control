#include "system.hpp"
#include "emulator.hpp"
#include "command_executor.hpp"

#include "protocol.hpp"

#include "steppers.hpp"
#include "sensors.hpp"
#include "devices.hpp"

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

CommandExecutor::CommandExecutor(
    HomingController * homingController,
    RunningController * runningController,
    MovingController * movingController,
    BarcodeScanner * tubeScanner,
    BarcodeScanner * cartridgeScanner)
{
    this->homingController = homingController;
    this->runningController = runningController;
    this->homingController = homingController;
    this->tubeScanner = tubeScanner;
    this->cartridgeScanner = cartridgeScanner;
}

void CommandExecutor::updateState()
{
    sendSteppersStates();
    sendSensorsValues();

    tubeScanner->updateState();
    cartridgeScanner->updateState();

    if (0 != waitForCommandDone) // Есть команды, ожидающие завершения
    {
        if (
            (0 == movingController->updateState()) &&
            (0 == homingController->updateState()) &&
            (0 == runningController->updateState())  ) // Моторы завершили движение
        {
            lastCommandState = COMMAND_DONE;
            waitForCommandDone = 0;
            Protocol::sendCommandState(&lastCommandId, lastCommandState);
        }
    }
}

void CommandExecutor::listenPacket(uint8_t *packet, uint8_t packetLength)
{
    uint32_t packetId = readLong(packet + 0);
    uint8_t commandCode = packet[4];
    
    switch (commandCode)
    {
        case CMD_HOME:
            executeHomeCommand(packet + 5, packetId);
        break;
        case CMD_RUN:
            executeRunCommand(packet + 5, packetId);
        break;
        case CMD_MOVE:
            executeMoveCommand(packet + 5, packetId);
        break;
        case CMD_STOP:
            executeStopCommand(packet + 5, packetId);
        break;
        case CMD_SET_SPEED:
            executeSetSpeedCommand(packet + 5, packetId);
        break;
        case CMD_SET_DEVICE_STATE:
            executeSetDeviceStateCommand(packet + 5, packetId);
        break;
        case CNC_MOVE:
            executeCncMoveCommand(packet + 5, packetId);
        break;
        case CNC_SET_SPEED:
            executeCncSetSpeedCommand(packet + 5, packetId);
        break;
        case CNC_HOME:
            executeCncHomeCommand(packet + 5, packetId);
        break;
        case CNC_ON_DEVICE:
            executeCncSetDeviceStateCommand(packet + 5, packetId, 1);
        break;
        case CNC_OFF_DEVICE:
            executeCncSetDeviceStateCommand(packet + 5, packetId, 0);
        break;
        case CNC_RUN:
            executeCncRunCommand(packet + 5, packetId);
        break;
        case CMD_ABORT:
            executeAbortCommand(packet + 5, packetId);
        break;
        case CMD_SCAN_BARCODE:
            executeBarcodeScanCommand(packet + 5, packetId);
        break;
        case CMD_GET_FIRMWARE_VERSION:
            executeGetFirmwareVersionCommand(packet + 5, packetId);
        break;
        default:
        {
#ifdef DEBUG
            String message = "Unknown command!";
            Protocol::sendMessage(message.c_str());
#endif
        }
        break;
    }
}

bool CommandExecutor::checkStepper(uint8_t stepper)
{
    bool result = (stepper >= 0 && stepper < STEPPERS_COUNT) ? true : false;
    if(!result)
    {
        String message = "wrong stepper = " + String(stepper);
        Protocol::sendMessage(message.c_str());
    }
    else
    {
        Steppers::get(stepper).setMinSpeed(5);
    }
    
    return result;
}

uint32_t CommandExecutor::readLong(uint8_t *buffer)
{
    return *((unsigned long *)(buffer));
}

uint16_t CommandExecutor::readInt(uint8_t *buffer)
{
    return *((unsigned int *)(buffer));
}

bool CommandExecutor::checkRepeatCommand(uint32_t commandId, uint8_t commandType)
{
#ifdef DEBUG
    {
        String message = "[cmd id = " + String(commandId) + "]";
        Protocol::sendMessage(message.c_str());
    }
#endif

    bool isRepeat = false;
    if(lastCommandId == commandId)
    {
        isRepeat = true;
#ifdef DEBUG
        {
            String message = "[repeat cmd]";
            Protocol::sendMessage(message.c_str());
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
    Protocol::sendCommandState(&lastCommandId, lastCommandState);

    return isRepeat;
}

void CommandExecutor::executeGetFirmwareVersionCommand(uint8_t *packet, uint32_t packetId)
{
    Protocol::sendFirmwareVersion(VERSION);
}

void CommandExecutor::executeAbortCommand(uint8_t *packet, uint32_t packetId)
{
    waitForCommandDone = 0;

    homingController->clearState();
    movingController->clearState();
    runningController->clearState();

    for (uint8_t i = 0; i < STEPPERS_COUNT; i++)
        Steppers::get(i).softHiZ();

    for (uint8_t i = 0; i < 12; i++)
        Devices::off(i);

#ifdef DEBUG
    {
        String message = "[Abort]";
        Protocol::sendMessage(message.c_str());
    }
#endif
}

void CommandExecutor::executeBarcodeScanCommand(uint8_t *packet, uint32_t packetId)
{
    if(checkRepeatCommand(packetId, SIMPLE_COMMAND)) return;

    int8_t scanner = packet[0];

    if(scanner == ScannerType::TubeScanner) {
        tubeScanner->startScan();
    } else if(scanner == ScannerType::CartridgeScanner) {
        cartridgeScanner->startScan();
    }
    
#ifdef DEBUG
    {
        String message = "[Scan barcode]";
        Protocol::sendMessage(message.c_str());
    }
#endif
}

//TODO: исправить
void CommandExecutor::executeHomeCommand(uint8_t *packet, uint32_t packetId)
{
    if(checkRepeatCommand(packetId, WAITING_COMMAND)) return;

#ifdef EMULATOR
    Emulator::runTask();
#endif
    
    int8_t stepper = packet[0];
    int32_t fullSpeed = readLong(packet + 1);

    if(!checkStepper(stepper)) return;

    homingController->clearState();
    homingController->addStepperForHoming(stepper, fullSpeed);

#ifdef DEBUG
    {
        String message = "[Home] ";
        message += "stepper = " + String(stepper);
        message += ", spd = " + String(fullSpeed);

        Protocol::sendMessage(message.c_str());
    }
#endif
}

void CommandExecutor::executeRunCommand(uint8_t *packet, uint32_t packetId)
{
    if(checkRepeatCommand(packetId, WAITING_COMMAND)) return;

#ifdef EMULATOR
    Emulator::runTask();
#endif

    int8_t stepper = packet[0];
    int32_t fullSpeed = readLong(packet + 1);

    if(!checkStepper(stepper)) return;

    Steppers::get(stepper).run(fullSpeed > 0 ? 1 : 0, fullSpeed);

#ifdef DEBUG
    {
        String message = "[Run] ";
        message += "stepper = " + String(stepper);
        message += ", speed = " + String(fullSpeed);

        Protocol::sendMessage(message.c_str());
    }
#endif
}

void CommandExecutor::executeMoveCommand(uint8_t *packet, uint32_t packetId)
{
    if(checkRepeatCommand(packetId, WAITING_COMMAND)) return;

#ifdef EMULATOR
    Emulator::runTask();
#endif

    int8_t stepper = packet[0];
    int32_t steps = readLong(packet + 1);

    if(!checkStepper(stepper)) return;

    movingController->addStepperForMove(stepper, steps);

#ifdef DEBUG
    {
        String message = "[Move] ";
        message += "stepper = " + String(stepper);
        message += ", steps = " + String(steps);

        Protocol::sendMessage(message.c_str());
    }
#endif
}

void CommandExecutor::executeStopCommand(uint8_t *packet, uint32_t packetId)
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

        Protocol::sendMessage(message.c_str());
    }
#endif
}

void CommandExecutor::executeSetSpeedCommand(uint8_t *packet, uint32_t packetId)
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

        Protocol::sendMessage(message.c_str());
    }
#endif
}

void CommandExecutor::executeSetDeviceStateCommand(uint8_t *packet, uint32_t packetId)
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

        Protocol::sendMessage(message.c_str());
    }
#endif
}

void CommandExecutor::executeCncMoveCommand(uint8_t *packet, uint32_t packetId)
{
    if(checkRepeatCommand(packetId, WAITING_COMMAND)) return;

#ifdef EMULATOR
    Emulator::runTask();
#endif

    int8_t countOfSteppers = packet[0];

    movingController->clearState();

#ifdef DEBUG
    {
        String message = "[CNC Move] ";
        message += "steppers = " + String(countOfSteppers);

        Protocol::sendMessage(message.c_str());
    }
#endif

    for (int i = 0; i < countOfSteppers; i++)
    {
        int8_t stepper = packet[i * 5 + 1];
        int32_t steps = readLong(packet + i * 5 + 2);

        if (checkStepper(stepper))
        {
            movingController->addStepperForMove(stepper, steps);
        }

#ifdef DEBUG
        {
            String message = "[stepper = " + String(stepper);
            message += ", steps = " + String(steps) + "] ";

            Protocol::sendMessage(message.c_str());
        }
#endif
    }
}

void CommandExecutor::executeCncHomeCommand(uint8_t *packet, uint32_t packetId)
{
    if(checkRepeatCommand(packetId, WAITING_COMMAND)) return;

#ifdef EMULATOR
    Emulator::runTask();
#endif

    uint8_t countOfSteppers = packet[0];

    homingController->clearState();

#ifdef DEBUG
    {
        String message = "[CNC Home] ";
        message += "steppers = " + String(countOfSteppers);

        Protocol::sendMessage(message.c_str());
    }
#endif

    for (int i = 0; i < countOfSteppers; i++)
    {
        int8_t stepper = packet[i * 5 + 1];
        int32_t fullSpeed = readLong(packet + i * 5 + 2);

        if (checkStepper(stepper))
        {
            homingController->addStepperForHoming(stepper, fullSpeed);
        }

#ifdef DEBUG
        {
            String message = "[stepper = " + String(stepper);
            message += ", speed = " + String(fullSpeed) + "] ";

            Protocol::sendMessage(message.c_str());
        }
#endif
    }
}

void CommandExecutor::executeCncRunCommand(uint8_t *packet, uint32_t packetId)
{
    if(checkRepeatCommand(packetId, WAITING_COMMAND)) return;

#ifdef EMULATOR
    Emulator::runTask();
#endif

    uint8_t countOfSteppers = packet[0];

    runningController->clearState();

#ifdef DEBUG
    {
        String message = "[CNC Run] ";
        message += "Steppers = " + String(countOfSteppers);

        Protocol::sendMessage(message.c_str());
    }
#endif

    for (int i = 0; i < countOfSteppers; i++)
    {
        int8_t stepper = packet[i * 5 + 1];
        int32_t fullSpeed = readLong(packet + i * 5 + 2);

        if (checkStepper(stepper))
        {
            runningController->addStepperForRun(stepper, fullSpeed);
        }

#ifdef DEBUG
        {
            String message = "[stepper = " + String(stepper);
            message += ", speed = " + String(fullSpeed) + "] ";

            Protocol::sendMessage(message.c_str());
        }
#endif
    }

    uint8_t sensorNumber = packet[countOfSteppers * 5 + 1];
    uint16_t sensorValue = readInt(packet + countOfSteppers * 5 + 2);
    uint8_t valueEdgeType = packet[countOfSteppers * 5 + 4];

    runningController->setRunParams(sensorNumber, sensorValue, valueEdgeType);
}

void CommandExecutor::executeCncSetSpeedCommand(uint8_t *packet, uint32_t packetId)
{
    if(checkRepeatCommand(packetId, SIMPLE_COMMAND)) return;
    
    uint8_t countOfSteppers = packet[0];

#ifdef DEBUG
    {
        String message = "[CNC Set speed] ";
        message += "steppers = " + String(countOfSteppers);

        Protocol::sendMessage(message.c_str());
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

            Protocol::sendMessage(message.c_str());
        }
#endif
    }
}

void CommandExecutor::executeCncSetDeviceStateCommand(uint8_t *packet, uint32_t packetId, uint8_t state)
{
    if(checkRepeatCommand(packetId, SIMPLE_COMMAND)) return;

    uint8_t countOfDevices = packet[0];

#ifdef DEBUG
    {
        String message = "[CNC Set dev state] ";
        message += "devs = " + String(countOfDevices);

        Protocol::sendMessage(message.c_str());
    }
#endif

    for (int i = 0; i < countOfDevices; i++)
    {
        uint8_t device = packet[i + 1];

        Devices::setState(device, state);
#ifdef DEBUG
        {
            String message = "[ dev = " + String(device) + "] ";

            Protocol::sendMessage(message.c_str());
        }
#endif
    }
}

void CommandExecutor::sendSteppersStates()
{
    uint16_t steppersStates[STEPPERS_COUNT];

    for (uint8_t i = 0; i < STEPPERS_COUNT; i++)
        steppersStates[i] =  Steppers::get(i).getStatus();

    Protocol::sendSteppersStates(steppersStates, STEPPERS_COUNT);
}

void CommandExecutor::sendSensorsValues()
{
    uint16_t sensorValues[16];

    for (uint8_t i = 0; i < 16; i++)
        sensorValues[i] =  Sensors::getSensorValue(i);

    Protocol::sendSensorsValues(sensorValues, 16);
}
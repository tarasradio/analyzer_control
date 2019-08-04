#include "command_executor2.hpp"
#include "protocol.hpp"
#include "steppers.hpp"
#include "sensors.hpp"
#include "devices.hpp"

#define DEBUG

enum StopType
{
    STOP_SOFT = 0x00,
    STOP_HARD,
    HiZ_SOFT,
    HiZ_HARD
};

enum HomingState
{
    HOMING_SUCCESS = 0x00,
    WAIT_SW_PRESSED,
    WAIT_SW_RELEASED
};

enum MoveType
{
    SIMPLE_MOVE = 0x00,
    HOMING_MOVE,
    NO_MOVE
};

enum CommandExecutionType
{
    SIMPLE_COMMAND = 0x00,
    WAITING_COMMAND
};

uint8_t steppersForMove[STEPPERS_COUNT];
uint8_t countMoveSteppers = 0;

uint8_t steppersForHomingStates[STEPPERS_COUNT];
uint8_t steppersHomingDirection[STEPPERS_COUNT];
uint32_t steppersHomingSpeed[STEPPERS_COUNT];

uint8_t currentMoveType = NO_MOVE;

uint32_t lastCommandId = 0;
uint8_t lastCommandState = COMMAND_DONE;
uint8_t waitForCommandDone = 0;

String messageToSend = "";

CommandExecutor2::CommandExecutor2()
{

}

// 0 - not move, 1 - move
uint8_t CommandExecutor2::getStepperMoveState(uint8_t stepper)
{
    uint16_t stepperStatus = getStepper(stepper).getStatus() & STATUS_MOT_STATUS;
    return (uint8_t)stepperStatus;
}

uint8_t CommandExecutor2::getSteppersInMove()
{
    uint8_t steppersInMove = 0;
    for (int i = 0; i < countMoveSteppers; i++)
    {
        uint8_t stepper = steppersForMove[i];

        if (getStepperMoveState(stepper) != 0)
            steppersInMove++;
    }

    return steppersInMove;
}

uint8_t CommandExecutor2::getSteppersInHoming()
{
    uint8_t steppersInHoming = 0;

    for (int i = 0; i < countMoveSteppers; i++)
    {
        uint8_t stepper = steppersForMove[i];
        if (steppersForHomingStates[i] == WAIT_SW_PRESSED)
        {
            if (getStepperMoveState(stepper) != 0)
            {
                steppersInHoming++;
            }
            else
            {
                steppersForHomingStates[i] = HOMING_SUCCESS;
            }
        }
        else if (steppersForHomingStates[i] == WAIT_SW_RELEASED)
        {
            if (getStepperMoveState(stepper) != 0)
            {
                steppersInHoming++;
            }
            else
            {
                getStepper(stepper).goUntil(RESET_ABSPOS, steppersHomingDirection[i], steppersHomingSpeed[i]);
                steppersForHomingStates[i] = WAIT_SW_PRESSED;
            }
        }
    }

    return steppersInHoming;
}

void CommandExecutor2::UpdateState()
{
    printSteppersStates();

    if (waitForCommandDone != 0) // Есть команды, ожидающие завершения
    {
        if (getSteppersInMove() == 0 && getSteppersInHoming() == 0) // Моторы завершили движение
        {
            lastCommandState = COMMAND_DONE;
            waitForCommandDone = 0;
        }
    }

    printCommandStateResponse(lastCommandId, lastCommandState);
}

void CommandExecutor2::ExecuteCommand(uint8_t *packet, uint8_t packetLength)
{
    byte commandType = packet[0];

    switch (commandType)
    {
        case CMD_GO_UNTIL:
        {
            executeGoUntilCommand(packet + 1, packetLength - 1);
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
        case CMD_ABORT:
        {
            executeAbortCommand(packet + 1, packetLength - 1);
        }
        break;
        default:
        {
            printMessage("Unknown command!");
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
    printMessage(messageToSend);
  }
  return result;
}

uint32_t CommandExecutor2::readLong(uint8_t *buffer)
{
  return *((unsigned long *)(buffer));
}

bool CommandExecutor2::checkSameCommand(uint32_t commandId, uint8_t commandType)
{
#ifdef DEBUG
    messageToSend = "command id = " + String(commandId);
    printMessage(messageToSend);
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

        if(commandType == WAITING_COMMAND)
        {
            waitForCommandDone = 1;
        }
    }
    printCommandStateResponse(commandId, lastCommandState);
#ifdef DEBUG
    messageToSend = "Is same = " + String(isSame);
    printMessage(messageToSend);
#endif
    return isSame;
}

void CommandExecutor2::executeAbortCommand(uint8_t *packet, uint8_t packetLength)
{
    waitForCommandDone = 0;

    for (uint8_t i = 0; i < STEPPERS_COUNT; i++)
    {
        getStepper(i).softHiZ();
    }

    for (uint8_t i = 0; i < 12; i++)
    {
        Devices::device_off(i);
    }

#ifdef DEBUG
    messageToSend = "[Abort] ";

    printMessage(messageToSend);
#endif
}

void CommandExecutor2::executeGoUntilCommand(uint8_t *packet, uint8_t packetLength)
{
    uint8_t stepper = packet[0];
    uint8_t direction = packet[1];
    uint32_t fullSpeed = readLong(packet + 2);
    uint32_t packetId = readLong(packet + 6);

    if(checkSameCommand(packetId, WAITING_COMMAND))
        return;

    if (!checkStepper(stepper))
        return;

    countMoveSteppers = 1;
    steppersForMove[0] = stepper;

    getStepper(stepper).goUntil(RESET_ABSPOS, direction, fullSpeed);

#ifdef DEBUG
    messageToSend = "[Go until] ";
    messageToSend += "stepper = " + String(stepper);
    messageToSend += ", dir = " + String(direction);
    messageToSend += ", speed = " + String(fullSpeed);

    printMessage(messageToSend);
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

    printMessage(messageToSend);
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

    countMoveSteppers = 1;
    steppersForMove[0] = stepper;

    getStepper(stepper).move(direction, steps);

#ifdef DEBUG
    messageToSend = "[Move] ";
    messageToSend += "stepper = " + String(stepper);
    messageToSend += ", dir = " + String(direction);
    messageToSend += ", steps = " + String(steps);

    printMessage(messageToSend);
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
    messageToSend += ", stopType = ";
#endif

    switch (stopType)
    {
    case STOP_SOFT:
    {
#ifdef DEBUG
        messageToSend += "SOFT";
#endif
        getStepper(stepper).softStop();
    }
    break;
    case STOP_HARD:
    {
#ifdef DEBUG
        messageToSend += "HARD";
#endif
        getStepper(stepper).hardStop();
    }
    break;
    case HiZ_SOFT:
    {
#ifdef DEBUG
        messageToSend += "HiZ SOFT";
#endif
        getStepper(stepper).softHiZ();
    }
    break;
    case HiZ_HARD:
    {
#ifdef DEBUG
        messageToSend += "HiZ HARD";
#endif
        getStepper(stepper).hardHiZ();
    }
    break;
    }
#ifdef DEBUG
    printMessage(messageToSend);
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

    getStepper(stepper).setMaxSpeed(fullSpeed);
    getStepper(stepper).setFullSpeed(fullSpeed);

#ifdef DEBUG
    messageToSend = "[Set speed] ";
    messageToSend += "stepper = " + String(stepper);
    messageToSend += ", speed = " + String(fullSpeed);

    printMessage(messageToSend);
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

    printMessage(messageToSend);
#endif
}

void CommandExecutor2::executeCncMoveCommand(uint8_t *packet, uint8_t packetLength)
{
    uint8_t countOfSteppers = packet[0];
    uint32_t packetId = readLong(packet + countOfSteppers * 6 + 1);

    if(checkSameCommand(packetId, WAITING_COMMAND))
        return;

    countMoveSteppers = countOfSteppers;

#ifdef DEBUG
    messageToSend = "[CNC Move] ";
    messageToSend += "Steppers = " + String(countOfSteppers);
    printMessage(messageToSend);
#endif

    for (int i = 0; i < countOfSteppers; i++)
    {
        uint8_t stepper = packet[i * 6 + 1];
        uint8_t direction = packet[i * 6 + 2];
        uint32_t steps = readLong(packet + i * 6 + 3);

        if (checkStepper(stepper))
        {
            steppersForMove[i] = stepper;
            getStepper(stepper).move(direction, steps);
        }

#ifdef DEBUG
        messageToSend = "[ stepper = " + String(stepper);
        messageToSend += ", dir = " + String(direction);
        messageToSend += ", steps = " + String(steps) + "] ";

        printMessage(messageToSend);
#endif
    }
}

void CommandExecutor2::executeCncHomeCommand(uint8_t *packet, uint8_t packetLength)
{
    uint8_t countOfSteppers = packet[0];

    uint32_t packetId = readLong(packet + countOfSteppers * 6 + 1);

    if(checkSameCommand(packetId, WAITING_COMMAND))
        return;

    countMoveSteppers = countOfSteppers;

#ifdef DEBUG
    messageToSend = "[CNC Home] ";
    messageToSend += "Steppers = " + String(countOfSteppers);
    printMessage(messageToSend);
#endif

    for (int i = 0; i < countOfSteppers; i++)
    {
        uint8_t stepper = packet[i * 6 + 1];
        uint8_t direction = packet[i * 6 + 2];
        uint32_t fullSpeed = readLong(packet + i * 6 + 3);

        if (checkStepper(stepper))
        {
            steppersForMove[i] = stepper;
            steppersHomingDirection[i] = direction;
            steppersHomingSpeed[i] = fullSpeed;

            uint8_t sw_status = getStepper(stepper).getStatus() & STATUS_SW_F;
            if (sw_status != 0)
            {
                steppersForHomingStates[i] = WAIT_SW_PRESSED;
                getStepper(stepper).goUntil(RESET_ABSPOS, direction, fullSpeed);
            }
            else
            {
                steppersForHomingStates[i] = WAIT_SW_RELEASED;
                uint8_t inverseDir = direction == FWD ? REV : FWD;
                getStepper(stepper).releaseSw(RESET_ABSPOS, inverseDir); // настроить MIN SPEED
            }
        }

#ifdef DEBUG
        messageToSend = "[ stepper = " + String(stepper);
        messageToSend += ", dir = " + String(direction);
        messageToSend += ", speed = " + String(fullSpeed) + "] ";

        printMessage(messageToSend);
#endif
    }
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
    printMessage(messageToSend);
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

        printMessage(messageToSend);
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
    printMessage(messageToSend);
#endif

    for (int i = 0; i < countOfDevices; i++)
    {
        uint8_t device = packet[i + 1];

        Devices::device_set_state(device, state);
#ifdef DEBUG
        messageToSend = "[ dev = " + String(device) + "] ";

        printMessage(messageToSend);
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

void CommandExecutor2::printMessage(String messageText)
{
    Serial.write(packetHeader, packetHeaderLength);
    Serial.write(TEXT_MESSAGE);
    Serial.println(messageText);
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
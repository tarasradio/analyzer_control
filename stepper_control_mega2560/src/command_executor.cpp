#include "command_executor.hpp"
#include "protocol.hpp"

#include "steppers.hpp"
#include "sensors.hpp"
#include "devices.hpp"

//#define DEBUG

void readPacket();
void processPacket();
void executeCommand(uint8_t *buffer, uint8_t bufferLength);

bool checkStepper(uint8_t stepper);

void executeAbortCommand(uint8_t *packet, uint8_t packetLength);
void executeGoUntilCommand(uint8_t *packet, uint8_t packetLength);
void executeRunCommand(uint8_t *packet, uint8_t packetLength);
void executeMoveCommand(uint8_t *packet, uint8_t packetLength);
void executeStopCommand(uint8_t *packet, uint8_t packetLength);
void executeSetSpeedCommand(uint8_t *packet, uint8_t packetLength);
void executeSetDeviceStateCommand(uint8_t *packet, uint8_t packetLength);

void executeCncMoveCommand(uint8_t *packet, uint8_t packetLength);
void executeCncHomeCommand(uint8_t *packet, uint8_t packetLength);
void executeCncSetSpeedCommand(uint8_t *packet, uint8_t packetLength);
void executeCncSetDeviceStateCommand(uint8_t *packet, uint8_t packetLength, uint8_t state);

void printCommandStateResponse(uint32_t commandId, uint8_t commandState);
void printMessage(String messageText);
void printSteppersStates();

uint8_t incomingBuffer[64];
uint8_t currentBufferByte = 0;

uint8_t steppersForMove[STEPPERS_COUNT];
uint8_t countMoveSteppers = 0;

uint8_t getSteppersInMove()
{
  uint8_t steppersInMove = 0;
  for (int i = 0; i < countMoveSteppers; i++)
  {
    uint8_t stepper = steppersForMove[i];
    uint16_t stepperStatus = getStepper(stepper).getStatus() & STATUS_MOT_STATUS;

    if (stepperStatus != 0)
      steppersInMove++;
  }

  return steppersInMove;
}

uint32_t lastCommandId = 0;
uint8_t waitForCommandDone = 0;

//TODO: добавить проверку не завершенных команд
void addCommandForWait(uint32_t commandId)
{
  lastCommandId = commandId;
  waitForCommandDone = 1;
}

void executionMainLoop()
{
  readPacket();
  processPacket();
  printSteppersStates();

  if (waitForCommandDone != 0) // Есть команды, ожидающие завершения
  {
    if (getSteppersInMove() == 0) // Моторы завершили движение
    {
      printCommandStateResponse(lastCommandId, COMMAND_DONE);
      waitForCommandDone = 0;
    }
  }
}

void readPacket()
{
  currentBufferByte = 0;
  uint8_t countBytes = Serial.available();
  if (countBytes > 0)
  {
    while (countBytes != 0)
    {
      incomingBuffer[currentBufferByte++] = Serial.read();
      countBytes--;
    }
  }
}

enum ReceiveState
{
  RECEIVING_HEADER,
  RECEIVING_BODY
};

uint8_t packetBuffer[64];

void processPacket()
{
  uint8_t i = 0;

  uint8_t state = RECEIVING_HEADER;
  uint8_t currentHeaderByte = 0;
  uint8_t currentEndByte = 0;
  uint8_t currentPacketByte = 0;

  while (i != currentBufferByte)
  {
    switch (state)
    {
    case RECEIVING_HEADER:
    {
      if (incomingBuffer[i] == packetHeader[currentHeaderByte])
        currentHeaderByte++;
      else
        currentHeaderByte = 0;

      if (currentHeaderByte == packetHeaderLength)
      {
        state = RECEIVING_BODY;
        currentEndByte = 0;
        currentPacketByte = 0;
      }
    }
    break;
    case RECEIVING_BODY:
    {
      packetBuffer[currentPacketByte++] = incomingBuffer[i];

      if (incomingBuffer[i] == packetEnd[currentEndByte])
        currentEndByte++;
      else
        currentEndByte = 0;

      if (currentEndByte == packetEndLength)
      {
        currentHeaderByte = 0;
        state = RECEIVING_HEADER;

        uint8_t packetLength = currentPacketByte - packetEndLength;

        executeCommand(packetBuffer, packetLength);
      }
    }
    break;
    }
    i++;
  }
}

void executeCommand(uint8_t *packet, uint8_t packetLength)
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
      //TODO: Добавить обработку
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

String messageToSend = "";

bool checkStepper(uint8_t stepper)
{
  bool result = (stepper >= 0 && stepper < STEPPERS_COUNT) ? true : false;
  if(!result)
  {
    messageToSend = "wrong stepper = " + String(stepper);
    printMessage(messageToSend);
  }
  return result;
}

uint32_t readLong(uint8_t *buffer)
{
  return *((unsigned long *)(buffer));
}

void executeAbortCommand(uint8_t *packet, uint8_t packetLength)
{
  waitForCommandDone = 0;

  for(uint8_t i = 0; i < STEPPERS_COUNT; i++)
  {
    getStepper(i).softHiZ();
  }

  for(uint8_t i = 0;  i < 12; i++)
  {
    device_off(i);
  }

#ifdef DEBUG
  messageToSend = "[Abort] ";

  printMessage(messageToSend);
#endif
}

void executeGoUntilCommand(uint8_t *packet, uint8_t packetLength)
{
  uint8_t stepper = packet[0];
  uint8_t direction = packet[1];
  uint32_t fullSpeed = readLong(packet + 2);
  uint32_t packetId = readLong(packet + 6);

  if(!checkStepper(stepper)) return;

  printCommandStateResponse(packetId, COMMAND_OK);

  countMoveSteppers = 1;
  steppersForMove[0] = stepper;

  addCommandForWait(packetId);

  getStepper(stepper).goUntil(RESET_ABSPOS, direction, fullSpeed);

#ifdef DEBUG
  messageToSend = "[Go until] ";
  messageToSend += "stepper = " + String(stepper);
  messageToSend += ", dir = " + String(direction);
  messageToSend += ", speed = " + String(fullSpeed);

  printMessage(messageToSend);
#endif
}

void executeRunCommand(uint8_t *packet, uint8_t packetLength)
{
  uint8_t stepper = packet[0];
  uint8_t direction = packet[1];
  uint32_t fullSpeed = readLong(packet + 2);
  uint32_t packetId = readLong(packet + 6);

  if(!checkStepper(stepper)) return;

  printCommandStateResponse(packetId, COMMAND_OK);

  getStepper(stepper).run(direction, fullSpeed);

#ifdef DEBUG
  messageToSend = "[Run] ";
  messageToSend += "stepper = " + String(stepper);
  messageToSend += ", dir = " + String(direction);
  messageToSend += ", speed = " + String(fullSpeed);

  printMessage(messageToSend);
#endif
}

void executeMoveCommand(uint8_t *packet, uint8_t packetLength)
{
  uint8_t stepper = packet[0];
  uint8_t direction = packet[1];
  uint32_t steps = readLong(packet + 2);
  uint32_t packetId = readLong(packet + 6);

  if(!checkStepper(stepper)) return;

  printCommandStateResponse(packetId, COMMAND_OK);

  countMoveSteppers = 1;
  steppersForMove[0] = stepper;

  addCommandForWait(packetId);
  getStepper(stepper).move(direction, steps);

#ifdef DEBUG
  messageToSend = "[Move] ";
  messageToSend += "stepper = " + String(stepper);
  messageToSend += ", dir = " + String(direction);
  messageToSend += ", steps = " + String(steps);

  printMessage(messageToSend);
#endif
}

void executeStopCommand(uint8_t *packet, uint8_t packetLength)
{
  uint8_t stepper = packet[0];
  uint8_t stopType = packet[1];
  uint32_t packetId = readLong(packet + 2);

  if(!checkStepper(stepper)) return;

  printCommandStateResponse(packetId, COMMAND_OK);

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

void executeSetSpeedCommand(uint8_t *packet, uint8_t packetLength)
{
  uint8_t stepper = packet[0];
  uint32_t fullSpeed = readLong(packet + 1);
  uint32_t packetId = readLong(packet + 5);

  if(!checkStepper(stepper)) return;

  printCommandStateResponse(packetId, COMMAND_OK);

  getStepper(stepper).setMaxSpeed(fullSpeed);
  getStepper(stepper).setFullSpeed(fullSpeed);

#ifdef DEBUG
  messageToSend = "[Set speed] ";
  messageToSend += "stepper = " + String(stepper);
  messageToSend += ", speed = " + String(fullSpeed);

  printMessage(messageToSend);
# endif
}

void executeSetDeviceStateCommand(uint8_t *packet, uint8_t packetLength)
{
  uint8_t device = packet[0];
  uint8_t state = packet[1];
  uint32_t packetId = readLong(packet + 2);



  printCommandStateResponse(packetId, COMMAND_OK);
  device_set_state(device, state);

#ifdef DEBUG
  messageToSend = "[Set device state] ";
  messageToSend += "device = " + String(device);
  messageToSend += ", state = " + String(state);

  printMessage(messageToSend);
#endif
}

void executeCncMoveCommand(uint8_t *packet, uint8_t packetLength)
{
  uint8_t countOfSteppers = packet[0];
  uint32_t packetId = readLong(packet + countOfSteppers * 6 + 1);

  printCommandStateResponse(packetId, COMMAND_OK);

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

    if(checkStepper(stepper))
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

  addCommandForWait(packetId);
}

void executeCncHomeCommand(uint8_t *packet, uint8_t packetLength)
{
  uint8_t countOfSteppers = packet[0];

  uint32_t packetId = readLong(packet + countOfSteppers * 6 + 1);

  printCommandStateResponse(packetId, COMMAND_OK);

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
    
    if(checkStepper(stepper))
    {
      steppersForMove[i] = stepper;
      uint8_t sw_status = getStepper(stepper).getStatus() & STATUS_SW_F;
      if(sw_status != 0)
      {
        getStepper(stepper).goUntil(RESET_ABSPOS, direction, fullSpeed);
      }
      else
      {
        //TODO: добавить решение!!!
        //getStepper(stepper).releaseSw(RESET_ABSPOS, direction);
      }
    }

#ifdef DEBUG
    messageToSend = "[ stepper = " + String(stepper);
    messageToSend += ", dir = " + String(direction);
    messageToSend += ", fullSpeed = " + String(fullSpeed) + "] ";

    printMessage(messageToSend);
#endif
  }

  addCommandForWait(packetId);
}

void executeCncSetSpeedCommand(uint8_t *packet, uint8_t packetLength)
{
  uint8_t countOfSteppers = packet[0];
  
  uint32_t packetId = readLong(packet + countOfSteppers * 5 + 1);

  printCommandStateResponse(packetId, COMMAND_OK);

#ifdef DEBUG
  messageToSend = "[CNC Set speed] ";
  messageToSend += "Steppers = " + String(countOfSteppers);
  printMessage(messageToSend);
#endif

  for (int i = 0; i < countOfSteppers; i++)
  {
    uint8_t stepper = packet[i * 5 + 1];
    uint32_t fullSpeed = readLong(packet + i * 5 + 2);

    if(checkStepper(stepper))
    {
      getStepper(stepper).setMaxSpeed(fullSpeed);
      getStepper(stepper).setFullSpeed(fullSpeed);
    }

#ifdef DEBUG
    messageToSend = "[ stepper = " + String(stepper);
    messageToSend += ", fullSpeed = " + String(fullSpeed) + "] ";

    printMessage(messageToSend);
#endif
  }
}

void executeCncSetDeviceStateCommand(uint8_t *packet, uint8_t packetLength, uint8_t state)
{
  uint8_t countOfDevices = packet[0];

  uint32_t packetId = readLong(packet + countOfDevices * 1 + 1);

  printCommandStateResponse(packetId, COMMAND_OK);

#ifdef DEBUG
  messageToSend = "[CNC Set dev state] ";
  messageToSend += "devs = " + String(countOfDevices);
  printMessage(messageToSend);
#endif

  for (int i = 0; i < countOfDevices; i++)
  {
    uint8_t device = packet[i + 1];

    device_set_state(device, state);
#ifdef DEBUG
    messageToSend = "[ dev = " + String(device) + "] ";

    printMessage(messageToSend);
#endif
  }
}

void printCommandStateResponse(uint32_t commandId, uint8_t commandState)
{
  Serial.write(packetHeader, packetHeaderLength);
  Serial.write(COMMAND_STATE);
  Serial.write(commandState);
  Serial.write((byte *)&commandId, sizeof(commandId));
  Serial.write(packetEnd, packetEndLength);
}

void printMessage(String messageText)
{
  Serial.write(packetHeader, packetHeaderLength);
  Serial.write(TEXT_MESSAGE);
  Serial.println(messageText);
  Serial.write(packetEnd, packetEndLength);
}

void printSteppersStates()
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
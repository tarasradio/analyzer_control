#ifndef protocol_hpp
#define protocol_hpp

#include <Arduino.h>

static uint8_t packetHeader[] = {0x55, 0x55};
static uint8_t packetHeaderLength = 2;

static uint8_t packetEnd[] = {0xAA, 0xAA};
static uint8_t packetEndLength = 2;

enum CommandStates
{
  COMMAND_OK,
  COMMAND_BAD_FORMAT,
  COMMAND_DONE
};

enum Responses
{
  STEPPERS_STATES = 0x10,
  COMMAND_STATE = 0x11,
  TEXT_MESSAGE = 0x12
};

enum StepperCommands
{
  CMD_ABORT = 0x08,
  CMD_GO_UNTIL = 0x09,
  CMD_RUN = 0x10,
  CMD_MOVE = 0x11,
  CMD_STOP = 0x12,
  CMD_SET_SPEED = 0x13
};

enum AdditionalCommands
{
	CMD_SET_DEVICE_STATE = 0x14
};

enum CncCommands
{
	CNC_MOVE = 0x15,
	CNC_SET_SPEED,
	CNC_STOP,
	CNC_HOME,
	CNC_ON_DEVICE,
	CNC_OFF_DEVICE
};

#endif

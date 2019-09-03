#ifndef protocol_hpp
#define protocol_hpp

#include <Arduino.h>

static const uint8_t packetHeader[] = {0x55, 0x55};
static const uint8_t packetHeaderLength = 2;

static const uint8_t packetEnd[] = {0xAA, 0xAA};
static const uint8_t packetEndLength = 2;

enum CommandStates
{
  COMMAND_OK,
  COMMAND_BAD_FORMAT,
  COMMAND_DONE
};

enum Responses
{
  STEPPERS_STATES = 0x10,
  SENSORS_VALUES,
  COMMAND_STATE,
  TEXT_MESSAGE,
  BAR_CODE_MESSAGE
};

enum StepperCommands
{
  CMD_HOME = 0x09,
  CMD_RUN = 0x10,
  CMD_MOVE = 0x11,
  CMD_STOP = 0x12,
  CMD_SET_SPEED = 0x13
};

enum AdditionalCommands
{
	CMD_SET_DEVICE_STATE = 0x14,
	CMD_ABORT,
  CMD_WAIT_TIME,
  CMD_BAR_START
};

enum CncCommands
{
	CNC_MOVE = 0x18,
	CNC_SET_SPEED,
	CNC_STOP,
	CNC_HOME,
	CNC_ON_DEVICE,
	CNC_OFF_DEVICE,
  CNC_RUN
};

#endif

#ifndef protocol_hpp
#define protocol_hpp

#include <Arduino.h>

enum CommandStates
{
  COMMAND_OK,
  COMMAND_BAD_FORMAT,
  COMMAND_DONE
};

enum Responses
{
  FIRMWARE_VERSION = 0x0F,
  STEPPERS_STATES_MESSAGE = 0x10,
  SENSORS_VALUES_MESSAGE,
  COMMAND_STATE_MESSAGE,
  TEXT_MESSAGE,
  BARCODE_MESSAGE
};

enum StepperCommands
{
  CMD_HOME = 0x0F,
  CMD_RUN = 0x10,
  CMD_MOVE = 0x11,
  CMD_STOP = 0x12,
  CMD_SET_SPEED = 0x13
};

enum AdditionalCommands
{
	CMD_SET_DEVICE_STATE = CMD_SET_SPEED + 1,
	CMD_ABORT,
  CMD_WAIT_TIME,
  CMD_SCAN_BARCODE,
  CMD_GET_FIRMWARE_VERSION,
  CMD_SET_LED_COLOR,
};

enum CncCommands
{
	CNC_MOVE = CMD_SET_LED_COLOR + 1,
	CNC_SET_SPEED,
	CNC_STOP,
	CNC_HOME,
	CNC_ON_DEVICE,
	CNC_OFF_DEVICE,
  CNC_RUN,
};

class Protocol
{
public:
    static void sendFirmwareVersion(const char* version);
    static void sendMessage(const char* message);
    static void sendBarcode(uint8_t id, const char* barCode);
    static void sendSteppersStates(const uint16_t *steppersStates, uint8_t steppersCount);
    static void sendSensorsValues(const uint16_t *sensorsValues, uint8_t sensorsCount);
    static void sendCommandState(const uint32_t *commandId, uint8_t commandState);
};

#endif

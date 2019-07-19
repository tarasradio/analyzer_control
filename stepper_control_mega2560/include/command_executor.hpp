#ifndef command_executor_hpp
#define command_executor_hpp

#include <Arduino.h>

enum StopType
{
    STOP_SOFT = 0x00,
    STOP_HARD = 0x01,
    HiZ_SOFT = 0x02,
    HiZ_HARD = 0x03
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

void executionMainLoop();
#endif
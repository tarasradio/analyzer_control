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

void executionMainLoop();
#endif
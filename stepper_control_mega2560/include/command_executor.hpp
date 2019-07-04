#ifndef command_executor_hpp
#define command_executor_hpp

#include <Arduino.h>

enum StopType
{
    STOP_SOFT = 0x00,
    STOP_HARD = 0x01
};

void executionMainLoop();
#endif
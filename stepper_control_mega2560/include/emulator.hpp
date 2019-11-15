#ifndef emulator_h
#define emulator_h

#include <Arduino.h>

#define HOME_DELAY 5000
#define RUN_DELAY 5000
#define MOVE_DELAY 5000

class Emulator
{
private:

public:
  static void Init();
  static uint32_t GetElapsedMilliseconds();
  static const char* GetBarCodeMessage();
  static void RunTask();
  static void StopTask();
  static bool TaskIsRunning();
};

#endif
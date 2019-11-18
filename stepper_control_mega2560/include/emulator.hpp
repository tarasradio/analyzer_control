#ifndef emulator_h
#define emulator_h

#include <Arduino.h>

#define HOME_DELAY 2000
#define RUN_DELAY 2000
#define MOVE_DELAY 2000

class Emulator
{
private:

public:
  static void Init();
  static uint32_t GetElapsedMilliseconds();

  static bool BarCodeExist();
  static void NextBarCode();
  static const char* GetBarCodeMessage();
  
  static void RunTask();
  static void StopTask();
  static bool TaskIsRunning();
};

#endif
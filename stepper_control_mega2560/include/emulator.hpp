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
  static void init();
  static uint32_t getElapsedMilliseconds();

  static bool barCodeExist();
  static void nextBarCode();
  static const char* getBarCodeMessage();
  
  static void runTask();
  static void stopTask();
  static bool taskIsRunning();
};

#endif
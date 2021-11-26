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

  static bool barcodeExist();
  static void nextBarcode();
  static const char* getBarcodeMessage();
  
  static void runTask();
  static void stopTask();
  static bool taskIsRunning();
};

#endif
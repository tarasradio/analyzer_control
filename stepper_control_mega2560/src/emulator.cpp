#include "emulator.hpp"

static bool _taskIsRunning = false;
static uint32_t timer = 0;

static bool _barCodeExist = false;
static const char* barCodeMessages[3] = { "77FG89RT\0", "34QW45AZ\0", "34WE56AD\0" };
static uint8_t currentMessage = 0;

void Emulator::init()
{
  timer = 0;
  currentMessage = 0;
}

uint32_t Emulator::getElapsedMilliseconds()
{
  return millis() - timer;
}

void Emulator::runTask()
{
  timer = millis();
  _taskIsRunning = true;
}

void Emulator::stopTask()
{
  _taskIsRunning = false;
}

bool Emulator::taskIsRunning()
{
  return _taskIsRunning;
}

bool Emulator::barCodeExist()
{
  return _barCodeExist;
}

void Emulator::nextBarCode()
{
  _barCodeExist = true;
}

const char* Emulator::getBarCodeMessage()
{
  _barCodeExist = false;
  if(currentMessage < 3)
    return barCodeMessages[currentMessage++];
  else
  {
    return "\0";
  }
}
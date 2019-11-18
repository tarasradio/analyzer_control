#include "emulator.hpp"

static bool taskIsRunning = false;
static uint32_t timer = 0;

static bool barCodeExist = false;
static const char* barCodeMessages[3] = { "77FG89RT\0", "34QW45AZ\0", "34WE56AD\0" };
static uint8_t currentMessage = 0;

void Emulator::Init()
{
  timer = 0;
  currentMessage = 0;
}

uint32_t Emulator::GetElapsedMilliseconds()
{
  return millis() - timer;
}

void Emulator::RunTask()
{
  timer = millis();
  taskIsRunning = true;
}

void Emulator::StopTask()
{
  taskIsRunning = false;
}

bool Emulator::TaskIsRunning()
{
  return taskIsRunning;
}

bool Emulator::BarCodeExist()
{
  return barCodeExist;
}

void Emulator::NextBarCode()
{
  barCodeExist = true;
}

const char* Emulator::GetBarCodeMessage()
{
  barCodeExist = false;
  if(currentMessage < 3)
    return barCodeMessages[currentMessage++];
  else
  {
    return "\0";
  }
}
#include <Arduino.h>

#include "sensors.hpp"
#include "devices.hpp"
#include "steppers.hpp"
#include "task_manager.hpp"

#define BAUDRATE 115200
#define POLLING_TIMEOUT 50

TaskManager _taskManager;

void initLED()
{
  DDRA |= (1 << 5);
  DDRA |= (1 << 6);
  DDRA |= (1 << 7);

  PORTA |= (1 << 5);
  PORTA |= (1 << 6);
  PORTA |= (1 << 7);
}

void setup()
{
  initLED();

  Serial.begin(BAUDRATE);

  Devices::devices_init_pins();
  
  steppers_init_pins();
  steppers_reset();
  steppers_default_init();
}

void loop()
{
  _taskManager.TaskLoop();
  
  delay(POLLING_TIMEOUT);
}
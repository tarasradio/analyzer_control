#include <Arduino.h>

#include "sensors.hpp"
#include "devices.hpp"
#include "steppers.hpp"
//#include "command_executor.hpp"
#include "task_manager.hpp"

#define BAUDRATE 115200
#define POLLING_TIMEOUT 50

TaskManager _taskManager = TaskManager();

void setup()
{
  Serial.begin(BAUDRATE);

  devices_init_pins();
  
  steppers_init_pins();
  steppers_reset();
  steppers_default_init();
}

void loop()
{
  //executionMainLoop();
  _taskManager.TaskLoop();
  
  delay(POLLING_TIMEOUT);
}
#include <Arduino.h>

#include "sensors.hpp"
#include "devices.hpp"
#include "steppers.hpp"
#include "command_executor.hpp"

#define BAUDRATE 115200
#define POLLING_TIMEOUT 50

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
  executionMainLoop();
  
  delay(POLLING_TIMEOUT);
}
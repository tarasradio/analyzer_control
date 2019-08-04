#include "sensors.hpp"

int16_t Sensors::getSensorState(int sensorNumber)
{
  return analogRead(A0 + sensorNumber);
}
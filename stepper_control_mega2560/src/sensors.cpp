#include "sensors.hpp"

int16_t getSensorState(int sensorNumber)
{
  return analogRead(A0 + sensorNumber);
}
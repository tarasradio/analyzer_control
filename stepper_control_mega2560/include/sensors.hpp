#ifndef sensors_hpp
#define sensors_hpp

#include <Arduino.h>

class Sensors
{
public:
    static int16_t getSensorState(int sensorNumber);
};


#endif
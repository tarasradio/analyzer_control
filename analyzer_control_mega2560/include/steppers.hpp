#ifndef steppers_h
#define steppers_h

#include <Arduino.h>
#include <powerSTEP01ArduinoLibrary.h>
#include <SPI.h>

#define STEPPERS_COUNT 18

#define CS_PIN 53 // PB0
#define RESET_PIN 48 // PL1
#define FLAG_PIN 49 // PL0

class Steppers
{
private:

public:
    Steppers();
    static void initPins();
    static void reset();
    static void defaultInit();
    static powerSTEP& get(uint8_t stepper);
    static uint8_t getMoveState(uint8_t stepper);
};

#endif
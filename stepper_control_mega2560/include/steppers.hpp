#ifndef steppers_h
#define steppers_h

#include <Arduino.h>
#include <powerSTEP01ArduinoLibrary.h>
#include <SPI.h>

#define STEPPERS_COUNT 18

#define CS_PIN 53 // PB0
#define RESET_PIN 48 // PL1
#define FLAG_PIN 49 // PL0

void steppers_init_pins();
void steppers_reset();
void steppers_default_init();
powerSTEP getStepper(uint8_t stepper_id);

#endif
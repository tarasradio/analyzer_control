#include <Arduino.h>

#include "command_executor.hpp"
#include "packet_manager.hpp"

#include "sensors.hpp"
#include "devices.hpp"
#include "steppers.hpp"

#include "emulator.hpp"

#define BAUDRATE 115200
#define POLLING_TIMEOUT 50

BarcodeScanner tubeScanner(&Serial1, ScannerType::TubeScanner);
BarcodeScanner cartridgeScanner(&Serial2, ScannerType::CartridgeScanner);

MovingController moveController;
HomingController homeController;
RunningController runController;

CommandExecutor commandExecutor = CommandExecutor(  &homeController,
                                                    &runController, 
                                                    &moveController, 
                                                    &tubeScanner,
                                                    &cartridgeScanner);

PacketManager packetManager = PacketManager(&commandExecutor);

unsigned long timer;

void sendSteppersStates();
void sendSensorsValues();

void setup()
{
    Serial.begin(BAUDRATE);

    Emulator::init();
    
    Devices::initPins();

    Steppers::initPins();
    Steppers::reset();
    Steppers::defaultInit();

    Steppers::get(15).configStepMode(STEP_FS_32);

    Steppers::get(9).setAcc(1000);
    Steppers::get(9).setDec(1000);

    Steppers::get(10).setAcc(1000);
    Steppers::get(10).setDec(1000);

    timer = millis();

    commandExecutor.init_leds();
}

void loop()
{
    if (millis() - timer >= POLLING_TIMEOUT) 
    {
        timer = millis();

        packetManager.readPacket();
        packetManager.findByteStuffingPacket();
        
        cartridgeScanner.updateState();
        tubeScanner.updateState();
        
        commandExecutor.updateState();

        sendSteppersStates();
        sendSensorsValues();
    }

    static long timer2 = micros();

    if(micros() - timer2 >= 5000) {
        timer2 = micros();
        runController.updateState();
    }
}

void sendSteppersStates()
{
    uint16_t steppersStates[STEPPERS_COUNT];

    for (uint8_t i = 0; i < STEPPERS_COUNT; i++)
        steppersStates[i] =  Steppers::get(i).getStatus();

    Protocol::sendSteppersStates(steppersStates, STEPPERS_COUNT);
}

void sendSensorsValues()
{
    uint16_t sensorValues[16];

    for (uint8_t i = 0; i < 16; i++)
        sensorValues[i] =  Sensors::getSensorValue(i);

    Protocol::sendSensorsValues(sensorValues, 16);
}
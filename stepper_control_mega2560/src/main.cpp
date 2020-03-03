#include <Arduino.h>

#include "command_executor.hpp"
#include "packet_manager.hpp"

#include "sensors.hpp"
#include "devices.hpp"
#include "steppers.hpp"

#include "emulator.hpp"

#define BAUDRATE 115200
#define POLLING_TIMEOUT 50

BarScanner scanner;

MovingController moveController;
HomingController homeController;
RunningController runController;

CommandExecutor commandExecutor = CommandExecutor(  &homeController,
                                                    &runController, 
                                                    &moveController, 
                                                    &scanner);

PacketManager packetManager = PacketManager(&commandExecutor);

uint32_t timer;

void setup()
{
    Serial.begin(BAUDRATE);

    Emulator::init();
    
    Devices::initPins();

    Steppers::initPins();
    Steppers::reset();
    Steppers::defaultInit();

    Steppers::get(15).configStepMode(STEP_FS_32);

    timer = millis();
}

void loop()
{
    if (millis() - timer >= POLLING_TIMEOUT) 
    {
        timer = millis();

        packetManager.readPacket();
        packetManager.findByteStuffingPacket();
        commandExecutor.updateState();
    }
}
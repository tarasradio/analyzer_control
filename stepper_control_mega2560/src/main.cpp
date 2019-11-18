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

CommandExecutor commandExecutor =
    CommandExecutor(&homeController, &runController, &moveController, &scanner);

PacketManager packetManager = PacketManager(&commandExecutor);

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

    Emulator::Init();
    
    Devices::initPins();

    Steppers::initPins();
    Steppers::reset();
    Steppers::defaultInit();

    Steppers::get(15).configStepMode(STEP_FS_32);
}

void loop()
{
    packetManager.ReadPacket();
    packetManager.findByteStuffingPacket();
    commandExecutor.UpdateState();

    delay(POLLING_TIMEOUT);
}
#include "bar_scanner.hpp"

#include "packet_manager.hpp"

#define EMULATOR

byte barBuffer[64];
uint8_t currentBarByte = 0;

BarScanner::BarScanner()
{
    Serial1.begin(9600);
}

volatile int number = 0;

void BarScanner::updateState()
{
#ifdef EMULATOR
    //String message = "[Bar read] code = " + String("RedMary");
    //PacketManager::printMessage(message);
    if(number == 0)
    {
        PacketManager::printBarCode("RedMary4590");
    }
        
    return;
#endif
    currentBarByte = 0;
    while(Serial1.available() > 0)
    {
        byte symbol = Serial1.read();
        if('\r' == symbol)
        {
            // Обработка приема сообщения

            barBuffer[currentBarByte] = '\0';
            String message = "[Bar read] code = " + String((char*)barBuffer);
            PacketManager::printMessage(message);

            PacketManager::printBarCode(String((char*)barBuffer));

            currentBarByte = 0;
        }
        else
        {
            barBuffer[currentBarByte++] = symbol;
            if(currentBarByte >= 64)
            {
                // слишком длинное сообщение
                PacketManager::printMessage("[Bar read] overflow");
                currentBarByte = 0;
            }
        }
    }
}

const byte scanCommand[] = {0x7E, 0x00, 0x08, 0x01, 0x00, 0x02, 0x01, 0xAB, 0xCD};

void BarScanner::startScan()
{
#ifdef EMULATOR
    if(number == 0)
        number++;
#endif
    Serial1.write(scanCommand, 9);
}
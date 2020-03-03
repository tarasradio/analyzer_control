#include "system.hpp"
#include "emulator.hpp"
#include "bar_scanner.hpp"

#include "protocol.hpp"


byte barBuffer[64];
uint8_t currentBarByte = 0;

BarScanner::BarScanner()
{
    Serial1.begin(9600);
}

void BarScanner::updateState()
{
#ifdef EMULATOR
    if(Emulator::barCodeExist())
        Protocol::sendBarCode(Emulator::getBarCodeMessage());
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
#ifdef DUBUG
            String message = "[Bar read] code = " + String((char*)barBuffer);
            Protocol::SendMessage(message.c_str());
#endif
            Protocol::sendBarCode(String((char*)barBuffer).c_str());

            currentBarByte = 0;
        }
        else
        {
            barBuffer[currentBarByte++] = symbol;
            if(currentBarByte >= 64)
            {
                // слишком длинное сообщение
#ifdef DUBUG
                String message = "[Bar read] overflow";
                Protocol::SendMessage(message.c_str());
#endif
                currentBarByte = 0;
            }
        }
    }
}

const byte scanCommand[] = {0x7E, 0x00, 0x08, 0x01, 0x00, 0x02, 0x01, 0xAB, 0xCD};

void BarScanner::startScan()
{
#ifdef EMULATOR
    Emulator::nextBarCode();
#endif
    Serial1.write(scanCommand, 9);
}
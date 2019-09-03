#include "bar_scanner.hpp"

#include "packet_manager.hpp"

byte barBuffer[64];
byte currentBarByte = 0;

BarScanner::BarScanner()
{
    Serial1.begin(9600);
}

void BarScanner::updateState()
{
    currentBarByte = 0;
    while(Serial1.available() > 0)
    {
        byte symbol = Serial1.read();
        if('\r' == symbol)
        {
            // Обработка приема сообщения

            barBuffer[currentBarByte] = '\0';

            messageToSend = "[Bar read] ";
            messageToSend += "code = " + String((char*)barBuffer);
            PacketManager::printMessage(messageToSend);

            PacketManager::printBarCode(String((char*)barBuffer));

            currentBarByte = 0;
        }
        else
        {
            barBuffer[currentBarByte++] = symbol;
            if(currentBarByte >= 64)
            {
                // слишком длинное сообщение
            }
        }
    }
}

const byte scanCommand[] = {0x7E, 0x00, 0x08, 0x01, 0x00, 0x02, 0x01, 0xAB, 0xCD};

void BarScanner::startScan()
{
    Serial1.write(scanCommand, 9);
}
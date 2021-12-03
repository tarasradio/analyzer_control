#include "system.hpp"
#include "emulator.hpp"
#include "barcode_scanner.hpp"

#include "protocol.hpp"

byte barcodeBuffer[64];
uint8_t currentBarcodeByte = 0;

BarcodeScanner::BarcodeScanner(HardwareSerial * serialPort, uint8_t id)
{
    this->id = id;
    serial = serialPort;
    serial->begin(9600);
}

void BarcodeScanner::updateState()
{
#ifdef EMULATOR
    if(Emulator::barcodeExist())
        Protocol::sendBarcode(id, Emulator::getBarcodeMessage());
    return;
#endif

    currentBarcodeByte = 0;
    while(serial->available() > 0)
    {
        byte symbol = serial->read();
        if('\n' == symbol)
        {
            // Обработка приема сообщения

            barcodeBuffer[currentBarcodeByte] = '\0';
#ifdef DUBUG
            String message = "[Barcode scan] barcode = " + String((char*)barcodeBuffer);
            Protocol::sendMessage(message.c_str());
#endif
            Protocol::sendBarcode(id, String((char*)barcodeBuffer).c_str());

            currentBarcodeByte = 0;
        }
        else
        {
            barcodeBuffer[currentBarcodeByte++] = symbol;
            if(currentBarcodeByte >= 100)
            {
                // слишком длинное сообщение
#ifdef DUBUG
                String message = "[Barcode scan] overflow";
                Protocol::SendMessage(message.c_str());
#endif
                currentBarcodeByte = 0;
            }
        }
    }
}

const byte scanCommand[] = {0x7E, 0x00, 0x08, 0x01, 0x00, 0x02, 0x01, 0xAB, 0xCD};

void BarcodeScanner::startScan()
{
#ifdef EMULATOR
    Emulator::nextBarcode();
#endif
    serial->write(scanCommand, 9);
}
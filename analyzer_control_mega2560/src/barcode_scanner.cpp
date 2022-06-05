#include "system.hpp"
#include "emulator.hpp"
#include "barcode_scanner.hpp"

#include "protocol.hpp"

const byte startSeq [] = { 0x02, 0x00, 0x00, 0x01, 0x00, 0x33, 0x31 };
const byte scanCommand[] = { 0x7E, 0x00, 0x08, 0x01, 0x00, 0x02, 0x01, 0xAB, 0xCD };

char barcodeBuffer[BUFFER_SIZE];
unsigned tail = 0;
byte inChar;

BarcodeScanner::BarcodeScanner(HardwareSerial * serialPort, uint8_t id)
{
    this->id = id;
    serial = serialPort;
    serial->begin(9600);
}

bool BarcodeScanner::isSeq(char chr) {
  static uint8_t current = 0;
  if (chr != startSeq[current]) {
      current = 0;
      return false;
  } else {
      if (++current >= sizeof(startSeq)) {
          current = 0;
          return true;
      } else {
          return false;
      }
  }
}

void BarcodeScanner::updateState()
{
#ifdef EMULATOR
    if(Emulator::barcodeExist())
        Protocol::sendBarcode(id, Emulator::getBarcodeMessage());
    return;
#endif

  if (serial->available()) {
    serial->readBytes(&inChar, 1);
    if (isSeq((char)inChar)) {
      tail = 0;
      while (serial->readBytes(&inChar, 1) && inChar) {
        barcodeBuffer[tail++] = (char)inChar;
        if (tail == BUFFER_SIZE - 1) {
            break;
        }
      }
      barcodeBuffer[tail] = '\0';
      
      Protocol::sendBarcode(id, barcodeBuffer);
    }
  }
}

void BarcodeScanner::startScan()
{
#ifdef EMULATOR
    Emulator::nextBarcode();
#endif
    serial->write(scanCommand, 9);
}
#ifndef bar_scanner_hpp
#define bar_scanner_hpp

#include <Arduino.h>

class BarcodeScanner
{
private:
    HardwareSerial * serial;
    uint8_t id;
public:
    BarcodeScanner(HardwareSerial * serialPort, uint8_t id);
    void updateState();
    void startScan();
};

#endif
#ifndef bar_scanner_hpp
#define bar_scanner_hpp

#include <Arduino.h>

enum ScannerType {
    TubeScanner = 0x00,
    CartridgeScanner = 0x01
};

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
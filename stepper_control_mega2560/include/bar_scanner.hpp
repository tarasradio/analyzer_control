#ifndef bar_scanner_hpp
#define bar_scanner_hpp

#include <Arduino.h>

class BarScanner
{
private:
    HardwareSerial * serial;
    uint8_t id;
public:
    BarScanner(HardwareSerial * serialPort, uint8_t id);
    void updateState();
    void startScan();
};

#endif
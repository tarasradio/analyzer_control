#ifndef bar_scanner_hpp
#define bar_scanner_hpp

#include <Arduino.h>

class BarScanner
{
private:
    String messageToSend = "";
public:
    BarScanner();
    void updateState();
    void startScan();
};

#endif
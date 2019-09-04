#ifndef bar_scanner_hpp
#define bar_scanner_hpp

#include <Arduino.h>

class BarScanner
{
private:
    
public:
    BarScanner();
    void updateState();
    void startScan();
};

#endif
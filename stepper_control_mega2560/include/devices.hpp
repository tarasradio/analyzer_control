#ifndef devices_hpp
#define devices_hpp

#include <Arduino.h>

#define DEVICES_J_DDR DDRJ
#define DEVICES_J_PORT PORTJ // 1, 0

#define DEVICES_B_DDR DDRB
#define DEVICES_B_PORT PORTB // 5, 4

#define DEVICES_H_DDR DDRH
#define DEVICES_H_PORT PORTH // 3, 4, 5, 6

#define DEVICES_E_DDR DDRE
#define DEVICES_E_PORT PORTE // 3, 5, 4

#define DEVICES_G_DDR DDRG
#define DEVICES_G_PORT PORTG // 5

class Devices
{
public:
    static void devices_init_pins();
    static void device_on(uint8_t device);
    static void device_off(uint8_t device);
    static void device_set_state(uint8_t device, uint8_t state);
};

#endif
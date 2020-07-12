#include "devices.hpp"

void Devices::initPins()
{
    DEVICES_J_DDR |= (1 << 1) | (1 << 0);
    DEVICES_B_DDR |= (1 << 5) | (1 << 4);
    DEVICES_H_DDR |= (1 << 6) | (1 << 5) | (1 << 4) | (1 << 3);
    DEVICES_E_DDR |= (1 << 5) | (1 << 4) | (1 << 3);
    DEVICES_G_DDR |= (1 << 5);
}

void Devices::on(uint8_t device)
{
    setState(device, 1);
}

void Devices::off(uint8_t device)
{
    setState(device, 0);
}

void Devices::setState(uint8_t device, uint8_t state)
{
    if (device >= 0 && device <= 1) // VALVES_B : (1, 0)
    {
        device = (1 - device);
        state > 0 ? DEVICES_J_PORT |= (1 << device) : DEVICES_J_PORT &= ~(1 << device);
    }
    if (device >= 2 && device <= 3) // VALVES_B : (5 - 4)
    {
        device = (7 - device);
        state > 0 ? DEVICES_B_PORT |= (1 << device) : DEVICES_B_PORT &= ~(1 << device);
    }
    else if (device > 3 && device <= 7) // VALVES_H : (6 - 3)
    {
        device = (10 - device);
        state > 0 ? DEVICES_H_PORT |= (1 << device) : DEVICES_H_PORT &= ~(1 << device);
    }
    else if (device == 8) // VALVES_E : (3)
    {
        state > 0 ? DEVICES_E_PORT |= (1 << 3) : DEVICES_E_PORT &= ~(1 << 3);
    }
    else if (device == 9) // VALVES_G : (5)
    {
        state > 0 ? DEVICES_G_PORT |= (1 << 5) : DEVICES_G_PORT &= ~(1 << 5);
    }
    else if (device > 9 && device <= 11) // VALVES_E : (5, 4)
    {
        device = (15 - device);
        state > 0 ? DEVICES_E_PORT |= (1 << device) : DEVICES_E_PORT &= ~(1 << device);
    }
}

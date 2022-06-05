#include "steppers.hpp"

static powerSTEP steppers[STEPPERS_COUNT] = 
{
  powerSTEP(0, CS_PIN, RESET_PIN),
  powerSTEP(1, CS_PIN, RESET_PIN),
  powerSTEP(2, CS_PIN, RESET_PIN),
  powerSTEP(3, CS_PIN, RESET_PIN),
  powerSTEP(4, CS_PIN, RESET_PIN),
  powerSTEP(5, CS_PIN, RESET_PIN),
  powerSTEP(6, CS_PIN, RESET_PIN),
  powerSTEP(7, CS_PIN, RESET_PIN),
  powerSTEP(8, CS_PIN, RESET_PIN),
  powerSTEP(9, CS_PIN, RESET_PIN),
  powerSTEP(10, CS_PIN, RESET_PIN),
  powerSTEP(11, CS_PIN, RESET_PIN),
  powerSTEP(12, CS_PIN, RESET_PIN),
  powerSTEP(13, CS_PIN, RESET_PIN),
  powerSTEP(14, CS_PIN, RESET_PIN),
  powerSTEP(15, CS_PIN, RESET_PIN),
  powerSTEP(16, CS_PIN, RESET_PIN),
  //powerSTEP(17, CS_PIN, RESET_PIN)
};

powerSTEP & Steppers::get(uint8_t stepper)
{
  return steppers[stepper];
}

void Steppers::initPins()
{
  pinMode(RESET_PIN, OUTPUT);
  pinMode(CS_PIN, OUTPUT);
  pinMode(MOSI, OUTPUT);
  pinMode(MISO, OUTPUT);
  pinMode(SCK, OUTPUT);
}

void Steppers::reset()
{
  digitalWrite(RESET_PIN, HIGH);
  digitalWrite(RESET_PIN, LOW);
  digitalWrite(RESET_PIN, HIGH);
  digitalWrite(CS_PIN, HIGH);
}

void Steppers::defaultInit()
{
  SPI.begin();
  SPI.setDataMode(SPI_MODE3);

  for (int i = 0; i < STEPPERS_COUNT; i++)
  {
    steppers[i].SPIPortConnect(&SPI);
    steppers[i].configSyncPin(BUSY_PIN, 0);
    steppers[i].configStepMode(STEP_FS_128);

    steppers[i].setMaxSpeed(1000);
    steppers[i].setFullSpeed(2000);
    steppers[i].setAcc(2000);
    steppers[i].setDec(2000);

    steppers[i].setSlewRate(SR_520V_us);
    steppers[i].setOCThreshold(8);
    steppers[i].setOCShutdown(OC_SD_ENABLE);

    steppers[i].setPWMFreq(PWM_DIV_1, PWM_MUL_0_75);
    steppers[i].setVoltageComp(VS_COMP_DISABLE);
    steppers[i].setSwitchMode(SW_USER);
    steppers[i].setOscMode(INT_16MHZ);

    steppers[i].setRunKVAL(64);
    steppers[i].setAccKVAL(64);
    steppers[i].setDecKVAL(64);
    steppers[i].setHoldKVAL(32);

    steppers[i].setParam(ALARM_EN, 0x8F);

    steppers[i].getStatus();
  }
}

// 0 - not move, 1 - move
uint8_t Steppers::getMoveState(uint8_t stepper)
{
  uint16_t stepperStatus = steppers[stepper].getStatus() & STATUS_MOT_STATUS;
  return (uint8_t)stepperStatus;
}
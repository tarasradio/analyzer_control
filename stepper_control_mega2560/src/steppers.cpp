#include "steppers.hpp"

powerSTEP stepper_0(0, CS_PIN, RESET_PIN);
powerSTEP stepper_1(1, CS_PIN, RESET_PIN);
powerSTEP stepper_2(2, CS_PIN, RESET_PIN);
powerSTEP stepper_3(3, CS_PIN, RESET_PIN);
powerSTEP stepper_4(4, CS_PIN, RESET_PIN);
powerSTEP stepper_5(5, CS_PIN, RESET_PIN);
powerSTEP stepper_6(6, CS_PIN, RESET_PIN);
powerSTEP stepper_7(7, CS_PIN, RESET_PIN);
powerSTEP stepper_8(8, CS_PIN, RESET_PIN);
powerSTEP stepper_9(9, CS_PIN, RESET_PIN);
powerSTEP stepper_10(10, CS_PIN, RESET_PIN);
powerSTEP stepper_11(11, CS_PIN, RESET_PIN);
powerSTEP stepper_12(12, CS_PIN, RESET_PIN);
powerSTEP stepper_13(13, CS_PIN, RESET_PIN);
powerSTEP stepper_14(14, CS_PIN, RESET_PIN);
powerSTEP stepper_15(15, CS_PIN, RESET_PIN);
powerSTEP stepper_16(16, CS_PIN, RESET_PIN);
powerSTEP stepper_17(17, CS_PIN, RESET_PIN);

powerSTEP steppers[STEPPERS_COUNT] = {
  stepper_0,    stepper_1,    stepper_2,
  stepper_3,    stepper_4,    stepper_5, 
  stepper_6,    stepper_7,    stepper_8,
  stepper_9,    stepper_10,   stepper_11, 
  stepper_12,   stepper_13,   stepper_14,
  stepper_15,   stepper_16,   stepper_17 };

powerSTEP getStepper(uint8_t stepper_id)
{
  return steppers[stepper_id];
}

void steppers_init_pins()
{
  // Prepare pins
  pinMode(RESET_PIN, OUTPUT);
  pinMode(CS_PIN, OUTPUT);
  pinMode(MOSI, OUTPUT);
  pinMode(MISO, OUTPUT);
  pinMode(SCK, OUTPUT);
}

void steppers_reset()
{
  // Reset powerSTEP and set CS
  digitalWrite(RESET_PIN, HIGH);
  digitalWrite(RESET_PIN, LOW);
  digitalWrite(RESET_PIN, HIGH);
  digitalWrite(CS_PIN, HIGH);
}

void steppers_default_init()
{
  // Start SPI
  SPI.begin();
  SPI.setDataMode(SPI_MODE3);

  for(int i = 0; i < STEPPERS_COUNT; i++)
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
uint8_t get_stepper_move_state(uint8_t stepper)
{
  uint16_t stepperStatus = getStepper(stepper).getStatus() & STATUS_MOT_STATUS;
  return (uint8_t)stepperStatus;
}
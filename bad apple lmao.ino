#include <U8g2lib.h>
#include <Wire.h>
#include <SD.h>

U8G2_SH1106_128X64_NONAME_F_HW_I2C u8g2(U8G2_R0);

void setup() {
  Wire.setSDA(0);
  Wire.setSCL(1);
  SPI.setSCK(2);
  SPI.setTX(3);
  SPI.setRX(4);
  SPI.setCS(5);

  while (!SD.begin(5)) {
    Serial.println("SD Card failed, or not present");
    delay(1000);
  }
  Serial.begin(115200);
  while(!u8g2.begin()) {
    Serial.println("LCD failed");
    delay(1000);
  }
}
unsigned long lastTime = 0;
unsigned int frameCount = 0;
float fps = 0;
bool firstFrame = true;

unsigned int appleframe = 1;

void loop() {
  if (u8g2.availableForWrite() > 0) {
    Serial.println("u8g2 unavailable for write");
    return;
  }

  long currentTime = millis();
  fps = (1000.0f / (float)(currentTime - lastTime));
  lastTime = currentTime;

  // play video
  u8g2.clearBuffer();
  drawFile(0, 0, appleframe);
  u8g2.sendBuffer();
  Serial.println("Frame: " + String(appleframe) + "/3483" + " FPS: " + String(fps, 4));
  appleframe++;

  if (appleframe > 3483) appleframe = 1;
  // limit to 15 fps
  if ((long)(1000/15) - ((long)millis() - currentTime) > 0) {
    delay(1000 / 15 - (millis() - currentTime));
  }
}

void drawFile(u8g2_int_t x, u8g2_int_t y, int frame) {
  uint8_t w;
  uint8_t h;
  uint8_t b;
  uint8_t mask;
  uint8_t len;
  u8g2_int_t xpos;
  File myFile = SD.open("Complete.bin");
  if (myFile) {
    int readbytes = frame * 1026;
    myFile.seek(readbytes);
    w = myFile.read(); 
    h = myFile.read();
    // expecting 128x64

    while( h > 0 ) { 
      xpos = x;
      len = w;               
      mask = 1;
      b = myFile.read();  
      
      
      for(;;) {      
        if ( b & mask ) {   
          u8g2.setDrawColor(0);
          u8g2.drawPixel(xpos,y);
        } else {
          u8g2.setDrawColor(1);
          u8g2.drawPixel(xpos,y);
        }
        xpos++;      
        mask <<= 1;  
        len--;              
        if ( len == 0 )
            break;
        if ( mask == 0 )
        {
          mask = 1;           
          b = myFile.read();  
        }
      }      
      
      y++;      
      h--;      
    }
    myFile.close();      
  }
  u8g2.setDrawColor(1);
}
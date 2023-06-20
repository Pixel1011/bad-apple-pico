# Bad apple on Pi Pico

Played on a 128x64 SH1106 display with a Raspberry Pi Pico W. <br>
Built on [arduino-pico core](https://github.com/earlephilhower/arduino-pico) and runs at about 15 fps at 133Mhz and with -Os optimisation.


## Wiring

Screen SDA -> GP0 <br>
Screen SCL -> GP1 <br>

SD SCK -> GP2 <br>
SD TX/MOSI -> GP3 <br>
SD RX/MISO -> GP4 <br>
SD CS -> GP5 <br>

note: got bored so made this in 2 days, was fun to actually get video running on that screen
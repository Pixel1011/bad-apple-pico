# Bad apple on Pi Pico

Played on a 128x64 SH1106 display with a Raspberry Pi Pico W.
Built on [arduino-pico core](https://github.com/earlephilhower/arduino-pico) and runs at about 15 fps at 133Mhz and with -Os optimisation.

## Wiring

Screen SDA -> GP0
Screen SCL -> GP1

SD SCK -> GP2
SD TX/MOSI -> GP3
SD RX/MISO -> GP4
SD CS -> GP5

note: got bored so made this in 2 days, was fun to actually get video running on that screen
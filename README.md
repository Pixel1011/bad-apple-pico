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

FFMPEG command to create xbm files from video:
```
.\ffmpeg.exe -i 'video.mp4' -filter_complex "color=c=0x000000:r=1:d=1:s=8x16,format=rgb24[b];color=c=0xffffff:r=100:d=1:s=8x16,format=rgb24[w];[b][w]hstack=2[bw];[0:V:0][bw]paletteuse=new=false:dither=none,format=yuv420p[out]" -map "[out]" out%03d.xbm
``````

note: got bored so made this in 2 days, was fun to actually get video running on that screen
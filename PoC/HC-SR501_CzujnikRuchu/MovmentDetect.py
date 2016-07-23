#!/usr/bin/env python

import RPi.GPIO as GPIO
import time

PIR = 26                # On-board pin number 7 (GPIO04)
val = False
GPIO.setmode(GPIO.BOARD)        # Change this if using GPIO numbering
GPIO.setup(PIR, GPIO.IN)        # Set PIR as input

try:
    while True:
        time.sleep(1)
        val = GPIO.input(PIR)           
        if (val == True):               # check if the input is HIGH
            print 'moooove!'
except KeyboardInterrupt:
        GPIO.cleanup()
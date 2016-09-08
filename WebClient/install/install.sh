#!/bin/bash

# X Server and Chrome: https://www.raspberrypi.org/forums/viewtopic.php?t=121195
wget -qO - http://bintray.com/user/downloadSubjectPublicKey?username=bintray | sudo apt-key add -
echo "deb http://dl.bintray.com/kusti8/chromium-rpi jessie main" | sudo tee -a /etc/apt/sources.list
sudo apt-get update
sudo apt-get install xorg chromium-browser

# Unclutter - removes cursor
sudo apt-get install unclutter

# Apache & php
sudo apt-get install apache2 apache2-doc apache2-utils
sudo apt-get install libapache2-mod-php5 php5 php-pear php5-xcache
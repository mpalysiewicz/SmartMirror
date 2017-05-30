sudo apt-get install mongodb nodejs npm

#ln -s /usr/bin/nodejs /usr/bin/node

mv /var/SensorsService /var/sensors-service
mv /var/DomoticzDataPublisher /var/domoticz-data-publisher

cd /var/sensors-service
sudo npm install

cd /var/domoticz-data-publisher
sudo npm install

sudo systemctl enable /var/sensors-service/sensors.service
sudo systemctl enable /var/domoticz-data-publisher/domoticz-data-publisher.service
sudo systemctl daemon-reload
sudo systemctl start sensors
sudo systemctl start domoticz-data-publisher
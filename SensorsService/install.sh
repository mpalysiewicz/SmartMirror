sudo apt-get install mongodb nodejs npm

#ln -s /usr/bin/nodejs /usr/bin/node

mv /var/SensorsService /var/sensors-service
mv /var/DomoticDataPublisher /var/domotic-data-publisher

cd /var/sensors-service
sudo npm install

cd /var/domotic-data-publisher
sudo npm install

sudo systemctl enable /var/sensors-service/sensors.service
sudo systemctl enable /var/domotic-data-publisher/domotic-data-publisher.service
sudo systemctl daemon-reload
sudo systemctl start sensors
sudo systemctl start domotic-data-publisher
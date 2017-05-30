var request = require('request');
var interval = 60000;
var domoticzUrl = 'http://192.168.1.7:8080';
var sensorsServiceUrl = 'http://192.168.1.8';

function sleep(time) {
    return new Promise(function(resolve){
		setTimeout(resolve, time)
	});
}

function ReadFromDomoticz() {
    console.log("Reading from Domoticz" + GetCurrentDateTime());
    ReadAndPost(141, "salon_hum", "Salon wilgotność", "Humidity", "%");
    ReadAndPost(141, "salon_temp", "Salon temperatura", "Temp", "°C");
    ReadAndPost(269, "antresola_dust", "Czujnik pyłu", "Data", "ppm"); //"µg/m³"
    setTimeout(function () { ReadFromDomoticz() }, interval);
}

function ReadAndPost(domo_id, serv_id, name, property, unit) {
    request.get(domoticzUrl+'/json.htm?type=devices&rid=' + domo_id,
        function (error, response, body) {
            var result = JSON.parse(body).result[0];
            var value = parseValue(result[property],serv_id.split("_")[1]);
            post(serv_id, name, result.LastUpdate, value, unit);           
        });      
}

function parseValue(input, type) {
    if (type == 'dust')
        return input.split(" ")[0];
    else
        return input;
}

function post(id, name, timestamp, value, unit) {
    console.log("Sending: " + id + " " + name + " " + timestamp + " " + value + unit);
    request.post(
        sensorsServiceUrl + '/save',
        {
            json: {
                "name": name,
                "id": id,
                "data": {
                    "measurement_time": timestamp,
                    "value": value,
                    "unit": unit
                }
            }
        },
        function (error, response, body) {         
            if (!error && response.statusCode == 200) {
                console.log(body)
            }
        }
    );
}

function GetCurrentDateTime() {
    var now = new Date();
    return now.getFullYear() + "-" + now.getMonth() + "-" + now.getDay() + " " + now.getHours() + ":" + now.getMinutes() + ":" + now.getSeconds();
}

ReadFromDomoticz();
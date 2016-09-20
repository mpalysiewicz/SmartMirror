var request = require('request');
var interval = 60000;


function sleep(time) {
    return new Promise((resolve) => setTimeout(resolve, time));
}

function ReadFromDomotic() {
    console.log("Reading from Domotic" + GetCurrentDateTime());
    ReadAndPost(2, "room2_hum", "Room 2 humidity", "Humidity", "%");
    ReadAndPost(2, "room2_temp", "Room 2 temperature", "Temp", "°C");
    ReadAndPost(5, "room2_dust", "Room 2 dust", "Data", "ppm"); //"µg/m³"
    setTimeout(function () { ReadFromDomotic() }, interval);
}

function ReadAndPost(domo_id, serv_id, name, property, unit) {
    request.get('http://10.3.55.17:8080/json.htm?type=devices&rid=' + domo_id,
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
        'http://10.3.54.74:8082/save',
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

ReadFromDomotic();
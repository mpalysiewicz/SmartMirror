var request = require('request');
var interval = 30000;


//5, 'Humidity', '%'

function ReadFromDomotic() {
    console.log("Reading from Domotic" + GetCurrentDateTime());
    request.get('http://10.3.55.17:8080/json.htm?type=devices&rid=' + 4,            
        function (error, response, body) {
            var result = JSON.parse(body).result[0];
            post("room2_hum", result.LastUpdate, result.Humidity, "%");   
            console.log("Wait " + GetCurrentDateTime());        
            sleep(30000).then(ReadFromDomotic());
            
        });            
}

function post(id, timestamp, value, unit) {
    //var timestamp = GetCurrentDateTime();
    request.post(
        'http://10.3.54.74:8082/save',
        {
            json: {
                "name": "Room 4 Temerature",
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

function sleep(time) {
    return new Promise((resolve) => setTimeout(resolve, time));
}

function GetCurrentDateTime() {
    var now = new Date();
    return now.getFullYear() + "-" + now.getMonth() + "-" + now.getDay() + " " + now.getHours() + ":" + now.getMinutes() + ":" + now.getSeconds();
}

ReadFromDomotic();
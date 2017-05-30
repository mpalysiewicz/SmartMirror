var request = require('request');

request.post(
    'http://localhost:8082/save',
    { json: {
        "name": "Salon Temerature",
        "id": "room2_temp",
        "data": {
            "measurement_time": "2016-09-08 20:06:15",
            "value": "22,2",
            "unit": "C"
        }   
    } },
    function (error, response, body) {
        if (!error && response.statusCode == 200) {
            console.log(body)
        }
    }
);
var clientFromConnectionString = require('azure-iot-device-amqp').clientFromConnectionString;
var Message = require('azure-iot-device').Message;
var Amqp = require('azure-iot-device-amqp').Amqp;

var connectionStrings = [
    'HostName=MagicMirror.azure-devices.net;DeviceId=room2_hum;SharedAccessKey=9q5FR9Y3JXxirIYXPyWJ6Sa7YOWk0BMb7xvjT25dHwQ=',
    'HostName=MagicMirror.azure-devices.net;DeviceId=room2_temp;SharedAccessKey=ipmUX0yNYS4CvSmsEtUzXCLrw2SUqGQLvNVm5NIu29I=',
    'HostName=MagicMirror.azure-devices.net;DeviceId=room2_dust;SharedAccessKey=4RdSmrJbZQ8eprV4OWANoK6+cf8JmaLN+L70bdn5LEs='
]

var client;
var currentdata;

function publish(sensordata, callback) {

    for (var i = 0; i < connectionStrings.length; i++) {
        if (connectionStrings[i].indexOf(sensordata.id) !== -1) {
            client = clientFromConnectionString(connectionStrings[i], Amqp);
            break;
        }
    }

    if(client === undefined) {
        console.log('Unknown sensor: ', sensordata.id);
        return;
    }

    currentdata = sensordata;
    client.open(connectCallback);
    
}

var connectCallback = function(err) {
    if (err) {
        console.log('Could not connect: ' + err.message);
    } else {
        console.log('Client connected');
        client.on('message', function (msg) {
            console.log('Id: ' + msg.messageId + ' Body: ' + msg.data);
            client.complete(msg, printResultFor('completed'));

        });

        var data = JSON.stringify({ deviceId: currentdata.id, value: currentdata.data.value });
        var message = new Message(data);
        message.properties.add('Timestamp', currentdata.data.measurement_time);
        console.log('Sending message: ' + message.getData());
        client.sendEvent(message, printResultFor('send'));

        client.on('error', function (err) {
            console.log(err.message);
        });

        client.on('disconnect', function () {
            clearInterval(sendInterval);
            client.removeAllListeners();
            client.connect(connectCallback);
        });
    }
}

function printResultFor(op) {
    return function printResult(err, res) {
        if (err) console.log(op + ' error: ' + err.toString());
        if (res) console.log(op + ' status: ' + res);
    };
}

module.exports = {
    publish : publish
};
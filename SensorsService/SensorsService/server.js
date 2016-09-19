var express = require('express');
var app = express();
var fs = require("fs");
var mongo = require('./mongo');

app.use(bodyParser.json());

//GET all data
app.get('/listSensorsData', function (req, res) {
    mongo.readAllSensorsData(function(status, data) { res.send(data)});
})

//GET sensor last measurement
app.get('/:id/lastValue', function (req, res) {
    mongo.readLastSensorData(req.params.id, function(status, data) { res.send(data)});
})

//GET a specific sensor all data
app.get('/:id', function (req, res) {
    mongo.readSensorData(req.params.id, function(status, data) { res.send(data) });
})

app.post('/save', function (req, res) {
    mongo.addMeasurement(req.body, function(status, message) { res.send({status: status, message: message })});
});

var server = app.listen(8082, function () {
    var host = server.address().address
    var port = server.address().port

    console.log("Example app listening at http://%s:%s", host, port)
})
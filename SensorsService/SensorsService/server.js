var mongo = require('./mongo');
var express = require('express');
var app = express();
var fs = require("fs");
var bodyParser = require('body-parser');

app.use(bodyParser.json());

//GET all data
app.get('/listSensorsData', function (req, res) {
    mongo.readAllSensorsData(function(status, data) { res.send({status: status, data: data })});
    /*fs.readFile(__dirname + "/" + "sensorsData.json", 'utf8', function (err, data) {
        console.log(data);
        res.end(data);
    });*/
})

//GET sensor last measurement
app.get('/:id/lastValue', function (req, res) {
    mongo.readLastSensorData(req.params.id, function(status, data) { res.send(data)});
    /*fs.readFile(__dirname + "/" + "sensorsData.json", 'utf8', function (err, data) {
        sensorsData = JSON.parse(data);
        var result = sensorsData[req.param('id')]        
        var lastMeasurement = result['data'].pop()
        console.log(lastMeasurement);
        res.end(JSON.stringify(lastMeasurement));
    });*/
})

//GET a specific sensor all data
app.get('/:id', function (req, res) {
    mongo.readSensorData(req.params.id, function(status, data) { res.send(data) });
    /*fs.readFile(__dirname + "/" + "sensorsData.json", 'utf8', function (err, data) {
        sensorsData = JSON.parse(data);
        var result = sensorsData[req.params.id]        
        console.log(result);
        res.end(JSON.stringify(result));
    });*/
})

app.post('/save', function (req, res) {
    console.log(req.body);
    mongo.addMeasurement(req.body, function(status, message) { res.send({status: status, message: message })});
    //fs.appendFileSync(__dirname + "/" + "sensorsData.json", JSON.stringify(req.body), 'utf-8');
});

var server = app.listen(8082, function () {

    var host = server.address().address
    var port = server.address().port

    console.log("Example app listening at http://%s:%s", host, port)
})
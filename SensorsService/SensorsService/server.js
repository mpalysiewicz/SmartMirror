var express = require('express');
var app = express();
var fs = require("fs");
var bodyParser = require('body-parser');

app.use(bodyParser.json());

var mongodb = require('mongodb');
var mongoClient = mongodb.MongoClient;
var url = 'mongodb://10.3.54.74:27017/sensors';

//GET all data
app.get('/listSensorsData', function (req, res) {
    fs.readFile(__dirname + "/" + "sensorsData.json", 'utf8', function (err, data) {
        console.log(data);
        res.end(data);
    });
})

//GET sensor last measurement
app.get('/lastValue', function (req, res) {    
    fs.readFile(__dirname + "/" + "sensorsData.json", 'utf8', function (err, data) {
        sensorsData = JSON.parse(data);
        var result = sensorsData[req.param('id')]        
        var lastMeasurement = result['data'].pop()
        console.log(lastMeasurement);
        res.end(JSON.stringify(lastMeasurement));
    });
})

//GET a specific sensor all data
//app.get('/:id', function (req, res) {    
//    fs.readFile(__dirname + "/" + "sensorsData.json", 'utf8', function (err, data) {
//        sensorsData = JSON.parse(data);
//        var result = sensorsData[req.params.id]        
//        console.log(result);
//        res.end(JSON.stringify(result));
//    });
//})

app.post('/save', function (req, res) {
    console.log(req.body);
    fs.appendFileSync(__dirname + "/" + "sensorsData.json", JSON.stringify(req.body), 'utf-8');

    mongoClient.connect(url, function (err, db) {
        if (err) {
            console.log('Unable to connect to the mongoDB server. Error:', err);
        } else {
            //HURRAY!! We are connected. :)
            console.log('Connection established to', url);

            // Get the documents collection
            var collection = db.collection('users');

            //Create some users
            var user1 = {name: 'modulus admin', age: 42, roles: ['admin', 'moderator', 'user']};
            var user2 = {name: 'modulus user', age: 22, roles: ['user']};
            var user3 = {name: 'modulus super admin', age: 92, roles: ['super-admin', 'admin', 'moderator', 'user']};

            // Insert some users
            collection.insert([user1, user2, user3], function (err, result) {
                if (err) {
                    console.log(err);
                } else {
                    console.log('Inserted %d documents into the "users" collection. The documents inserted with "_id" are:', result.length, result);
                }
                //Close connection
                db.close();
            });
        }
    });
});


var server = app.listen(8081, function () {

    var host = server.address().address
    var port = server.address().port

    console.log("Example app listening at http://%s:%s", host, port)

})
var mongodb = require('mongodb');
var mongoClient = mongodb.MongoClient;
var url = 'mongodb://localhost:27017/sensors';
//var url = 'mongodb://10.3.54.74:27017/sensors';
var mdb;

mongoClient.connect(url, function (err, db) {
    if (err) {
        console.log('Unable to connect to the mongoDB server. Error:', err);
        res.send('Error: ', err)
    } else {
        mdb = db;
        //db.close()
    }
});

module.exports = {

    addMeasurement: function (data, callback) {
        if (mdb === undefined) {
            console.log('Error: mongoDB is not accessible.');
            callback(false, 'Error: mongoDB is not accessible.');
        } else {
            var collection = mdb.collection(data.id);    
            collection.insert([data], function (err, result) {
                if (err) {
                    console.log('Unable to insert document. Error: ', err);
                    callback(false, 'Unable to insert document. Error: ' + err);
                } else {
                    console.log('Inserted %d documents into the "users" collection. The documents inserted with "_id" are:', result.length, result);
                    callback(true);
                }
            });
        }
    },

    readSensorData: function (sensorId, callback) {
        if (mdb === undefined) {
            console.log('Error: mongoDB is not accessible.');
            callback(false, 'Error: mongoDB is not accessible.');
        } else {
            var collection = mdb.collection(sensorId);    
            var data = collection.find().sort({"data.measurement_time": 1}).toArray(function (err, result) {
                if (err) {
                    console.log('Unable to read collection. Error: ', err);
                    callback(false, 'Unable to read collection. Error: ' + err);
                } else {
                    var data = {
                        id: result[0].id,
                        name: result[0].name,
                        data: []
                    };
                    for (var i in result) {
                        data.data.push(result[i].data);
                    }
                    console.log(data);
                    callback(true, data);
                }
            });
        }
    },

    readLastSensorData: function (sensorId, callback) {
        if (mdb === undefined) {
            console.log('Error: mongoDB is not accessible.');
            callback(false, 'Error: mongoDB is not accessible.');
        } else {
            console.log(sensorId);
            var collection = mdb.collection(sensorId);    
            var data = collection.find().sort({"data.measurement_time": -1}).nextObject(function (err, result) {
                if (err) {
                    console.log('Unable to read collection. Error: ', err);
                    callback(false, 'Unable to read collection. Error: ' + err);
                } else {
                    console.log(result);
                    callback(true, result);
                }
            });
        }
    },
    
    readAllSensorsData: function (callback) {
        if (mdb === undefined) {
            console.log('Error: mongoDB is not accessible.');
            callback(false, 'Error: mongoDB is not accessible.');
        } else {
            var alldata = {};

            mdb.collections(function (err, items) {
                if (err) {
                    console.log('Unable to read database. Error: ', err);
                    callback(false, 'Unable to read database. Error: ' + err);
                } else {
                    callback(true, JSON.stringify(items, censor(items)));
                }
            });

            /*db.getCollectionNames().forEach(function(element) {
                var data = collection.find().toArray(function (err, result) {
                    if (err) {
                        console.log('Unable to insert document. Error: ', err);
                        callback(false, 'Unable to insert document. Error: ' + err);
                    } else {

                    }
                });
            }, this);*/

            console.log(alldata);
            callback(true, alldata);
        }
    }

};
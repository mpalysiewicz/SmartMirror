var mongodb = require('mongodb');
var mongoClient = mongodb.MongoClient;
var url = 'mongodb://localhost:27017/sensors';
//var url = 'mongodb://10.3.54.74:27017/sensors';
var mdb;

mongoClient.connect(url, function (err, db) {
    if (err) {
        console.log('Unable to connect to the mongoDB server. Error:', err);
        //res.send('Error: ', err)
    } else {
        mdb = db;
        //db.close()
    }
});

function addMeasurement(data, callback) {
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
};

function readSensorData(sensorId, callback) {
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
                if(result == null || result.length == 0) {
                    callback(true, {});
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
            }
        });
    }
};

function readLastSensorData(sensorId, callback) {
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
                if(result == null)
                    callback(true, {});
                else
                    callback(true, result);
            }
        });
    }
};

function readAllSensorsDataRecursively(sensors, sensorIndex, allData, callback) {
    if(sensors.length == sensorIndex)
    {
        callback(true, allData);
    } else {
        readSensorData(sensors[sensorIndex], function(status, sensorData){
            allData.push(sensorData);
            readAllSensorsDataRecursively(sensors, sensorIndex+1, allData, callback);
        })
    }
}
    
function readAllSensorsData(callback) {
    if (mdb === undefined) {
        console.log('Error: mongoDB is not accessible.');
        callback(false, 'Error: mongoDB is not accessible.');
    } else {
        mdb.collections(function (err, items) {
            if (err) {
                console.log('Unable to read database. Error: ', err);
                callback(false, 'Unable to read database. Error: ' + err);
            } else {
                var sensors = [];
                for(var i in items)
                {
                    if(items[i].collectionName.length >= 6 && items[i].collectionName.substring(0, 6) === 'system')
                        continue;
                    sensors.push(items[i].collectionName)
                }
                readAllSensorsDataRecursively(sensors, 0, [], function(status, alldata) {
                    callback(true, alldata);
                })
            }
        });
    }
};

module.exports = {
    addMeasurement: addMeasurement,
    readSensorData: readSensorData,
    readLastSensorData: readLastSensorData,
    readAllSensorsData: readAllSensorsData
};
(function(){
    'use strict';

    function SensorService($http){
        var service = {};
        service.sensorList = null;

        service.init = function(){
            for(var i=0; i<config.sensorService.sensors.length; i++){
                service.sensorList[i] = getJsonData(config.sensorService.address, config.sensorService.sensors[i].id, config.sensorService.sensors[i].name);
            }
        }
    }
    function getJsonData(address,id,name){
        var value;
        $http.jsonp(address+id+'/lastValue').success(function(data) {
            value = data[1]+data[2];
        })
        return value;
    }
    angular.module('SmartMirror').factory('SensorService',SensorService);
})
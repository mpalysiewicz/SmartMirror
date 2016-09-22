(function() {
    'use strict';


    function LightService($http, $translate) {
        var service = {};
        var SaidParameter = {};
        // update lights
        service.performUpdate = function(spokenWords){
            // split string into separate words and remove empty ones
            var spokenWords = spokenWords.toLowerCase().split(" ").filter(Boolean);
   
            // what locations are defined in the config
            var definedLocations = [];
            for(var i = 0; i < config.domoticz.groups.length; i++){
                definedLocations.push(config.domoticz.groups[i].name.toLowerCase());
            }

            SaidParameter['locations'] = [];
            SaidParameter['on'] = "On";

            // what has been said
            for(var i = 0; i < spokenWords.length; i++){
                var index = definedLocations.indexOf(spokenWords[i]);
                if(index > -1){
                    SaidParameter['locations'].push(index);
                }

                // turn lights on or off?
                if($translate.instant('lights.action.off') == spokenWords[i]){
                    SaidParameter['on'] = "Off";
                }


                // Adjust brightness
                if(spokenWords[i] == '100%' || $translate.instant('lights.intensity.max').includes(spokenWords[i])){
                    SaidParameter['brightness'] = 1.0;
                } else if(spokenWords[i] == '10%'){
                    SaidParameter['brightness'] = 0.1;
                } else if(spokenWords[i] == '20%'){
                    SaidParameter['brightness'] = 0.2;
                } else if(spokenWords[i] == '25%' || $translate.instant('lights.intensity.quarter').includes(spokenWords[i])){
                    SaidParameter['brightness'] = 0.25;
                } else if(spokenWords[i] == '30%'){
                    SaidParameter['brightness'] = 0.3;
                } else if(spokenWords[i] == '40%'){
                    SaidParameter['brightness'] = 0.3;
                } else if(spokenWords[i] == '50%' || $translate.instant('lights.intensity.half').includes(spokenWords[i])){
                    SaidParameter['brightness'] = 0.5;
                } else if(spokenWords[i] == '60%'){
                    SaidParameter['brightness'] = 0.6;
                } else if(spokenWords[i] == '70%'){
                    SaidParameter['brightness'] = 0.7;
                } else if(spokenWords[i] == '75%' || $translate.instant('lights.intensity.threequarter').includes(spokenWords[i])){
                    SaidParameter['brightness'] = 0.75;
                } else if(spokenWords[i] == '80%'){
                    SaidParameter['brightness'] = 0.8;
                } else if(spokenWords[i] == '90%'){
                    SaidParameter['brightness'] = 0.9;
                }


                // reset all LED
                if($translate.instant('lights.action.reset').includes(spokenWords[i])){
                    localStorage.clear()
                }

            }

            // if spoken words contain no location, use all defined locations
            if(SaidParameter['locations'].length == 0){
                for(var i = 0; i < definedLocations.length; i++){
                    SaidParameter['locations'].push(i);
                }
            }

            var SavedSettings = [];

            // get remaining info from local storage
            for(var j = 0 ; j < SaidParameter['locations'].length; j++){
                var i = SaidParameter['locations'][j];
                var SavedSetting = {};
                // read settings from storage or use default
                if(localStorage.getItem('Light_Setup_' + i) == null){
                    SavedSetting['brightness'] = 0.4
                }
                else{
                    SavedSetting = JSON.parse(localStorage.getItem('Light_Setup_' + i));
                }

                // overwrite settings with spoken info
                for(var key in SaidParameter){
                    if(SaidParameter.hasOwnProperty(key)) {
                        SavedSetting[key] = SaidParameter[key];
                    }
                }

                // save new values in local storage
                localStorage.setItem('Light_Setup_' + i, JSON.stringify(SavedSetting));

                SavedSetting['location'] = i;

                SavedSettings.push(SavedSetting);
            }

            SavedSettings.map(updateLights);
        }

        function updateLights(setting){
            var index = setting['location'];
            for(var i = 0; i < config.domoticz.groups[index].name.length; i++){
                    updateDomoticz(i, index, setting);
            }
        }


        function updateDomoticz(i, index, setting){

            $http.put('http://' + config.domoticz.ip + '/json.htm?type=command&param=switchlight&idx=' + config.domoticz.groups[index].id + "&switchcmd=" + SaidParameter['on'] + "&level=0&passcode=")
            .success(function (data, status, headers) {
                console.log(data);
            })
            //http://192.168.1.7:8080/json.htm?type=command&param=switchlight&idx=61&switchcmd=On&level=0&passcode=
        }

        return service;
    }

    angular.module('SmartMirror')
        .factory('LightService', LightService);

}());

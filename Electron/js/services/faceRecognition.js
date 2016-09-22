(function() {
    'use strict';

    function FaceRecognitionService($http) {
        var service = {};
        service.faceId = null;
        service.isInitialized = false;

        service.init = function() {
            console.log('!!! init');
            if(service.isInitialized) {
                return new Promise(function(success, failure){
                    success();
                })
            }

            var video = document.getElementById('video');
            if(navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
                navigator.mediaDevices.getUserMedia({ video: true }).then(function(stream) {
                    video.src = window.URL.createObjectURL(stream);
                    video.play();
                });
            }

            service.isInitialized = true;

            return new Promise(function(success, failure){
                setTimeout(success, 2000);
            })
        };

        service.takeSnapshot = function() {
            this.init();
            return this.init().then(takeSnapshot).then(recognizeFace);
        };

        service.addPerson = function(name, callback) {
            return this.init().then(takeSnapshot).then(function(snapshot) { return createPerson(snapshot) });
        };

        function takeSnapshot(){
            console.log('!!! captureSnapshot');
            var canvas = document.getElementById('canvas');
            var context = canvas.getContext('2d');
            var video = document.getElementById('video');
            context.drawImage(video, 0, 0, 640, 480);
            return new Promise(function (success, failure) {
                canvas.toBlob(success, 'image/jpeg', 100);
            });
        }

        function recognizeFace(snapshot) {
            return $http({
                url: 'https://api.projectoxford.ai/face/v1.0/detect?returnFaceId=true',
                method: 'post',
                data: snapshot,
                processData: false,
                headers: {
                    "Content-Type": "application/octet-stream",
                    "Ocp-Apim-Subscription-Key": config.faceRecognition.key
                }
            }).then(function mySucces(response) {
                console.log(response);
                if(response.data.length > 0)
                    return identifyPersons(response.data[0].faceId);
                else
                    return "Show your face";
            }, function myError(response) {
                console.log(response);
            });
        };

        function identifyPersons(faceId) {
            return $http({
                url: 'https://api.projectoxford.ai/face/v1.0/findsimilars',
                method: 'post',
                data: { 
                    faceListId: config.faceRecognition.faceListId,
                    faceId: faceId
                },
                headers: {
                    "Content-Type": "application/json",
                    "Ocp-Apim-Subscription-Key": config.faceRecognition.key
                }
            }).then(function mySucces(response) {
                console.log(response);
                if(response.data.length > 0)
                    return getPerson(response.data[0].persistedFaceId);
                else
                    return "I don't know you";
            }, function myError(response) {
                console.log(response);
            });
        };

        function getPerson(faceId) {
            return $http({
                url: 'https://api.projectoxford.ai/face/v1.0/facelists/' + config.faceRecognition.faceListId,
                headers: {
                    "Ocp-Apim-Subscription-Key": config.faceRecognition.key
                }
            }).then(function mySucces(response) {
                console.log(response);
                for(var i in response.data.persistedFaces) {
                    if(response.data.persistedFaces[i].persistedFaceId === faceId) {
                        return response.data.persistedFaces[i].userData;
                    }
                }
                return 'Ups';
            }, function myError(response) {
                console.log(response);
            });
        };

        function createPerson(snapshot, name) {
            $http({
                url: 'https://api.projectoxford.ai/face/v1.0/persongroups/'+config.faceRecognition.personGroupId+'/persons',
                method: 'post',
                data: {name: name, userData: 'test'},
                headers: {
                    "Content-Type": "application/json",
                    "Ocp-Apim-Subscription-Key": config.faceRecognition.key
                }
            }).then(function mySucces(response) {
                addPersonFace(snapshot, response.data.personId)
            }, function myError(response) {
                console.log(response);
            });   
        }

        function addPersonFace(snapshot, personId) {
            $http({
                url: 'https://api.projectoxford.ai/face/v1.0/persongroups/{personGroupId}/persons/'+personId+'/persistedFaces',
                method: 'post',
                data: snapshot,
                headers: {
                    "Content-Type": "application/octet-stream",
                    "Ocp-Apim-Subscription-Key": config.faceRecognition.key
                }
            }).then(function mySucces(response) {
                console.log(response);
            }, function myError(response) {
                console.log(response);
            });
        };

        return service;
    }

    angular.module('SmartMirror').factory('FaceRecognitionService', FaceRecognitionService);
}(window.annyang));
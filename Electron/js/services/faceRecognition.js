(function() {
    'use strict';

    function FaceRecognitionService($http) {
        var service = {};
        service.faceId = null;
        service.isInitialized = false;

        service.takeSnapshot = function(callback) {
            this.init(function() { captureSnapshot(callback); });
        };

        service.init = function(callback) {
            if(service.isInitialized) {
                callback();
                return;
            }

            var video = document.getElementById('video');
            if(navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
                navigator.mediaDevices.getUserMedia({ video: true }).then(function(stream) {
                    video.src = window.URL.createObjectURL(stream);
                    video.play();
                });
            }

            service.isInitialized = true;
            setTimeout(callback, 2000);
        };

        function captureSnapshot(callback){
            var canvas = document.getElementById('canvas');
            var context = canvas.getContext('2d');
            var video = document.getElementById('video');
            context.drawImage(video, 0, 0, 640, 480);
            canvas.toBlob(function(blobData) {handleSnapshot(blobData, callback);}, 'image/jpeg', 100);
        }

        function handleSnapshot(blobData, callback) {
            $http({
                url: 'https://api.projectoxford.ai/face/v1.0/detect?returnFaceId=true',
                method: 'post',
                data: blobData,
                processData: false,
                headers: {
                    "Content-Type": "application/octet-stream",
                    "Ocp-Apim-Subscription-Key": config.faceRecognition.key
                }
            }).then(function mySucces(response) {
                console.log(response);
                if(response.data.length > 0)
                    identifyPersons(response.data[0].faceId, callback);
                else
                    callback("Show your face")
            }, function myError(response) {
                console.log(response);
            });
        };

        function identifyPersons(faceId, callback) {
            $http({
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
                    getPerson(response.data[0].persistedFaceId, callback);
                else
                    callback("I don't know you")
            }, function myError(response) {
                console.log(response);
            });
        };

        function getPerson(faceId, callback) {
            $http({
                url: 'https://api.projectoxford.ai/face/v1.0/facelists/' + config.faceRecognition.faceListId,
                headers: {
                    "Ocp-Apim-Subscription-Key": config.faceRecognition.key
                }
            }).then(function mySucces(response) {
                console.log(response);
                for(var i in response.data.persistedFaces) {
                    if(response.data.persistedFaces[i].persistedFaceId === faceId) {
                        callback(response.data.persistedFaces[i].userData)
                        break;
                    }
                }
            }, function myError(response) {
                console.log(response);
            });
        };

        return service;
    }

    angular.module('SmartMirror').factory('FaceRecognitionService', FaceRecognitionService);
}(window.annyang));
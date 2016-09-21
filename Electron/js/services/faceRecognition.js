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
                    "Ocp-Apim-Subscription-Key": "5c122f3ebf724f20b145852c695c0ea1"
                }
            }).then(function mySucces(response) {
                callback(response.data[0].faceId)
                service.faceId = response.data[0].faceId;
                console.log(JSON.stringify(response));
            }, function myError(response) {
            });
        };

        return service;
    }

    angular.module('SmartMirror').factory('FaceRecognitionService', FaceRecognitionService);
}(window.annyang));
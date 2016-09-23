(function() {
    'use strict';

    function FaceRecognitionService($http, $translate) {
        var service = {};
        service.faceId = null;
        service.isInitialized = false;

        service.init = function() {
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
            return this.init().then(takeSnapshot).then(detectFace).then(findSimilarFace);
        };

        service.addPerson = function(name) {
            return this.init().then(takeSnapshot).then(function(snapshot) { return addFaceToFaceList(snapshot, name); });
        };

        function takeSnapshot(){
            var canvas = document.getElementById('canvas');
            var context = canvas.getContext('2d');
            var video = document.getElementById('video');
            context.drawImage(video, 0, 0, 640, 480);
            return new Promise(function (success, failure) {
                canvas.toBlob(success, 'image/jpeg', 100);
            });
        }

        function detectFace(snapshot) {
            return $http({
                url: 'https://api.projectoxford.ai/face/v1.0/detect',
                method: 'post',
                data: snapshot,
                params: {
                    returnFaceId: true
                },
                headers: {
                    "Content-Type": "application/octet-stream",
                    "Ocp-Apim-Subscription-Key": config.faceRecognition.key
                }
            }).then(function mySucces(response) {
                console.log(response);
                if(response.data.length == 0)
                    return null;
                return response.data[0].faceId;
            }, function myError(response) {
                console.log(response);
            });
        };

        function findSimilarFace(faceId) {
            if(faceId === null)
                return $translate.instant('faceRecognition.showyourface');

            return $http({
                url: 'https://api.projectoxford.ai/face/v1.0/findsimilars',
                method: 'post',
                data: { 
                    faceListId: config.faceRecognition.faceListId,
                    faceId: faceId,
                    maxNumOfCandidatesReturned: 1
                },
                headers: {
                    "Content-Type": "application/json",
                    "Ocp-Apim-Subscription-Key": config.faceRecognition.key
                }
            }).then(function mySucces(response) {
                console.log(response);
                if(response.data.length == 0)
                    return null;
                return findFaceInFaceList(response.data[0].persistedFaceId);
            }, function myError(response) {
                console.log(response);
            });
        };

        function findFaceInFaceList(faceId) {
            if(faceId == null)
                return $translate.instant('faceRecognition.idontknowyou');

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
                return null;
            }, function myError(response) {
                console.log(response);
            });
        };

        function addFaceToFaceList(snapshot, name) {
            return $http({
                url: 'https://api.projectoxford.ai/face/v1.0/facelists/'+config.faceRecognition.personGroupId+'/persistedFaces',
                method: 'post',
                data: snapshot,
                params: {
                    userData: name
                },
                headers: {
                    "Content-Type": "application/octet-stream",
                    "Ocp-Apim-Subscription-Key": config.faceRecognition.key
                }
            }).then(function mySucces(response) {
                console.log(response);
                return response.data.persistedFaceId;
            }, function myError(response) {
                console.log(response);
            });
        };

        /*function createPerson(snapshot, name) {
            return $http({
                url: 'https://api.projectoxford.ai/face/v1.0/persongroups/'+config.faceRecognition.personGroupId+'/persons',
                method: 'post',
                data: {
                    name: name
                },
                headers: {
                    "Content-Type": "application/json",
                    "Ocp-Apim-Subscription-Key": config.faceRecognition.key
                }
            }).then(function mySucces(response) {
                console.log(response);
                return addPersonFace(snapshot, response.data.personId)
            }, function myError(response) {
                console.log(response);
            });   
        }

        function addPersonFace(snapshot, personId) {
            return $http({
                url: 'https://api.projectoxford.ai/face/v1.0/persongroups/'+config.faceRecognition.personGroupId+'/persons/'+personId+'/persistedFaces',
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

        function identifyPerson(faceId) {
            return $http({
                url: 'https://api.projectoxford.ai/face/v1.0/identify',
                method: 'post',
                data: {
                    faceIds: [faceId],
                    personGroupId: config.faceRecognition.personGroupId,
                    maxNumOfCandidatesReturned: 1
                },
                headers: {
                    "Content-Type": "application/json",
                    "Ocp-Apim-Subscription-Key": config.faceRecognition.key
                }
            }).then(function mySucces(response) {
                console.log(response);
                return 'aaa';
            }, function myError(response) {
                console.log(response);
            });
        };*/

        return service;
    }

    angular.module('SmartMirror').factory('FaceRecognitionService', FaceRecognitionService);
}(window.annyang));
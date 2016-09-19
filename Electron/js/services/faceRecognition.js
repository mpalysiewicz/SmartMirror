(function() {
    'use strict';

    function FaceRecognitionService($http, $q) {
        var service = {};

        service.init = function() {

            var MIRROR_STATES = {
                BLANK: 'blank', // Basic state. No face detected in screen. No one logged in.
                FACE_CLOSE: 'face-close', // Detected a face in screen. Not close enough to authenticate. No one logged in.
                LOGGED_IN: 'logged-in', // Successfully authenticated. Face still in screen. User logged in.
                NOT_DETECTED: 'not-detected', // Unable to authenticate. Face still in screen. User not logged in.
                LOGGING_OUT: 'logging-out' // Face no longer in screen. User logged in, but timeout has begun. Will logout after timeout expires.
            };

            window.MIRROR_STATES = MIRROR_STATES;
            init();
        };

        function rotatePage() {
            var curWidth = $(window).width();
            var curHeight = $(window).height();
            var container = document.body;
            container.style.transform = 'rotate(0deg)';
            container.style.width = curHeight + 'px';
            container.style.height = curWidth + 'px';
            container.style.transform = 'rotate(-90deg)';
            container.style.transformOrigin = 'left top';
            container.style.position = 'relative';
            container.style.top = curHeight + 'px';
        }

        function handleStateChange(state) {
            //Making sure the UI starts clean.
            $('.auth-state').attr('aria-hidden', 'true');
            switch (state) {
                case MIRROR_STATES.FACE_CLOSE:
                    $('#face-close').attr('aria-hidden', 'false');
                    break;

                case MIRROR_STATES.LOGGED_IN:
                    $('#face-authenticated .greeting-name').html(Authenticate.user.name + '!');
                    $('#face-authenticated').attr('aria-hidden', 'false');
                    $('.auth-content').attr('aria-hidden', 'false');
                    $('#logged-in-name').html(Authenticate.user.name);
                    break;

                case MIRROR_STATES.NOT_DETECTED:
                    $('#non-user-detected').attr('aria-hidden', 'false');
                    break;

                case MIRROR_STATES.LOGGING_OUT:
                    $('#logging-out').attr('aria-hidden', 'false');
                    break;

                default:
                    $('.auth-content').attr('aria-hidden', 'true');
            }
        }

        function init() {
            document.addEventListener('mirrorstatechange', function(e) {
                handleStateChange(e.detail);
            });

            document.dispatchEvent(new CustomEvent('mirrorstatechange', {
                'detail': MIRROR_STATES.BLANK
            }));

 
            Authenticate.init();
        }
    }
    angular.module('SmartMirror')
        .factory('FaceRecognitionService', FaceRecognitionService);
}(window.annyang));
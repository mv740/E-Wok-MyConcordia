'use strict';

angular
    .module('myApp')
    .controller('ImageModalCtrl', ['$rootScope', function($rootScope) {

        var imageModal = this;

        $rootScope.$on('imageModal.enlargeImage', function (event, image) {
            imageModal.image = image;
        });

    }]);
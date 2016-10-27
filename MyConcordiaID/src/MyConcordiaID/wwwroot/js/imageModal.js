/**
 * Created by franc on 10/14/2016.
 */

'use strict';

angular.module('myApp.imageModal', [])

    .controller('ImageModalCtrl', ['$rootScope', '$scope', function($rootScope, $scope) {

        var imageModal = $scope;

        $rootScope.$on('enlargeImage', function (event, image) {
            imageModal.image = image;
        });

    }]);
'use strict';

angular.module('myApp.searchModal', ['ngRoute', 'angularCSS'])


.controller('SearchModalCtrl', ['$rootScope', '$scope', '$http', 'myConfig', function($rootScope, $scope, $http, myConfig) {

    var searchModal = $scope;

    $rootScope.$on('searchModal.updateSearchModal', function (event, student) {

        searchModal.student = student;

        $http.get(myConfig.baseUrl + myConfig.pendingPicture + student.id).then(function(value) {
            searchModal.pendingPicture = value.data.pendingpicture;
            searchModal.student.gallery = [value.data.previouspicturE1, value.data.previouspicturE2];

            $rootScope.$broadcast('gallery.updateGallery', searchModal.student.gallery);
        });

    });


    searchModal.sendValidation = function(valid) {

        var data = {
            id: searchModal.student.id,
            valid : valid
        }

        $http({
            method: 'POST',
            url: myConfig.baseUrl + myConfig.validatePhoto,
            data: data
        });

    }

}]);


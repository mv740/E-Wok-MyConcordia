'use strict';

angular.module('myApp.searchModal', ['ngRoute', 'angularCSS'])


.controller('SearchModalCtrl', ['$rootScope', '$scope', '$http', 'myConfig', function ($rootScope, $scope, $http, myConfig) {


    var searchModal = $scope;
    searchModal.loading = true;

    $rootScope.$on('searchModal.doneLoading', function (event) {
        searchModal.loading = false;
    });

    $rootScope.$on('searchModal.updateSearchModal', function (event, student) {

        searchModal.student = student;

        $http.get(myConfig.baseUrl + myConfig.pendingPicture + student.id).then(function (value) {
            searchModal.pendingPicture = value.data.pendingpicture;
            searchModal.student.gallery = [value.data.previouspicturE1, value.data.previouspicturE2];

            $rootScope.$broadcast('gallery.updateGallery', searchModal.student.gallery);
        });

    });


    searchModal.sendValidation = function (valid) {

        var json = {
            id: parseInt(searchModal.student.id),
            valid: valid
        }

        $http({
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: 'POST',
            url: myConfig.baseUrl + myConfig.validatePhoto,
            data: json
        }).then(
        function (success) {
            console.log('validate success');
        },
        function (failure) {
            console.log('validate failure');
        });

    }

}]);


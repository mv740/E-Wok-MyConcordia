'use strict';

angular
    .module('myApp')
    .controller('modalsCtrl', ['$rootScope', '$http', 'myConfig', function ($rootScope, $http, myConfig) {

    var modals = this;
    modals.loading = true;

    $rootScope.$on('modals.update', function (event, student) {

        modals.student = student;

        $http.get(myConfig.baseUrl + myConfig.pendingPicture + student.id).then(function (value) {

            modals.student.pendingPicture = value.data.pendingpicture;
            //modals.student.previousPictures = value.data.previousPictures;
            modals.student.previousPictures = [value.data.previouspicturE1, value.data.previouspicturE2, value.data.previouspicturE1, value.data.previouspicturE2, value.data.previouspicturE1, value.data.previouspicturE2, value.data.previouspicturE1, value.data.previouspicturE2];
            modals.loading = false;
        });

    });


    modals.sendValidation = function (valid) {

        var json = {
            id: parseInt(modals.student.id),
            valid: valid
        };

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

    };

    modals.enlargeImage = function (image) {
        modals.enlargedImage = image;
    };

}]);


'use strict';

angular.module('myApp.searchModal', ['ngRoute', 'ngTouch', 'ngSwippy', 'angularCSS'])


.controller('SearchModalCtrl', ['$rootScope', '$scope', '$http', 'myConfig', function($rootScope, $scope, $http, myConfig) {

    var searchModal = $scope;
    
    searchModal.student = {};

    searchModal.ngSwippy = {
        showInfo : false,
        clickedTimes: 0,
        actions: [],
        size : {
            width: 250,
            height: 350
        }
    };

    $rootScope.$on('updateSearchModal', function (event, student) {
        searchModal.student = student;
        searchModal.ngSwippy.collection = [{
            thumbnail: student.gallery.toValidate
        }];

        $rootScope.$broadcast('updateGallery', student.gallery.validated);
    });


    searchModal.ngSwippy.onClick = function () {
        searchModal.ngSwippy.actions.unshift({ name: 'Click on item' });
  };


    searchModal.ngSwippy.swipend = function () {
        searchModal.ngSwippy.actions.unshift({ name: 'Collection Empty' });
  };


    searchModal.ngSwippy.swipeLeft = function (person) {
        searchModal.ngSwippy.actions.unshift({ name: 'Left swipe' });
        sendValidation(false);
  };

    searchModal.ngSwippy.swipeRight = function (person) {
        searchModal.ngSwippy.actions.unshift({ name: 'Right swipe' });
        sendValidation(true);
    };

    function sendValidation(valid) {

        var data = myConfig.validatePhoto.dataTemplate;
        data.id = searchModal.student.id;
        data.valid = valid

        $http({
            method: 'POST',
            url: myConfig.baseUrl + myConfig.validatePhoto,
            data: data
        });

    }

}]);


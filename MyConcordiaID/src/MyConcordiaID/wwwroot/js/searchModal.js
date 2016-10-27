'use strict';

angular.module('myApp.searchModal', ['ngRoute', 'ngTouch', 'ngSwippy', 'angularCSS'])


.controller('SearchModalCtrl', ['$rootScope', '$scope', '$timeout', function($rootScope, $scope, $timeout) {

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
    

    searchModal.ngSwippy.collection = [{
        thumbnail: 'images/1.jpg'
    }];

    searchModal.ngSwippy.onClick = function () {
    $timeout(function(){
        searchModal.ngSwippy.clickedTimes = searchModal.ngSwippy.clickedTimes + 1;
        searchModal.ngSwippy.actions.unshift({ name: 'Click on item' });
    });

  };

    searchModal.ngSwippy.swipend = function () {
        searchModal.ngSwippy.actions.unshift({ name: 'Collection Empty' });
  };



    searchModal.ngSwippy.swipeLeft = function (person) {
        searchModal.ngSwippy.actions.unshift({ name: 'Left swipe' });
  };

    searchModal.ngSwippy.swipeRight = function (person) {
        searchModal.ngSwippy.actions.unshift({ name: 'Right swipe' });
  };

}]);
'use strict';

angular.module('myApp.view2', ['ngRoute', 'ngTouch', 'ngSwippy', 'angularCSS'])

.config(['$routeProvider', function($routeProvider) {
  $routeProvider.when('/view2', {
    templateUrl: 'view2/view2.html',
    controller: 'View2Ctrl',
    css: 'view2/view2.css'
  });
}])

.controller('View2Ctrl', ['$scope', '$timeout', function($scope, $timeout) {

  $scope.itemsCollection = [{
    thumbnail: 'view2/images/1.jpg',
    title: 'Clara Oswin Oswald',
    subtitle: 'clara@gmail.com'
  }, {
    thumbnail: 'view2/images/2.jpg',
    title: 'Emy Pond',
    subtitle: 'emy@gmail.com'
  }]

  $scope.myCustomFunction = function(){
    $timeout(function(){
      $scope.clickedTimes = $scope.clickedTimes + 1;
      $scope.actions.unshift({name: 'Click on item'});
    });

  };

  $scope.size = {
    width: 300,
    height: 400
  };

  $scope.showinfo = true;

  $scope.swipend = function(){
    $scope.actions.unshift({name: 'Collection Empty'});
  };

  $scope.clickedTimes = 0;

  $scope.actions = [];

  $scope.swipeLeft = function(person){
    $scope.actions.unshift({name: 'Left swipe'});
  };

  $scope.swipeRight = function(person){
    $scope.actions.unshift({name: 'Right swipe'});
  };

}]);
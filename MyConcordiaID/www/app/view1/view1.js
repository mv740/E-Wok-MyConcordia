'use strict';

angular.module('myApp.view1', ['ngRoute', 'angularCSS'])

.config(['$routeProvider', function($routeProvider) {
  $routeProvider.when('/view1', {
    templateUrl: 'view1/view1.html',
    controller: 'View1Ctrl',
    css: 'view1/view1.css'
  });
}])

.controller('View1Ctrl', [function() {

}]);
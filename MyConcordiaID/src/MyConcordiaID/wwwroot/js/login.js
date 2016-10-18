/**
 * Created by franc on 10/15/2016.
 */

angular.module('myApp.login', ['ngRoute', 'angularCSS'])

    .config(['$routeProvider', function($routeProvider) {
        $routeProvider.when('/login', {
            templateUrl: 'partials/login/login.html',
            controller: 'LoginCtrl',
            css: 'partials/login/login.css'
        });
    }])
    .controller('LoginCtrl', ['$scope', function($scope) {

        //put stuff here

    }]);
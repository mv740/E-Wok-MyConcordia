'use strict';

// Declare app level module which depends on views, and components
angular.module('myApp', [
    'ngRoute',
    'ngAnimate',
    'ngMaterial',
    'myApp.login',
    'myApp.view1',
    'myApp.view1.searchBox',
    'myApp.view1.searchResults',
    'myApp.searchModal',
    'myApp.searchModal.gallery',
    'myApp.imageModal',
    'ngHamburger'
]).config(['$locationProvider', '$routeProvider', function ($locationProvider, $routeProvider) {
    $locationProvider.hashPrefix('!');

    $routeProvider.otherwise({redirectTo: '/view1'});
}]).controller('MainCtrl', function ($scope, $mdSidenav) {
    
    var app = $scope;

    app.toggleNav = function () {
        if (!$scope.hamState) {
            closeNav();
        }
        else openNav();
    };

    /* Set the width of the side navigation to 0 */
    var closeNav = function () {
        document.getElementById("mySidenav").style.width = "0";
        app.hamState = false;
    };
    app.closeNav = closeNav;

    function openNav() {
        document.getElementById("mySidenav").style.width = "250px";
    }

});

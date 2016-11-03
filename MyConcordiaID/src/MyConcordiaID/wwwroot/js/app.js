'use strict';

// Declare app level module which depends on views, and components
angular.module('myApp', [
    'ngRoute',
    'ngAnimate',
    'ngMaterial',
    'myApp.login',
    'myApp.sideNav',
    'ngHamburger',
    'myApp.view1',
    'myApp.view1.searchBox',
    'myApp.view1.searchResults',
    'myApp.searchModal',
    'myApp.searchModal.gallery',
    'myApp.imageModal'

]).constant("myConfig", {
    "baseUrl": "https://myconcordiaid.azurewebsites.net/api/",
    "validatePhoto": "student/validatePhoto/",
    "searchStudent": "student/",
    "pendingPicture": "student/PendingPicture/"

}).config(['$locationProvider', '$routeProvider', function ($locationProvider, $routeProvider) {
    $locationProvider.hashPrefix('!');

    $routeProvider
        .otherwise({ redirectTo: '/login' });
}]);
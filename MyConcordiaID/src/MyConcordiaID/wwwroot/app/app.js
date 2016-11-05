'use strict';

// Declare app level module which depends on views, and components
angular.module('myApp', [
    'ngRoute',
    'ngAnimate',
    'ngMaterial',
    'myApp.sideNav',
    'ngHamburger',
    'myApp.review',
    'myApp.review.searchBox',
    'myApp.review.searchResults',
    'myApp.searchModal',
    'myApp.searchModal.gallery',
    'myApp.imageModal'

]).constant("myConfig", {
    "baseUrl": "https://myconcordiaid.azurewebsites.net/api/",
    "validatePhoto": "student/ValidatePicture",
    "searchStudent": "student/",
    "pendingPicture": "student/PendingPicture/"

}).config(['$locationProvider', '$routeProvider', function ($locationProvider, $routeProvider) {
    $locationProvider.hashPrefix('!');

    $routeProvider
        .otherwise({ redirectTo: '/review' });
}]);
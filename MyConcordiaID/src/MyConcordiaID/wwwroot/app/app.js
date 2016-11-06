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
    'myApp.imageModal',
    'ui.bootstrap'

]).constant("myConfig", {
    "baseUrl": "https://myconcordiaid.azurewebsites.net/api/",
    "validatePhoto": "student/ValidatePicture",
    "searchStudent": "student/",
    "pendingPicture": "student/PendingPicture/",
    "picturePeriod": "admin/picturePeriod"

}).config(['$locationProvider', '$routeProvider', function ($locationProvider, $routeProvider) {
    $locationProvider.hashPrefix('!');

    $routeProvider
        .when('/admin', {
            templateUrl : 'partials/admin/admin.html'
        })
        .otherwise({ redirectTo: '/review' });
}]);
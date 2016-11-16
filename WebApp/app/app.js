'use strict';

// Declare app level module which depends on views, and components
angular.module('myApp', [
    'ngRoute',
    'ngAnimate',
    'ngMaterial',
    'ngHamburger',
    'ui.bootstrap',
    'angularCSS'

]).constant("myConfig", {
    "baseUrl": "https://myconcordiaid.azurewebsites.net/api/",
    "validatePhoto": "student/ValidatePicture",
    "searchStudent": "student/",
    "pendingPicture": "student/PendingPicture/",
    "picturePeriod": "admin/picturePeriod",
    "getStudents": "student",
    "search": "student/search"

}).config(['$locationProvider', '$routeProvider', function ($locationProvider, $routeProvider) {
    $locationProvider.hashPrefix('!');

    $routeProvider
        .when('/admin', {
            templateUrl : 'partials/admin/admin.html'
        }).when('/review', {
            templateUrl: 'partials/review/review.html',
            css: 'sass/views/review.css'
        })
        .otherwise({ redirectTo: '/review' });
}]);
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
    "picturePeriod": "admin/picturePeriod"

}).config(['$locationProvider', '$routeProvider', function ($locationProvider, $routeProvider) {
    $locationProvider.hashPrefix('!');

    $routeProvider
        .when('/admin', {
            templateUrl : 'partials/admin/admin.html'
        }).when('/review', {
            templateUrl: 'partials/review/review.html',
            css: [
                'partials/review/review.css',
                'partials/review/searchBox/searchBox.css',
                'partials/review/searchResults/searchResults.css',
                'partials/review/modals/studentModal/studentModal.css',
                'partials/review/modals/studentModal/gallery/gallery.css',
                'partials/review/modals/imageModal/imageModal.css',
                'partials/review/sideNav/sideNav.css'
            ]
        })
        .otherwise({ redirectTo: '/review' });
}]);
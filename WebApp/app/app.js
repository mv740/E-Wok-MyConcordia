'use strict';

// Declare app level module which depends on views, and components
angular.module('myApp', [
    'ngRoute',
    'ngAnimate',
    'ngMaterial',
    'ngHamburger',
    'ui.bootstrap',
    'ui.bootstrap.datetimepicker',
    'angularCSS',
    'ng.oidcclient',
    '720kb.tooltips',
    'fullPage.js',
    'ng-fusioncharts'


]).constant("myConfig", {
    "baseUrl": "https://api.myconcordiaid.me/api/",
    "validatePhoto": "student/ValidatePicture",
    "searchStudent": "student/",
    "getStudentPictures": "student/picture/",
    "picturePeriod": "admin/picturePeriod",
    "getStudents": "student",
    "getLogs": "log/",
    "search": "student/search",
    "getUpdatePeriod": "admin/UpdatePeriod",
    "validateArchived": "student/RevalidatePicture",
    "submitComment": "student/comment",
    "event": "Event",
    "getEvent": "Event/",
    "eventUser": "Event/user",
    "eventsCreated": "Event/admin/",
    "eventAttendees": "Event/IDTOKEN/users",
    "getEvents": "Event/admin"
    "getEventStats": "Event/IDTOKEN/stats"
})
    .config(['ngOidcClientProvider', function (ngOidcClientProvider) {


        var link = "https://api.myconcordiaid.me/";

        ngOidcClientProvider.setSettings({
            authority: link,
            client_id: "oidcWebClient",
            redirect_uri: "https://www.myconcordiaid.me/callback.html",
            post_logout_redirect_uri: "https://www.myconcordiaid.me/oidc",
            silent_redirect_uri: "https://www.myconcordiaid.me/oidc",
            //redirect_uri: "https://localhost/oidc",
            //post_logout_redirect_uri: "https://localhost/oidc",
            //silent_redirect_uri: "https://localhost/oidc",

            response_type: "token",
            scope: "profile",

            automaticSilentRenew: true,

            filterProtocolClaims: false,
            loadUserInfo: true
        });
    }])


    .config(['$locationProvider', '$routeProvider', '$httpProvider', function ($locationProvider, $routeProvider, $httpProvider) {
        //$locationProvider.hashPrefix('!');

        $routeProvider
            .when('/login', {
                templateUrl: 'partials/login/login.html',
                css: 'sass/views/login.css',
                controller: 'LoginController',
                controllerAs: 'vm',
                authenticate: false
            }).when('/admin', {
            templateUrl: 'partials/admin/admin.html',
            css: 'sass/views/admin.css',
            authenticate: true
        }).when('/review', {
            templateUrl: 'partials/review/review.html',
            css: 'sass/views/review.css',
            authenticate: true
        }).when('/event', {
            templateUrl: 'partials/event/event.html',
            css: 'sass/views/event.css',
            authenticate: true
        })
            .otherwise({redirectTo: '/review'});


        $locationProvider.html5Mode({
            enabled: true,
            requireBase: true
        });


        $httpProvider.interceptors.push('AuthInterceptorService');
    }])
    .run(['$rootScope', 'SessionService', '$location', function ($rootScope, SessionService, $location) {

        $rootScope.$on("$routeChangeStart", function (event, curr, prev) {

            if (!SessionService.isAuthenticated()) {

                // redirect to the login route
                console.log('Unauthorized access');
                $location.path('/login');
            }


        });
    }]);

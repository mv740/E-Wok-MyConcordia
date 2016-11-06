// Ionic Starter App

// angular.module is a global place for creating, registering and retrieving Angular modules
// 'starter' is the name of this angular module example (also set in a <body> attribute in index.html)
// the 2nd parameter is an array of 'requires'
// 'starter.controllers' is found in controllers.js

// Client side variable used to restrict access to the Update Picture feature.
var canUpdate = true;


angular.module('starter', ['ionic', 'starter.controllers', 'ngCordova'])
  .constant('serverName', 'https://myconcordiaid.azurewebsites.net/api/')




.run(function($ionicPlatform, $rootScope) {
  $ionicPlatform.ready(function() {
    // Hide the accessory bar by default (remove this to show the accessory bar above the keyboard
    // for form inputs)
    if (window.cordova && window.cordova.plugins.Keyboard) {
      cordova.plugins.Keyboard.hideKeyboardAccessoryBar(true);
      cordova.plugins.Keyboard.disableScroll(true);


    }
    if (window.StatusBar) {
      // org.apache.cordova.statusbar required
      StatusBar.styleDefault();
    }

    $rootScope.canUpdate = true;
    $rootScope.validPeriod = true;
  });
})

.config(function($stateProvider, $urlRouterProvider) {
  $stateProvider

    .state('app', {
    url: '/app',
    abstract: true,
    templateUrl: 'templates/menu.html',
    controller: 'AppCtrl'
  })

  .state('app.id', {
    url: '/id',
    views: {
      'menuContent': {
        templateUrl: 'templates/id.html',
        controller: 'IdCtrl'
      }
    }
  })

  .state('app.camera', {
      url: '/camera',
      views: {
        'menuContent': {
          templateUrl: 'templates/camera.html',
          controller: 'CameraCtrl',


          // If the student cannot update picture, restrict access to the camera.
          resolve: {
            security: ['$q','$rootScope', function ($q, $rootScope){
              if(!$rootScope.canUpdate || !$rootScope.validPeriod){
                alert("Cannot update picture at the moment. Please contact Birks for more details.");
                return $q.reject("Not Authorized");
              }
            }]
          }

        }
      }
    })
    .state('app.login', {
      url: '/login',
      views: {
        'menuContent': {
          templateUrl: 'templates/login.html',
          controller: 'LoginCtrl'
        }
      }
    });
  // if none of the above states are matched, use this as the fallback
  $urlRouterProvider.otherwise('/app/login');
});

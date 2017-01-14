// Ionic Starter App

// angular.module is a global place for creating, registering and retrieving Angular modules
// 'starter' is the name of this angular module example (also set in a <body> attribute in index.html)
// the 2nd parameter is an array of 'requires'
// 'starter.controllers' is found in controllers.js

angular.module('starter', ['ionic', 'starter.controllers', 'ngCordova', 'ng.oidcclient'])
  .constant('serverName', 'https://myconcordiaid.azurewebsites.net/api/')


  .run(function ($ionicPlatform, $rootScope) {
    $ionicPlatform.ready(function () {
      // LOCK ORIENTATION TO PORTRAIT.
      screen.lockOrientation('portrait');

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

  .config(['ngOidcClientProvider', function (ngOidcClientProvider) {


    var link = "https://myconcordiaid.azurewebsites.net/";

    ngOidcClientProvider.setSettings({
      authority: link,
      client_id: "oidcdemomobile",
      redirect_uri: "https://localhost/oidc",
      post_logout_redirect_uri: "https://localhost/oidc",
      silent_redirect_uri: "https://localhost/oidc",

      response_type: "token",
      scope: "profile",

      automaticSilentRenew: true,

      filterProtocolClaims: true,
      loadUserInfo: true,

      popupNavigator: new Oidc.CordovaPopupNavigator(),
      iframeNavigator: new Oidc.CordovaIFrameNavigator()
    });
  }])
  .config(['$stateProvider', '$urlRouterProvider', '$httpProvider', function ($stateProvider, $urlRouterProvider, $httpProvider) {

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
            controller: 'IdController as vm',
            css: 'css/id.css'
          }
        }
      })
      .state('app.marshalling', {
        url: '/marshalling',
        views: {
          'menuContent': {
            templateUrl: 'templates/marshalling.html',
            controller: 'IdController as vm',
            css: 'css/marshalling.css'
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
              security: ['$q', '$rootScope', function ($q, $rootScope) {
                if (!$rootScope.canUpdate || !$rootScope.validPeriod) {
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
            controller: 'LoginController as vm',
            css: 'css/login.css'
          }
        }
      });
    // if none of the above states are matched, use this as the fallback
    $urlRouterProvider.otherwise('/app/login');

    $httpProvider.interceptors.push('AuthInterceptorService');
  }]);

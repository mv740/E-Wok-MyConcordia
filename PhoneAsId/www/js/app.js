// Ionic Starter App

// angular.module is a global place for creating, registering and retrieving Angular modules
// 'starter' is the name of this angular module example (also set in a <body> attribute in index.html)
// the 2nd parameter is an array of 'requires'
// 'starter.controllers' is found in controllers.js

angular.module('starter', ['ionic', 'ionic.contrib.drawer', 'starter.controllers', 'ngCordova', 'ng.oidcclient', 'angularCSS', 'templates', 'pascalprecht.translate'])
  .constant('Settings', {
    'api' : 'https://api.myconcordiaid.me/api/',
    'baseUrl' : 'https://api.myconcordiaid.me/'
  })


  .run(function ($ionicPlatform, $rootScope, $translate) {
    $ionicPlatform.ready(function () {

      //gets preferred language and sets it for translations
      if(typeof navigator.globalization !== "undefined") {
        navigator.globalization.getPreferredLanguage(function(language) {
          $translate.use((language.value).split("-")[0]).then(function(data) {
            console.log("LANGUAGE SUCCESS -> " + data);
          }, function(error) {
            console.log("LANGUAGE ERROR -> " + error);
          });
        }, null);
      }

      //workaround for white screen showing after splash screen
      setTimeout(function () {
        navigator.splashscreen.hide();
      }, 100);

      hockeyapp.start(success, fail, "0723638003684a1b8b3a6476e3816afd");


      function success() {
        console.log("success");

        hockeyapp.checkForUpdate();
      }


      function fail() {
        console.log("fail");
      }

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

  .config(['ngOidcClientProvider', 'Settings', function (ngOidcClientProvider, Settings) {

    ngOidcClientProvider.setSettings({
      authority: Settings.baseUrl,
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
            css: 'sass/views/id.css'
          }
        }
      })
      .state('app.marshalling', {
        url: '/marshalling',
        views: {
          'menuContent': {
            templateUrl: 'templates/marshalling.html',
            controller: 'IdController as vm',
            css: 'sass/views/marshalling.css'
          }
        }
      })
      .state('app.events', {
        url: '/events',
        views: {
          'menuContent': {
            templateUrl: 'templates/events.html',
            controller: 'EventsController as ev',
            css: 'sass/views/event.css'
          }
        }
      })
      .state('app.camera', {
        url: '/camera',
        views: {
          'menuContent': {
            templateUrl: 'templates/camera.html',
            controller: 'CameraCtrl as camCtrl',
            css: 'sass/views/camera.css',


            // If the student cannot update picture, restrict access to the camera.
            resolve: {
              security: ['$q', '$rootScope', function ($q, $rootScope) {
                // If !valid & !pending, picture should be updated regardless of canUpdate or validPeriod. Otherwise...
                // If your picture is pending, then you can't update picture.
                // If !pending and are valid, if you can't update or aren't within the valid period, you can't update picture.
                if (($rootScope.pending) || (($rootScope.valid && !$rootScope.pending) && (!$rootScope.canUpdate || !$rootScope.validPeriod))) {
                  if (ionic.Platform.isIOS()) {
                    document.addEventListener("deviceready", onDeviceReady, true);
                    function onDeviceReady() {
                      navigator.notification.alert(
                        'Cannot update picture at the moment. Please contact Birks for more details.',
                        function () {
                        },
                        'Alert',
                        'OK'
                      );
                    }
                  }
                  if (ionic.Platform.isAndroid()) {
                    alert("Cannot update picture at the moment. Please contact Birks for more details.");
                  }
                  return $q.reject("Not Authorized");
                }
              }]
            }

          }
        }
      })

      .state('app.barcode', {
        url: '/barcode',
        views: {
          'menuContent': {
            templateUrl: 'templates/barcode.html',
            controller: 'BarcodeController as bc',
            css: 'sass/views/barcode.css'
          }
        }
      })

      .state('app.login', {
        url: '/login',
        views: {
          'menuContent': {
            templateUrl: 'templates/login.html',
            controller: 'LoginController as vm',
            css: ['css/login.css', 'sass/views/login.css']
          }
        }
      });
    // if none of the above states are matched, use this as the fallback
    $urlRouterProvider.otherwise('/app/login');

    $httpProvider.interceptors.push('AuthInterceptorService');
  }])
  .config(function($stateProvider, $urlRouterProvider, $translateProvider) {
    $translateProvider.translations('en', {
      undergraduate: "ENGLISH - Undergraduate",
      concordia_university: "ENGLISH - Concordia university"
    });
    $translateProvider.translations('fr', {
      undergraduate: "FRENCH - Premier cycle",
      concordia_university: "FRENCH - Universite de Concordia"
    });
    $translateProvider.preferredLanguage("en");
    $translateProvider.fallbackLanguage("en");
  });

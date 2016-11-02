// Ionic Starter App

// angular.module is a global place for creating, registering and retrieving Angular modules
// 'starter' is the name of this angular module example (also set in a <body> attribute in index.html)
// the 2nd parameter is an array of 'requires'
// 'starter.controllers' is found in controllers.js
angular.module('starter', ['ionic', 'starter.controllers', 'ngCordova', 'ng.oidcclient'])
  .constant('serverName', 'https://myconcordiaid.azurewebsites.net/api/')

.run(function($ionicPlatform) {
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
  });
})


.config(['ngOidcClientProvider', function (ngOidcClientProvider) {

    ngOidcClientProvider.setSettings({
      authority: "https://myconcordiaid.azurewebsites.net/",
      client_id: "oidcdemomobile",
      redirect_uri: "https://localhost/oidc",
      post_logout_redirect_uri: "https://localhost/oidc",
      silent_redirect_uri: "https://localhost/oidc",

      response_type: "id_token token",
      scope: "openid profile",

      automaticSilentRenew: true,

      filterProtocolClaims: true,
      loadUserInfo: true,

      popupNavigator: new Oidc.CordovaPopupNavigator(),
      iframeNavigator: new Oidc.CordovaIFrameNavigator()
    });
  }])

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
          controller: 'CameraCtrl'
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
    })

    .state('app.account', {
      url: "/account",
      cache: false,
      views: {
        'menuContent': {
          templateUrl: "templates/account.html",
          controller: 'AccountCtrl'
        }
      }
    });
  // if none of the above states are matched, use this as the fallback
  $urlRouterProvider.otherwise('/app/login');
});

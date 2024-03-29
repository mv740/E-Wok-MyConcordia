/**
 * Created by michal on 11/16/2016.
 *
 */

(function () {
  'use strict';

  angular
    .module('starter')
    .factory('AuthenticationService', AuthenticationService);

  AuthenticationService.$inject = ['SessionService', '$state', 'ngOidcClient', '$cordovaInAppBrowser', '$rootScope', '$ionicSideMenuDelegate', '$ionicNavBarDelegate', '$ionicPopup', '$cordovaNetwork', '$ionicLoading', '$filter'];

  function AuthenticationService(SessionService, $state, ngOidcClient, $cordovaInAppBrowser, $rootScope, $ionicSideMenuDelegate, $ionicNavBarDelegate, $ionicPopup, $cordovaNetwork, $ionicLoading, $filter) {
    var failedLoginAlert = function () {
      var alertPopup = $ionicPopup.alert({
        title: '<b>' + $filter('translate')('popup_login_fail_title') + '</b>',
        template: '{{"popup_login_fail" | translate}}'
      });

      alertPopup.then(function (res) {
        $state.go("app.login");
        console.log('redirected to login page.');
      });
    };

    var authService = {};

    /*
     start the oauth process
     */


    authService.signIn = function ($scope) {
      $scope.show = function() {
        $ionicLoading.show({
          template: '<p>{{"popup_loading" | translate}}</p><ion-spinner></ion-spinner>'
        });
      };
      $scope.hide = function(){
        $ionicLoading.hide();
      };

      if ($cordovaNetwork.isOnline()) {
        $scope.show($ionicLoading);
        ngOidcClient.signinPopup()
          .then(
            function (user) {
              $scope.hide($ionicLoading);
              console.log("user:" + JSON.stringify(user));
              if (user) {
                $ionicSideMenuDelegate.canDragContent(true);
                $ionicNavBarDelegate.showBar(true);
                hockeyapp.trackEvent(null, null, "LOGIN_SUCCESS");
                $state.go('app.id');
              }
            },
            function (rejected) {
              hockeyapp.trackEvent(null, null, "LOGIN_FAILED");
              failedLoginAlert(rejected);
            }
          );

      } else {
        $ionicPopup.alert({
          title: '<b>' + $filter('translate')('popup_login_fail_title') + '</b>',
          template: '{{"popup_login_fail" | translate}}'
        }).then(function (res) {
          console.log('redirected to login page.');
        });
      }
    };

    /*
     clear the session from the inappbrowser which will log you out of third party system (google)
     */
    authService.logOut = function () {
      //taken from
      //https://forum.ionicframework.com/t/facebook-logout-with-firebase-ionic-clear-oauth-session-so-new-user-can-login/54557/5

      var options = {
        location: 'yes',
        clearcache: 'yes',
        clearsessioncache: 'yes',
        toolbar: 'no',
        hidden: 'yes'
      };
      // _blank loads in background
      $rootScope.$on('$cordovaInAppBrowser:loadstop', function (e, event) {
        $cordovaInAppBrowser.close();
      });
      $cordovaInAppBrowser.open('http://www.google.com', '_blank', options);

      //hide nav bar
      $ionicNavBarDelegate.showBar(false);
      $state.go('app.login');
    };

    authService.isAuthenticated = function () {
      return SessionService.isAuthenticated();
    };

    return authService;
  }

})();

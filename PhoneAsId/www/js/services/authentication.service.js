/**
 * Created by michal on 11/16/2016.
 *
 */

(function () {
  'use strict';

  angular
    .module('starter')
    .factory('AuthenticationService', AuthenticationService);

  AuthenticationService.$inject = ['SessionService', '$state','ngOidcClient', '$cordovaInAppBrowser','$rootScope','$ionicSideMenuDelegate','$ionicNavBarDelegate'];

  function AuthenticationService(SessionService, $state, ngOidcClient, $cordovaInAppBrowser, $rootScope,$ionicSideMenuDelegate,$ionicNavBarDelegate) {

    var authService = {};

    /*
      start the oauth process
     */
    authService.signIn = function () {
      ngOidcClient.signinPopup().then(function (user) {
        //console.log("user:" + JSON.stringify(user));
        if (user) {
          $ionicSideMenuDelegate.canDragContent(true);
          $ionicNavBarDelegate.showBar(true);
          $state.go('app.id');
        }
      });
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
        clearsessioncache : 'yes',
        toolbar: 'no',
        hidden: 'yes'
      };
      // _blank loads in background
      $rootScope.$on('$cordovaInAppBrowser:loadstop', function(e, event){
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

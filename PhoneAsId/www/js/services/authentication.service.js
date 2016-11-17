/**
 * Created by michal on 11/16/2016.
 *
 */

(function () {
  'use strict';

  angular
    .module('starter')
    .factory('AuthenticationService', AuthenticationService);

  AuthenticationService.$inject = ['SessionService', '$state','ngOidcClient'];

  function AuthenticationService(SessionService, $state, ngOidcClient) {

    var authService = {};

    authService.signIn = function () {
      ngOidcClient.signinPopup().then(function (user) {
        //console.log("user:" + JSON.stringify(user));
        if (user) {
          $state.go('app.id');
        }
      });
    };

    //todo need to integrated logout from backend
    authService.logOut = function () {
      ngOidcClient.signoutPopup().then(function () {
        $state.go('app.login');
      });
    };

    authService.isAuthenticated = function () {
      if (SessionService.get()) {
        return SessionService.get().isAuthenticated();
      }
      return false;
    };

    return authService;
  }

})();

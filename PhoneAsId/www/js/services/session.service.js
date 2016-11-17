/**
 * Created by michal on 11/16/2016.
 */
(function () {
  'use strict';

  angular
    .module('starter')
    .factory('SessionService', SessionService);

  SessionService.$inject = ['ngOidcClient'];

  function SessionService(ngOidcClient) {

    var session = {};

    session.getAccessToken = function () {
      var userInfo = ngOidcClient.getUserInfo();
      if (userInfo && userInfo.user) {
        return userInfo.user.access_token;
      }
    };

    session.getAccessTokenType = function () {
      var userInfo = ngOidcClient.getUserInfo();
      if (userInfo && userInfo.user) {
        return userInfo.user.token_type;
      }
    };

    session.isAuthenticated = function () {
      var userInfo = ngOidcClient.getUserInfo();

      return (userInfo && userInfo.user)

    };

    return session;
  }
})();

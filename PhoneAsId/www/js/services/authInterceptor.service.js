/**
 * Created by michal on 11/16/2016.
 *
 * Intercept every http request to add the authentication token
 *
 */
(function () {
  'use strict';

  angular
    .module('starter')
    .factory('AuthInterceptorService', AuthInterceptorService);

  AuthInterceptorService.$inject = ['$q', '$injector'];

  function AuthInterceptorService($q,$injector) {

    var authInterceptorServiceFactory = {};

    var _request = function (config) {

      config.headers = config.headers || {};
      var SessionService = $injector.get('SessionService');

      if (SessionService.isAuthenticated()) {
        config.headers.Authorization = 'Bearer ' + SessionService.getAccessToken();
      }

      return config;
    };

    var _responseError = function (rejection) {
      if (rejection.status === 401) {
        var $state = $injector.get('$state');
        $state.go('app.login')
      }
      return $q.reject(rejection);
    };

    authInterceptorServiceFactory.request = _request;
    authInterceptorServiceFactory.responseError = _responseError;

    return authInterceptorServiceFactory;
  }

})();

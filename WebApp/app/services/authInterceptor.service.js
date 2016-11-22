/**
 * Created by Michal Wozniak on 11/18/2016.
 */
(function () {
    'use strict';

    angular
        .module('myApp')
        .factory('AuthInterceptorService', AuthInterceptorService);

    AuthInterceptorService.$inject = ['$q', '$injector','$location'];

    function AuthInterceptorService($q,$injector,$location) {

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
                $location.path('/login')
            }
            return $q.reject(rejection);
        };

        authInterceptorServiceFactory.request = _request;
        authInterceptorServiceFactory.responseError = _responseError;

        return authInterceptorServiceFactory;
    }

})();
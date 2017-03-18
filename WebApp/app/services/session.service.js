/**
 * Created by Michal Wozniak on 11/18/2016.
 */
(function () {
    'use strict';

    angular
        .module('myApp')
        .factory('SessionService', SessionService);

    SessionService.$inject = ['ngOidcClient', '$location','$rootScope'];

    function SessionService(ngOidcClient, $location, $rootScope) {

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
            //var userInfo = ngOidcClient.getUserInfo();
            //return userInfo.isAuthenticated;

            //if user refresh page, will detect if we still have an active session
            function findOidcUser(value) {
                return value.includes('oidc.user')
            }

            var storage =  Object.keys(sessionStorage);
            return storage.find(findOidcUser)

        };

        session.destroy = function () {
            ngOidcClient.removeUser().then(function successfulLogOut() {
                $location.path('/login');
                //trigger digest manually 
                $rootScope.$apply()
            });
        };

        return session;
    }
})();
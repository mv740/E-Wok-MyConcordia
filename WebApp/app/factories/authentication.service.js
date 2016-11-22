/**
 * Created by Michal Wozniak on 11/18/2016.
 */

(function () {
    'use strict';

    angular
        .module('myApp')
        .factory('AuthenticationService', AuthenticationService);

    AuthenticationService.$inject = ['SessionService', '$location','ngOidcClient','$rootScope'];

    function AuthenticationService(SessionService, $location, ngOidcClient, $rootScope) {

        var authService = {};

        authService.signIn = function () {
            ngOidcClient.signinPopup().then(function (user) {
                //console.log("user:" + JSON.stringify(user));
                if (user) {
                    $rootScope.$apply(function() {
                        $location.path('/review');
                    });
                }
            });
        };

        //todo need to integrated logout from backend
        authService.logOut = function () {
                SessionService.destroy();
                $location.path('/login');


            //
            // ngOidcClient.signoutPopup().then(function () {
            //     $location.path('/login');
            // });
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
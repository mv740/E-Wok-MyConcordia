/**
 * Created by Michal Wozniak on 11/18/2016.
 */
(function () {
    'use strict';

    angular
        .module('myApp')
        .controller('LoginController', LoginController);

    LoginController.$inject = ['AuthenticationService'];

    function LoginController(AuthenticationService) {
        var vm = this;


        vm.login = function () {
            AuthenticationService.signIn();
        };

        vm.logout = function () {

        };
    }
})();
/**
 * Created by NSPACE on 12/24/2016.
 */
(function () {
  'use strict';

  angular
    .module('starter')
    .controller('LoginController', LoginController);

  LoginController.$inject = ['AuthenticationService'];

  function LoginController(AuthenticationService) {
    var vm = this;

    vm.loginEnabled = false;

    vm.logIn = function () {
      AuthenticationService.signIn();
    };//

  }
})();

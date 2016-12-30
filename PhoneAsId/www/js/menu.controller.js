/**
 * Created by NSPACE on 11/25/2016.
 */
(function () {
  'use strict';

  angular
    .module('starter')
    .controller('MenuController', MenuController);

  MenuController.$inject = ['AuthenticationService'];

  function MenuController(AuthenticationService) {
    var vm = this;

    vm.logOut = logOut;

    function logOut() {
      AuthenticationService.logOut();
    }

  }
})();

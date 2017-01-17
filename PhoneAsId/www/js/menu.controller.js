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
    vm.overlay = false;

    vm.onSwipeLeft = onSwipeLeft;
    vm.toggleDrawer = toggleDrawer;
    vm.logOut = logOut;

    function logOut() {
      AuthenticationService.logOut();
    }

    function toggleDrawer(){
      if (vm.overlay)
      vm.overlay = false;
      else vm.overlay = true;
    }

    function onSwipeLeft(){
      vm.overlay = false;
    }

  }
})();

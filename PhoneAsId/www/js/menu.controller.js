/**
 * Created by NSPACE on 11/25/2016.
 */
(function () {
  'use strict';

  angular
    .module('starter')
    .controller('MenuController', MenuController);

  MenuController.$inject = ['AuthenticationService','$ionicSideMenuDelegate'];

  function MenuController(AuthenticationService, $ionicSideMenuDelegate) {
    var vm = this;

    vm.logOut = function () {
      AuthenticationService.logOut();
      $ionicSideMenuDelegate.canDragContent(false);
    };


    $ionicSideMenuDelegate.canDragContent(false)

  }
})();

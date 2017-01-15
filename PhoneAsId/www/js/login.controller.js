/**
 * Created by NSPACE on 12/24/2016.
 */
(function () {
  'use strict';

  angular
    .module('starter')
    .controller('LoginController', LoginController);

  LoginController.$inject = ['AuthenticationService' ,'$ionicSideMenuDelegate' ,'$scope', '$ionicNavBarDelegate'];

  function LoginController(AuthenticationService,$ionicSideMenuDelegate, $scope,$ionicNavBarDelegate) {
    var vm = this;

    vm.loginEnabled = false;
    vm.logIn = signIn;
    hideSideMenu();

    function signIn() {
      AuthenticationService.signIn();
    }


    function hideSideMenu() {
      $scope.$on('$ionicView.enter', function () {
        $ionicSideMenuDelegate.canDragContent(false);

        //hide nav bar
        $ionicNavBarDelegate.showBar(false);
      })
    }
  }
})();

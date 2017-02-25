
angular.module('starter.controllers', ['ionic', 'starter.controllers', 'starter.services'])

.controller('AppCtrl', ['$scope', '$ionicModal', '$timeout', '$http', function($scope, $ionicModal, $timeout, $http) {

  // With the new view caching in Ionic, Controllers are only called
  // when they are recreated or on app start, instead of every page change.
  // To listen for when this page is active (for example, to refresh data),
  // listen for the $ionicView.enter event:
  //$scope.$on('$ionicView.enter', function(e) {
  //});

}]);

//   .controller('LoginCtrl', ['$log', '$scope', '$state', '$ionicPlatform', 'ngOidcClient','$http', 'AuthenticationService', function ($log, $scope, $state, $ionicPlatform, ngOidcClient,$http,AuthenticationService) {
//   $log.log('LoginCtrl loaded');
//
//   $scope.apptitle = "OIDC Demo";
//   $scope.loginEnabled = false;
//
//   $scope.logIn = function () {
//     AuthenticationService.signIn();
//   };//
//
// }]);

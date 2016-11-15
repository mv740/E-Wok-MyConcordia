
angular.module('starter.controllers', ['ionic', 'starter.controllers'])

.controller('AppCtrl', ['$scope', '$ionicModal', '$timeout', '$http', 'serverName', function($scope, $ionicModal, $timeout, $http, serverName) {

  // With the new view caching in Ionic, Controllers are only called
  // when they are recreated or on app start, instead of every page change.
  // To listen for when this page is active (for example, to refresh data),
  // listen for the $ionicView.enter event:
  //$scope.$on('$ionicView.enter', function(e) {
  //});

  // // Form data for the login modal
  // $scope.loginData = {};
  //
  // // Create the login modal that we will use later
  // $ionicModal.fromTemplateUrl('templates/login.html', {
  //   scope: $scope
  // }).then(function (modal) {
  //   $scope.modal = modal;
  // });
  //
  // // Triggered in the login modal to close it
  // $scope.closeLogin = function () {
  //   $scope.modal.hide();
  // };
  //
  // // Open the login modal
  // $scope.login = function () {
  //   $scope.modal.show();
  // };
  //
  // // Perform the login action when the user submits the login form
  // $scope.doLogin = function () {
  //   console.log('Doing login', $scope.loginData);
  //   var url = serverName + "student";
  //   $http.get(url).success(function (response) {
  //     alert(response[0].firstName);
  //   });
  //   // Simulate a login delay. Remove this and replace with your login
  //   // code if using a login system
  //   $timeout(function () {
  //     $scope.closeLogin();
  //   }, 1000);
  // };
}])

  .controller('LoginCtrl', ['$log', '$scope', '$state', '$ionicPlatform', 'ngOidcClient','$http', function ($log, $scope, $state, $ionicPlatform, ngOidcClient,$http) {
  $log.log('LoginCtrl loaded');

  $scope.apptitle = "OIDC Demo";
  $scope.loginEnabled = false;

  var signin = function () {
    ngOidcClient.signinPopup().then(function (user) {
      $log.log("user:" + JSON.stringify(user));
      if (!!user) {
        $log.log('Logged in so going to home state');
        $state.go('app.account');
      }
    });
  }

  $ionicPlatform.ready(function () {
    $log.log('HomeCtrl: Platform ready so attempting signin.');
    $scope.loginEnabled = true;
  });

  $scope.logIn = signin;

}])

  .controller('AccountCtrl', ['$log', '$scope', '$state', 'ngOidcClient','$http', function ($log, $scope, $state, ngOidcClient,$http) {
    $log.log('AccountCtrl');

    $scope.userInfo = {
      userData: {},
      claims: []
    };

    function processUserData() {
      var userInfo = ngOidcClient.getUserInfo();
      $scope.userInfo.userData = userInfo;
      if (userInfo && userInfo.user) {
        $scope.userInfo.claims = [];
        for (var property in userInfo.user.profile) {
          if (userInfo.user.profile.hasOwnProperty(property)) {
            $scope.userInfo.claims.push({
              key: property,
              value: userInfo.user.profile[property]
            });
          }
        }
      }
    }

    processUserData();

    ngOidcClient.userInfoChanged($scope, function () {
      $scope.$apply(function () {
        processUserData();
      });
    });

    $scope.logOut = function () {
      ngOidcClient.signoutPopup().then(function () {
        $state.go('login');
      });
    };

    console.log($scope.userInfo);
    $scope.Confirmation = function () {
      var headers = {};
      headers['Authorization'] = 'Bearer ' + $scope.userInfo.userData.access_token;

      var token = $scope.userInfo.userData.user.access_token;
      console.log(token);

    $http({
      method: 'GET',
      url: 'https://myconcordiaid.azurewebsites.net/api/message',
      headers: {'Authorization': 'Bearer '+token}
    }).then(function successCallback(response) {
      console.log(response.data);
    });
  }
  }]);


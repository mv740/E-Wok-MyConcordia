angular.module('starter.controllers', ['ionic', 'starter.controllers'])



  .controller('AppCtrl', ['$scope', '$ionicModal', '$timeout', '$http', 'serverName', function ($scope, $ionicModal, $timeout, $http, serverName) {

    // With the new view caching in Ionic, Controllers are only called
    // when they are recreated or on app start, instead of every page change.
    // To listen for when this page is active (for example, to refresh data),
    // listen for the $ionicView.enter event:
    //$scope.$on('$ionicView.enter', function(e) {
    //});

    // Form data for the login modal
    $scope.loginData = {};

    // Create the login modal that we will use later
    $ionicModal.fromTemplateUrl('templates/login.html', {
      scope: $scope
    }).then(function (modal) {
      $scope.modal = modal;
    });

    // Triggered in the login modal to close it
    $scope.closeLogin = function () {
      $scope.modal.hide();
    };

    // Open the login modal
    $scope.login = function () {
      $scope.modal.show();
    };

    // Perform the login action when the user submits the login form
    $scope.doLogin = function () {
      console.log('Doing login', $scope.loginData);
      var url = serverName + "student";
      $http.get(url).success(function (response) {
        alert(response[0].firstName);
      });
      // Simulate a login delay. Remove this and replace with your login
      // code if using a login system
      $timeout(function () {
        $scope.closeLogin();
      }, 1000);
    };
  }])


  .controller('LoginCtrl', function ($scope, $http) {



  })


  .controller('IdCtrl', function ($scope) {

  })

  .controller('IdCtrl', function ($scope, $window, $state, $http, $rootScope) {

    var studentid = '38901062';
    $scope.studentInfo = "";

    //TODO move get request to login activity and pass id after successful authentication
    $http.get('https://myconcordiaid.azurewebsites.net/api/student/' + studentid)
      .success(function (data) {
        console.log('data success');
        console.log(data);

        $scope.studentInfo = data;

        $scope.netname = data.netname;
        $scope.valid = data.valid;
        $scope.pending = data.pending;
        $scope.updatepicture = data.updatepicture;

        $rootScope.canUpdate = data.updatepicture;

        //variables to be displayed on id
        $scope.profilepicture = data.profilepicture;
        $scope.firstname = data.firstname;
        $scope.lastname = data.lastname;
        $scope.dob = data.dob;
        $scope.id = data.id;
        $scope.ugradstatus = data.ugradstatus;
        $scope.expiredate = data.expiredate;
      })
      .error(function (data) {
        console.log('data error');
      });

      $http.get("https://myconcordiaid.azurewebsites.net/api/student/UpdatePeriod").then(function successCallback(response) {
          // this callback will be called asynchronously
          // when the response is available
          $rootScope.validPeriod = response.data.canUpdatePicture;

        }, function errorCallback(response) {
          // called asynchronously if an error occurs
          // or server returns response with an error status.
          console.log(response);
        });



    $scope.screenOrientation = screen.orientation.type;

    //changes the template view used when the phones orientation changes
    $window.addEventListener("orientationchange", function () {
      console.log(screen.orientation.type); //"portrait-primary" or "landscape-secondary"
      $scope.screenOrientation = screen.orientation.type;
      $state.reload();
    });


  });



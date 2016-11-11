/**
 * Created by NSPACE on 11/11/2016.
 */

angular.module('starter.controllers')
  
  .controller('IdCtrl', function ($scope, $window, $state, $http, $rootScope, StudentService) {
    $scope.studentInfo = '';
    getStudentIdInfo();

    /**
     * Makes a rest api call using the StudentService  object. This is used to get the student information
     * that will be displayed on the ID page as well as if their id is valid. When the students id is not
     * valid a temporary 'pending approval' id will be shown instead.
     */

    function getStudentIdInfo() {
      StudentService.fetchStudentIdInfo()
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
        .error(function (error) {
          console.log('data error');
        });
    }

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
    
  })

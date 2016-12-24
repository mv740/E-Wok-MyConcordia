/**
 * Created by NSPACE on 11/11/2016.
 *
 * Controller for both student id page and marshalling card info page
 */

angular.module('starter.controllers')

  .controller('IdCtrl', function ($scope, $window, $state, $http, $rootScope, StudentService) {
    $scope.showMarshallingButton;
    $scope.studentInfo = '';
    $scope.marshallingInfo = '';
    getStudentIdInfo();
    getMarshallingCardInfo();

    /**
     * Makes a rest api call using the StudentService  object. This is used to get the student information
     * that will be displayed on the ID page as well as if their id is valid. When the students id is not
     * valid a temporary 'pending approval' id will be shown instead.
     */

    function getStudentIdInfo() {
      StudentService.fetchStudentIdInfo()
        .success(function (data) {
          console.log('fetchStudentIdInfo data success');
          console.log(data);

          $scope.studentInfo = data;

          $scope.netname = data.netname;
          $scope.valid = data.valid;
          $scope.pending = data.pending;

          if($scope.valid == false && $scope.pending == false)
          {
            $state.go('app.camera');
          }

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
          console.log('fetchStudentIdInfo data error');
        });
    }

    /**
     * Makes a rest api call using the StudentService  object. This is used to get the marshalling card information
     * that will be displayed on the marshalling card info page. If a student has a valid marshalling card available a button
     * from the regular student id to the marshalling card will be shown.
     */

    function getMarshallingCardInfo() {
      StudentService.fetchMarshallingCardInfo()
        .success(function (data) {
          console.log('fetchMarshallingCardInfo data success');
          console.log(data);

          $scope.marshallingInfo = data;

          if(data.status == true) {
            console.log('fetchMarshallingCardInfo status true');
            $scope.showMarshallingButton = true;


            //variables to be displayed on marshalling card info page
            $scope.semester = data.card.semester;
            $scope.year = data.card.year;
            $scope.marshallingCode = data.card.marshallingCode;
            $scope.department = data.card.department;
            $scope.location = data.card.location;
            $scope.dateTime = data.card.dateTime;
            $scope.degree = data.card.degree;
            $scope.sid = data.card.sid;
          }
        })
        .error(function (error) {
          console.log('fetchMarshallingCardInfo data error');
        });
    }

    $scope.loadMarshallingCard = function() {
      $state.go('app.marshalling');
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

/**
 * Created by NSPACE on 11/11/2016.
 *
 * Controller for both student id page and marshalling card info page
 */
(function () {
  'use strict';

  angular
    .module('starter')
    .controller('IdController', IdController);

  IdController.$inject = ['$rootScope', '$scope', '$state', 'StudentService'];

  function IdController($rootScope, $scope, $state, StudentService) {
    var vm = this;

    vm.studentInfo = {};
    vm.marshallingInfo = {};
    vm.loadMarshallingCard = loadMarshallingCard;


    //refresh on every load of this page
    $scope.$on('$ionicView.beforeEnter', function (e) {

      //http://stackoverflow.com/questions/30236425/ionic-framework-ionicview-entered-event-fired-twice
      //prevent entered event fired twice
      if (e.targetScope !== $scope) {
        return;
      } else {
        console.log("enter");

        getUpdatePeriod();
        getStudentIdInfo();
        getMarshallingCardInfo();
      }
    });

    /////////////////////////////////////

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

          vm.studentInfo = data;

          vm.valid = data.valid;
          vm.netname = data.netname;
          vm.pending = data.pending;

          $rootScope.valid = data.valid;
          $rootScope.pending = data.pending;

          if (vm.valid == false && vm.pending == false) {
            $state.go('app.camera');
          }

          vm.updatePicture = data.updatePicture;
          $rootScope.canUpdate = data.updatePicture;

          //variables to be displayed on id
          vm.profilePicture = data.profilePicture;
          vm.firstName = data.firstName;
          vm.lastName = data.lastName;
          parseDate(data.dob);
          vm.id = data.id;
          vm.uGradStatus = data.uGradStatus;
          parseExpirationDate(data.expireDate);
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

          vm.marshallingInfo = data;

          if (data.status == true) {
            console.log('fetchMarshallingCardInfo status true');
            vm.showMarshallingButton = true;


            //variables to be displayed on marshalling card info page
            vm.semester = data.card.semester;
            vm.year = data.card.year;
            vm.marshallingCode = data.card.marshallingCode;
            vm.department = data.card.department;
            vm.location = data.card.location;
            vm.dateTime = data.card.dateTime;
            vm.degree = data.card.degree;
            vm.sid = data.card.sid;
          }
        })
        .error(function (error) {
          console.log('fetchMarshallingCardInfo data error');
        });
    }

    function loadMarshallingCard() {
      $state.go('app.marshalling');
    }

    function getUpdatePeriod() {
      StudentService.fetchUpdatePeriod()
        .then(function successCallback(response) {
          // this callback will be called asynchronously
          // when the response is available
          console.log("updatePeriod");
          console.log(response);
          $rootScope.validPeriod = response.data.canUpdatePicture;

        }, function errorCallback(response) {
          // called asynchronously if an error occurs
          // or server returns response with an error status.
          console.log(response);
        });
    }

    function parseDate(dateUTC) {
      let d = new Date(dateUTC);
      let day = d.getUTCDate();
      let month = d.getMonth() + 1;
      let year = d.getUTCFullYear();

      vm.dob = month + "/" + day + "/" + year;
    }

    function parseExpirationDate(dateUTC) {
      let d = new Date(dateUTC);
      let year = d.getYear();
      let month = d.toLocaleString("en-us", {month: "short"});


      vm.expireDate = month + ". " + year;
    }

  }
})();

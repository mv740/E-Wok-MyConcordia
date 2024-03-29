/**
 * Created by Simon on 2017-01-16.
 */

(function () {
  'use strict';

  angular
    .module('starter')
    .controller('BarcodeController', BarcodeController);

  BarcodeController.$inject = ['$rootScope', '$scope', '$state', '$cordovaBarcodeScanner', '$cordovaNativeAudio', 'EventService', 'StudentService'];

  function BarcodeController($rootScope, $scope, $state, $cordovaBarcodeScanner, $cordovaNativeAudio, EventService, StudentService) {
    var bc = this;

    bc.scanData = [];
    bc.barcodeData;
    bc.barcodeID;

    bc.responseMsg = '';

    $cordovaNativeAudio.preloadSimple('beep', 'audio/beep.mp3');

    $scope.$on('$ionicView.beforeEnter', function (e) {
      if (e.targetScope !== $scope) {
        return;
      } else {
        bc.eventData = EventService.data;
        console.log(bc.eventData);
      }
    });


    bc.scanBarcode = function () {
      $cordovaBarcodeScanner.scan().then(function (imageData) {
        if (imageData.text != '') {
          $cordovaNativeAudio.play('beep');

          bc.scanData.push(imageData.text);

          bc.barcodeData = imageData.text;
          console.log("Barcode Data: " + bc.barcodeData.length);

          console.log("Barcode id length: " + bc.barcodeData);
          if(bc.barcodeData.length == 8) { //handles barcode ids generated on the phone
            bc.barcodeID = bc.barcodeData;
          } else if (bc.barcodeData.length == 14) { //handles barcodes ids from physical card
            bc.barcodeID = bc.barcodeData.substring(4, 12);

          }
          console.log("Barcode ID: " + bc.barcodeID);

          var userParameter = {};
          userParameter.studentId = bc.barcodeID;
          userParameter.type = bc.eventData.information.type;
          userParameter.eventID = bc.eventData.information.eventID;
          console.log(userParameter);

          validateEventAttendee(userParameter);
        }
      }, function (error) {
        console.log("An error happened -> " + error);
      });
    };

    function validateEventAttendee(userParameter) {
      StudentService.validateEventAttendee(userParameter)
        .then(function successCallback(response) {
          console.log(response.data);
          bc.responseMsg = response.data.status;
        }, function errorCallback(response) {
          console.log(response);
          bc.responseMsg = "FAIL - ID does not exist";
        });
    }
  }
})();

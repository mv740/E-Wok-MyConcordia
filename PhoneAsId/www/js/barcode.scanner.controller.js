/**
 * Created by Simon on 2017-01-16.
 */

(function () {
  'use strict';

  angular
    .module('starter')
    .controller('BarcodeController', BarcodeController);

  BarcodeController.$inject = ['$rootScope', '$scope', '$state', '$cordovaBarcodeScanner', '$cordovaNativeAudio', 'EventService'];

  function BarcodeController($rootScope, $scope, $state, $cordovaBarcodeScanner, $cordovaNativeAudio, EventService) {
    var bc = this;

    bc.scanData = [];
    $cordovaNativeAudio.preloadSimple('beep', 'audio/beep.mp3');
    bc.eventData = EventService.data;

    bc.scanBarcode = function() {
      $cordovaBarcodeScanner.scan().then(function(imageData) {
        if(imageData.text != ''){
          $cordovaNativeAudio.play('beep');
          bc.scanData.push(imageData.text);
        }
      }, function(error) {
        console.log("An error happened -> " + error);
      });
    };

  }

})();

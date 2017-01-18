/**
 * Created by Simon on 2017-01-16.
 */

(function () {
  'use strict';

  angular
    .module('starter')
    .controller('BarcodeController', BarcodeController);

  BarcodeController.$inject = ['$rootScope', '$scope', '$state', '$cordovaBarcodeScanner', '$cordovaNativeAudio'];

  function BarcodeController($rootScope, $scope, $state, $cordovaBarcodeScanner, $cordovaNativeAudio) {
    var bc = this;

    bc.scanData = [];
    $cordovaNativeAudio.preloadSimple('beep', 'audio/beep.mp3');

    bc.scanBarcode = function() {
      $cordovaBarcodeScanner.scan().then(function(imageData) {
        $cordovaNativeAudio.play('beep');
        bc.scanData.push(imageData.text);
      }, function(error) {
        console.log("An error happened -> " + error);
      });
    };

  }

})();

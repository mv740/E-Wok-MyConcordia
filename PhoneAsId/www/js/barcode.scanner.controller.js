/**
 * Created by Simon on 2017-01-16.
 */

(function () {
  'use strict';

  angular
    .module('starter')
    .controller('BarcodeController', BarcodeController);

  BarcodeController.$inject = ['$rootScope', '$scope', '$state', '$cordovaBarcodeScanner'];

  function BarcodeController($rootScope, $scope, $state, $cordovaBarcodeScanner) {
    var bc = this;

    bc.scanData = [];

    bc.scanBarcode = function() {
      $cordovaBarcodeScanner.scan().then(function(imageData) {
        alert(imageData.text);
        bc.scanData.push(imageData.text);
      }, function(error) {
        console.log("An error happened -> " + error);
      });
    };

  }

})();

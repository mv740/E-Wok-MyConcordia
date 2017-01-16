/**
 * Created by Simon on 2017-01-16.
 */

(function () {
  'use strict';

  angular
    .module('starter')
    .controller('BarcodeController', BarcodeController);

  BarcodeController.$inject = ['$rootScope', '$scope', '$state'];

  function BarcodeController($rootScope, $scope, $state) {

    var bc = this;

    bc.text = "Hello";

  }

})();

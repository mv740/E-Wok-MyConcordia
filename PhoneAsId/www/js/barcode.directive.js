/**
 * Created by Simon on 2017-01-11.
 */
(function() {
  'use strict';

angular
  .module('starter')
  .directive('barcodeDirective', barcodeDirective);

function barcodeDirective(){
  var directive = {
    restrict: 'EA',
    template: '<canvas class="barcodegen"></canvas>',
    link:linkFunc,
    controller: 'IdController',
    controllerAs: 'vm',
    bindToController: true

  };
  return directive;

  function linkFunc(scope, el, attr, ctrl){
    var canvas = el.find('canvas');
    new JsBarcode(canvas[0], "hello");
  }
}})();



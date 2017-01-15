/**
 * Created by Simon on 2017-01-11.
 */
(function () {
  'use strict';

  angular
    .module('starter')
    .directive('barcodeDirective', barcodeDirective);

  function barcodeDirective() {

    var directive = {
      restrict: 'EA',
      template: '<canvas class="barcodegen"></canvas>',
      scope: {
        bcode: '=',
        height: '=',
        fontSize: '='
      },
      link: linkFunc,

    };
    return directive;

    function linkFunc(scope, el, attr, ctrl) {
      scope.$watch(function () {
          return scope.bcode;
        },

        function () {
          var canvas = el.find('canvas');
          console.log("scope.bar => " + scope.height);
          console.log("scope.fontsize => " + scope.fontSize);

          var height = scope.bar;
          new JsBarcode(canvas[0], scope.bcode, {
            format: "CODE39",
            height: scope.height,
            fontSize: scope.fontSize,
          });
        }
      );
    }
  }
})();



'use strict';

angular
    .module('myApp')
    .directive('kbPress', kbPress);

function kbPress(){

    return {
        restrict: 'A',
        link: function($scope, $element, $attrs) {
            $element.bind("keypress", function(event) {
                var keyCode = event.which || event.keyCode;

                if (keyCode == $attrs.code) {
                    $scope.$apply(function() {
                        $scope.$eval($attrs.dlKeyCode, {$event: event});
                    });

                }
            });
        }
    };
}
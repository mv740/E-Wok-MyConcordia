/**
 * Created by franc on 10/14/2016.
 */

'use strict';

angular.module('myApp.view1.searchResults', [])

    .controller('SearchResultsCtrl', ['$scope', function($scope) {
        $scope.results = [{
            name: 'francis'
        },{
            name: 'michal'
        }];
    }]);
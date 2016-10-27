/**
 * Created by franc on 10/14/2016.
 */

'use strict';

angular.module('myApp.view1.searchResults', [])

    .controller('SearchResultsCtrl', ['$rootScope', '$scope', function ($rootScope, $scope) {

        var searchResults = $scope;

        searchResults.results = [{
            "firstName": "Francis",
            "lastName": "Cote-Tremblay",
            "id": "9999999",
            "gallery": {
                "toValidate": "images/francis1.jpg",
                "validated": ["images/francis2.jpg", "images/francis3.jpg"]
            },
            "birthdate": {
                "day": "10",
                "month": "april",
                "year": "1992"
            },
            "netname": "f_cotetr"
        }, {
            "firstName": "Michal",
            "lastName": "Wozniak",
            "id": "88888",
            "gallery": {
                "toValidate": "images/michal1.jpg",
                "validated": ["images/michal2.jpg", "images/michal3.jpg"]
            },
            "birthdate": {
                "day": "1",
                "month": "january",
                "year": "1999"
            },
            "netname": "m_wozniak"
        }];

        searchResults.studentClick = function (student) {
            $rootScope.$broadcast('updateSearchModal', student );
        };

        
    }]);
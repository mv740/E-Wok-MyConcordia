/**
 * Created by franc on 10/14/2016.
 */

'use strict';

angular
    .module('myApp')
    .controller('SearchResultsCtrl', ['$rootScope', function ($rootScope) {

        var searchResults = this;

        searchResults.results = [{
            "firstname": "Francis",
            "lastname": "Cote-Tremblay",
            "id": "9999999",
            "birthdate": {
                "day": "10",
                "month": "april",
                "year": "1992"
            },
            "netname": "f_cotetr"
        }, {
            "firstname": "Michal",
            "lastname": "Wozniak",
            "id": "38901062",
            "birthdate": {
                "day": "1",
                "month": "january",
                "year": "1999"
            },
            "netname": "m_wozniak"
        }];

        searchResults.studentClick = function (student) {
            $rootScope.$broadcast('searchModal.updateSearchModal', student );
        };

        
    }]);
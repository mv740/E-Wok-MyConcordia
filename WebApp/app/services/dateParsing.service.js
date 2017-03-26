'use strict';

var months = ["January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
];

angular
    .module('myApp')
    .factory('dateParsingService', dateParsingService);

dateParsingService.$inject = ['$filter'];

function dateParsingService($filter) {

    var service = {
        parse: parse,
        parseUpdatePeriod: parseUpdatePeriod
    };

    return service;
    /////////////////////

    function parse(datetime){
        var date = {};

        date.year = datetime.substring(0,4);
        var monthNum = datetime.substring(5,7);
        date.month = months[parseInt(monthNum - 1)];
        date.day = datetime.substring(8,10);

        return date;
    }

    function parseUpdatePeriod(updatePeriod){
        var dateSplit = updatePeriod.split("/"); // the format provided by oracle is M/d/yyyy
        dateSplit[2] = dateSplit[2].substring(0,4);

        var year = dateSplit[2];
        var month = dateSplit[0] - 1; // have to do -1 because date object in angular uses 0 indexed months.
        var day = dateSplit[1];


        var date = new Date(year, month, day);

        return date.toLocaleDateString(); // parse date into localized settings according to the configuration of the browser.
    }

}
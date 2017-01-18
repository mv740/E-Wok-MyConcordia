'use strict';

var months = ["January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
];

angular
    .module('myApp')
    .factory('dateParsingService', dateParsingService);


function dateParsingService() {

    var service = {
        parse: parse
    };

    return service;
    /////////////////////

    function parse(datetime){
        var date = {};

        date.day = datetime.substring(0,2);
        var monthNum = datetime.substring(3,5);
        date.month = months[parseInt(monthNum - 1)];
        date.year = datetime.substring(6,10);

        return date;
    }

}
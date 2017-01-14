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

        date.year = datetime.substring(0,4);
        var monthNum = datetime.substring(5,7);
        date.month = months[parseInt(monthNum)];
        date.day = datetime.substring(8,10);

        return date;
    }

}
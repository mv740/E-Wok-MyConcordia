'use strict';

angular
    .module('myApp')
    .factory('searchParsingService', searchParsing);

var idLength = 8;
var months = ["january", "february", "march", "april", "may", "june", "july", "august", "september", "october", "november", "december"];


function searchParsing() {

    var service = {
        parseSearchInput: parseSearchInput
    };

    return service;

    /////////////////////

    function parseSearchInput(input) {
        //split by either a whitespace, a comma,a backslash a frontslash, a comma followed by a space or a dash
        var params = input.split(/,\s|-|\s|\\|\//);

        //dont modify order. The name is the result from splicing the id, birthdate and netname from the params.
        //after each finding, the params array is spliced
        var id = findID(params);
        var netname = findNetname(params);
        var birdthdate = findBirthdate(params);
        var name = params;

        var parsed = {
            'id': id,
            'netname': netname,
            'birthdate': birdthdate,
            'name': name
        };

        return parsed;
    }


    function findID(params) {
        for (var i = 0; i < params.length; i++) {
            if (params[i].match(/(\d{8})/)) {
                var id = params[i];
                params.splice(i, 1);
                return id;
            }
        }
        return null;
    }

    function findNetname(params) {
        for (var i = 0; i < params.length; i++) {
            if (params[i].match(/(_)/)) {
                var netname = params[i];
                params.splice(i, 1);
                return netname;
            }
        }
        return null;
    }

    function findBirthdate(params) {

        var birthdate = {};
        paramLoop:
        for (var i = 0; i < params.length; i++) {


            //find year
            if (params[i].match(/(\d{4})/)) {
                birthdate.year = parseInt(params[i]);
                params.splice(i, 1);
                i--;
                continue;
            }
            
            //find month
            for (var j = 0; j < months.length; j++) {
                if (params[i].toLowerCase().match(months[j].toLowerCase())) {
                    birthdate.month = j;
                    params.splice(i, 1);
                    i--;
                    continue paramLoop;
                }
            }

        }

        birthdate.day = findBirthdateDay(params);

        var datum = new Date(Date.UTC(birthdate.year,birthdate.month,birthdate.day,0,0,0));

        return datum;
    }

    function findBirthdateDay(params) {

        for (var i = 0; i < params.length; i++) {

            if (params[i].match(/(\d{1,2})/)) {
                var day = params[i];
                params.splice(i, 1);

                //if there is a th, nd or other suffix, take only the integer part
                if (day.length > 2) {
                    return day.substring(0,2);
                }
                return parseInt(day);
            }

        }
    }

}
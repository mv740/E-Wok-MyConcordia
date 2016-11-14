'use strict';

angular
    .module('myApp')
    .factory('searchParsingService', studentService);

var idLength = 8;
var months = ["january", "february", "march", "april", "may", "june", "july", "august", "september", "october", "november", "december"];


function studentService() {

    var service = {
        parseSearchInput: parseSearchInput
    };

    return service;

    /////////////////////

    function parseSearchInput(input) {
        //split by either a whitespace, a comma,a backslash or a frontslash
        var params = searchBox.toFind.split(/(\s|,|\\|\/)/);

        //dont modify order. The name is the result from splicing the id, birthdate and netname from the params.
        //after each finding, the params array is spliced
        var id = findID(params);
        var netname = findNetname(params);
        var birdthdate = findBirthdate(params);
        var name = params;

        var parsed = {
            'id': id,
            'netname': netname,
            'birdthdate': birdthdate,
            'name': name
        }

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
        for (var i = 0; i < params.length; i++) {


            //find year
            if (params[i].match(/(\d{4})/)) {
                 birtdate.year = params[i];
                 params.splice(i, 1);
            }
            
            //find month
            for (var j = 0; j < months.length; j++) {
                if (params[i].match(months[j])) {
                    birtdate.month = params[i];
                    params.splice(i, 1);
                }
            }

        }

        //search only for day if a year is found. Otherwise it means that the year and day could be mistaken if they were written as dd/month/yy instead of dd/month/yyyy
        if (birthdate.year) {
           birthdate.day = findBirthdateDay(params);
        }

        return birthdate;
    }

    function findBirthdateDay(params) {

        for (var i = 0; i < params.length; i++) {

            if (params[i].match(/(\d{1,2})/)) {
                var day = params[i];
                params.splice(i, 1);
                return day;
            }

        }
    }

}
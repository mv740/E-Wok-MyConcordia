/**
 * Created by franc on 10/14/2016.
 */

'use strict';

angular
    .module('myApp')
    .controller('SearchCtrl', SearchCtrl);

SearchCtrl.$inject = ['$rootScope', 'studentService', 'searchParsingService'];

function SearchCtrl($rootScope, studentService, searchParsingService) {

    var search = this;

    search.find = find;
    search.studentClick = studentClick;
    search.initialState = true;
    search.input = "";

    ///////////////////

    function find() {
        search.initialState = false;
        search.searching = true;
        search.results = [];

        //temporary until find can parse parameters
        if (search.input.length == 0){
            getAllStudents();
        }
        else{
            var params = searchParsingService.parseSearchInput(search.input);
            alert(JSON.stringify(params));
            studentService.search(params).then(function (value) {
                setResults(value.data);
             });
        }

    }

    //temporary until find can parse parameters
    function getAllStudents() {
        studentService.getStudents().then(function (value) {
            setResults(value.data);
        });
    }

    function studentClick(student) {
        $rootScope.$broadcast('modals.update', student);
    }

    function setResults(results){
        search.searching = false;
        search.results = results;
    }
}
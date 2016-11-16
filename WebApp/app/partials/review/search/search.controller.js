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

    ///////////////////

    function find() {
        search.initialState = false;
        search.searching = true;

        //temporary until find can parse parameters
        //getAllStudents();

        var params = searchParsingService.parseSearchInput(search.input);
        //alert(JSON.stringify(params));
        studentService.search(params).then(function (value) {
            search.results = value.data;
        });
    }

    //temporary until find can parse parameters
    function getAllStudents() {
        studentService.getStudents().then(function (value) {
            search.searching = false;
            search.results = value.data;
        });
    }

    function studentClick(student) {
        $rootScope.$broadcast('modals.update', student);
    }
}
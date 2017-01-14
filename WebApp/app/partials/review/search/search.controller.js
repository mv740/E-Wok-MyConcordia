/**
 * Created by franc on 10/14/2016.
 */

'use strict';

angular
    .module('myApp')
    .controller('SearchCtrl', SearchCtrl);

SearchCtrl.$inject = ['$modal', 'studentService', 'searchParsingService', 'dateParsingService'];

function SearchCtrl($modal, studentService, searchParsingService, dateParsingService) {

    var search = this;

    search.find = find;
    search.studentClick = studentClick;
    search.initialState = true;
    search.input = "";
    search.emptyResults = false;

    ///////////////////

    function find() {
        search.initialState = false;
        search.searching = true;
        search.results = [];
        search.emptyResults = false;

        //temporary until find can parse parameters
        if (search.input.length == 0){
            getAllStudents();
        }
        else{
            var params = searchParsingService.parseSearchInput(search.input);
            console.log(JSON.stringify(params));
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
        $modal.open({templateUrl: "partials/review/modals/studentModal/studentModal.html",
            controller: 'StudentModalCtrl as studentModal',
            windowClass: 'app-modal-window',
            keyboard: true,
            resolve: {
                student: function () {
                    return student;
                }
            }});
    }

    function setResults(results) {
        search.searching = false;
        search.results = results;
        search.emptyResults = search.results.length == 0;
        parseDOB();
    }

    function parseDOB(){
        search.results.forEach(function(result){
            var birthdate = dateParsingService.parse(result.dob);
            result.birthdate = birthdate;
        });
    }
}
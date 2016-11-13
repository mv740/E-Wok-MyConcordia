/**
 * Created by franc on 10/14/2016.
 */

'use strict';

angular
    .module('myApp')
    .controller('SearchCtrl', SearchCtrl);

SearchCtrl.$inject = ['$rootScope', 'studentService'];

function SearchCtrl($rootScope, studentService) {

    var search = this;

    search.find = find;
    search.studentClick = studentClick;
    search.initialState = true;

    ///////////////////

    function find() {
        search.initialState = false;
        search.searching = true;

        studentService.getStudents().then(function (value) {
            search.searching = false;
            search.results = value.data;
        });
    }

    function studentClick(student) {
        $rootScope.$broadcast('modals.update', student);
    }
}
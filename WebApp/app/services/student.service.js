'use strict';

angular
    .module('myApp')
    .factory('studentService', studentService);

studentService.$inject = ['$q', 'toastedHttpService', 'myConfig'];

function studentService($q, toastedHttp, myConfig) {

    var service = {
        sendValidation: sendValidation,
        getStudentPictures: getStudentPictures,
        getStudentLogs: getStudentLogs,
        getStudents: getStudents,
        search: search,
        getUpdatePeriod: getUpdatePeriod,
        validateArchived: validateArchived,
        submitComment: submitComment
    };

    return service;
    /////////////////////

    function sendValidation(id, valid) {
        var json = {
            id: parseInt(id),
            valid: valid
        };

        return toastedHttp.post(json, myConfig.validatePhoto);
    }

    function getStudentPictures(id) {
        return toastedHttp.get({param: id, topUrl: myConfig.getStudentPictures});
    }

    function getStudentLogs(netname) {
        return toastedHttp.get({param: netname, topUrl: myConfig.getLogs});
    }

    function getStudents() {
        return toastedHttp.get({topUrl: myConfig.getStudents});
    }

    function search(params) {
        return toastedHttp.post(params, myConfig.search);
    }

    function getUpdatePeriod() {
        return toastedHttp.get({topUrl: myConfig.getUpdatePeriod});
    }

    function validateArchived(id, valid){
        var json = { id: id,
            valid: valid};

        return toastedHttp.post(json, myConfig.validateArchived);

    }

    function submitComment(id, comment){
        var json = { id: id,
            comment: comment};

        return toastedHttp.post(json, myConfig.submitComment);
    }

}
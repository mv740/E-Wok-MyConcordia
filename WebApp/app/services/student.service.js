'use strict';

angular
    .module('myApp')
    .factory('studentService', studentService);

studentService.$inject = ['$q', 'toastedHttpService', 'myConfig', 'studentToastFeedback'];

function studentService($q, toastedHttp, myConfig, studentToastFeedback) {

    var service = {
        sendValidation: sendValidation,
        getStudentPictures: getStudentPictures,
        getStudentLogs: getStudentLogs,
        getStudents: getStudents,
        search: search,
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

        var settings = {
            data: json,
            topUrl: myConfig.validatePhoto,
            responseMsg: "",
            failureMsg: studentToastFeedback.sendValidation.failureMsg
        };

        if (valid)
            settings.responseMsg = studentToastFeedback.sendValidation.responseMsg.valid;
        else
            settings.responseMsg = studentToastFeedback.sendValidation.responseMsg.invalid;

        return toastedHttp.post(settings);
    }

    function getStudentPictures(id) {
        var settings = {
            param: id,
            topUrl: myConfig.getStudentPictures,
            failureMsg: studentToastFeedback.getStudentPictures.failureMsg
        };
        return toastedHttp.get(settings);
    }

    function getStudentLogs(netname) {
        var settings = {
            param: netname,
            topUrl: myConfig.getLogs,
            failureMsg: studentToastFeedback.getStudentLogs.failureMsg
        };
        return toastedHttp.get(settings);
    }

    function getStudents() {
        var settings = {
            topUrl: myConfig.getStudents,
            failureMsg: studentToastFeedback.getStudents.failureMsg
        };
        return toastedHttp.get(settings);
    }

    function search(params) {
        var settings = {
            data: params,
            topUrl: myConfig.search,
            responseMsg: "Student found",
            failureMsg: studentToastFeedback.search.failureMsg
        };
        return toastedHttp.post(settings);
    }



    function validateArchived(id, valid){
        var json = {
            id: id,
            valid: valid
        };

        var settings = {
            data: json,
            topUrl: myConfig.validateArchived,
            responseMsg: "",
            failureMsg: studentToastFeedback.validateArchived.failureMsg
        };

        if (valid)
            settings.responseMsg = studentToastFeedback.validateArchived.responseMsg.valid;
        else
            settings.responseMsg = studentToastFeedback.validateArchived.responseMsg.invalid;

        return toastedHttp.post(settings);
    }

    function submitComment(id, comment){
        var json = { id: id,
            comment: comment};

        var settings = {
            data: json,
            topUrl: myConfig.submitComment,
            responseMsg: studentToastFeedback.submitComment.responseMsg,
            failureMsg: studentToastFeedback.submitComment.failureMsg
        };
        return toastedHttp.post(settings);
    }

}
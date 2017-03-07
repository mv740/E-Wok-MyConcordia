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
            failureMsg: {
                401: "Please Login",
                404: "No picture found",
                500: "Sorry! Our servers are down :("
            }
        };

        if (valid)
            settings.responseMsg = "Picture Validated";
        else
            settings.responseMsg = "Picture Revoked";

        return toastedHttp.post(settings);
    }

    function getStudentPictures(id) {
        var settings = {
            param: id,
            topUrl: myConfig.getStudentPictures,
            failureMsg: {
                401: "Please Login",
                404: "No student found",
                500: "Sorry! Our servers are down :("
            }
        };
        return toastedHttp.get(settings);
    }

    function getStudentLogs(netname) {
        var settings = {
            param: netname,
            topUrl: myConfig.getLogs,
            failureMsg: {
                401: "Please Login",
                404: "No logs found",
                500: "Sorry! Our servers are down :("
            }
        };
        return toastedHttp.get(settings);
    }

    function getStudents() {
        var settings = {
            topUrl: myConfig.getStudents,
            failureMsg: {
                401: "Please Login",
                404: "Error",
                500: "Sorry! Our servers are down :("
            }
        };
        return toastedHttp.get(settings);
    }

    function search(params) {
        var settings = {
            data: params,
            topUrl: myConfig.search,
            responseMsg: "Student found",
            failureMsg: {
                401: "Please Login",
                404: "Error",
                500: "Sorry! Our servers are down :("
            }
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
            failureMsg: {
                401: "Please Login",
                404: "No archive picture found",
                500: "Sorry! Our servers are down :("
            }
        };

        if (valid)
            settings.responseMsg = "Archive picture validated";
        else
            settings.responseMsg = "Archive picture revoked";

        return toastedHttp.post(settings);
    }

    function submitComment(id, comment){
        var json = { id: id,
            comment: comment};

        var settings = {
            data: json,
            topUrl: myConfig.submitComment,
            responseMsg: "Commented",
            failureMsg: {
                401: "Please Login",
                404: "Couldn't comment. Student not found",
                500: "Sorry! Our servers are down :("
            }
        };
        return toastedHttp.post(settings);
    }

}
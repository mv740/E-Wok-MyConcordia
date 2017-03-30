'use strict';

angular
    .module('myApp')
    .factory('studentService', studentService);

studentService.$inject = ['$q', 'translateService', 'toastedHttpService', 'myConfig'];

function studentService($q, translateService, toastedHttp, myConfig) {

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
        var localizationPromises;

        var json = {
            id: parseInt(id),
            valid: valid
        };

        var settings = {
            data: json,
            topUrl: myConfig.validatePhoto,
            responseMsg: "",
            failureMsg: {
                401: "",
                404: "",
                500: ""
            }
        };

        if (valid)
            localizationPromises = {
                responseMsg: translateService.getTranslation("TOASTFEEDBACK.STUDENT.SENDVALIDATION.responseMsg.valid"),
                401: translateService.getTranslation("TOASTFEEDBACK.STUDENT.SENDVALIDATION.failureMsg.401"),
                404: translateService.getTranslation("TOASTFEEDBACK.STUDENT.SENDVALIDATION.failureMsg.404"),
                500: translateService.getTranslation("TOASTFEEDBACK.STUDENT.SENDVALIDATION.failureMsg.500")
            };
        else
            localizationPromises = {
                responseMsg: translateService.getTranslation("TOASTFEEDBACK.STUDENT.SENDVALIDATION.responseMsg.invalid"),
                401: translateService.getTranslation("TOASTFEEDBACK.STUDENT.SENDVALIDATION.failureMsg.401"),
                404: translateService.getTranslation("TOASTFEEDBACK.STUDENT.SENDVALIDATION.failureMsg.404"),
                500: translateService.getTranslation("TOASTFEEDBACK.STUDENT.SENDVALIDATION.failureMsg.500")
            };

        return $q.all(localizationPromises)
            .then(function (translations) {
                settings.responseMsg = translations.responseMsg;
                settings.failureMsg[401] = translations[401];
                settings.failureMsg[404] = translations[404];
                settings.failureMsg[500] = translations[500];
                return toastedHttp.post(settings);
            });
    }

    function getStudentPictures(id) {
        var localizationPromises = {
            401: translateService.getTranslation("TOASTFEEDBACK.STUDENT.GETSTUDENTPICTURES.failureMsg.401"),
            404: translateService.getTranslation("TOASTFEEDBACK.STUDENT.GETSTUDENTPICTURES.failureMsg.404"),
            500: translateService.getTranslation("TOASTFEEDBACK.STUDENT.GETSTUDENTPICTURES.failureMsg.500")
        };

        var settings = {
            param: id,
            topUrl: myConfig.getStudentPictures,
            failureMsg: {
                401: "",
                404: "",
                500: ""
            }
        };

        return $q.all(localizationPromises)
            .then(function (translations) {
                settings.failureMsg[401] = translations[401];
                settings.failureMsg[404] = translations[404];
                settings.failureMsg[500] = translations[500];
                return toastedHttp.get(settings);
            });
    }

    function getStudentLogs(netname) {
        var localizationPromises = {
            401: translateService.getTranslation("TOASTFEEDBACK.STUDENT.GETSTUDENTLOGS.failureMsg.401"),
            404: translateService.getTranslation("TOASTFEEDBACK.STUDENT.GETSTUDENTLOGS.failureMsg.404"),
            500: translateService.getTranslation("TOASTFEEDBACK.STUDENT.GETSTUDENTLOGS.failureMsg.500")
        };

        var settings = {
            param: netname,
            topUrl: myConfig.getLogs,
            failureMsg: {
                401: "",
                404: "",
                500: ""
            }
        };

        return $q.all(localizationPromises)
            .then(function (translations) {
                settings.failureMsg[401] = translations[401];
                settings.failureMsg[404] = translations[404];
                settings.failureMsg[500] = translations[500];
                return toastedHttp.get(settings);
            });
    }

    function getStudents() {
        var localizationPromises = {
            401: translateService.getTranslation("TOASTFEEDBACK.STUDENT.GETSTUDENTS.failureMsg.401"),
            404: translateService.getTranslation("TOASTFEEDBACK.STUDENT.GETSTUDENTS.failureMsg.404"),
            500: translateService.getTranslation("TOASTFEEDBACK.STUDENT.GETSTUDENTS.failureMsg.500")
        };

        var settings = {
            topUrl: myConfig.getStudents,
            failureMsg: {
                401: "",
                404: "",
                500: ""
            }
        };

        return $q.all(localizationPromises)
            .then(function (translations) {
                settings.failureMsg[401] = translations[401];
                settings.failureMsg[404] = translations[404];
                settings.failureMsg[500] = translations[500];
                return toastedHttp.get(settings);
            });
    }

    function search(params) {
        var localizationPromises = {
            responseMsg: translateService.getTranslation("TOASTFEEDBACK.STUDENT.SEARCH.responseMsg"),
            401: translateService.getTranslation("TOASTFEEDBACK.STUDENT.SEARCH.failureMsg.401"),
            404: translateService.getTranslation("TOASTFEEDBACK.STUDENT.SEARCH.failureMsg.404"),
            500: translateService.getTranslation("TOASTFEEDBACK.STUDENT.SEARCH.failureMsg.500")
        };

        var settings = {
            data: params,
            topUrl: myConfig.search,
            responseMsg: "",
            failureMsg: {
                401: "",
                404: "",
                500: ""
            }
        };

        return $q.all(localizationPromises)
            .then(function (translations) {
                settings.responseMsg = translations.responseMsg;
                settings.failureMsg[401] = translations[401];
                settings.failureMsg[404] = translations[404];
                settings.failureMsg[500] = translations[500];
                return toastedHttp.post(settings);
            });
    }

    function validateArchived(id, valid){
        var localizationPromises;

        var json = {
            id: id,
            valid: valid
        };

        var settings = {
            data: json,
            topUrl: myConfig.validateArchived,
            responseMsg: "",
            failureMsg: {
                401: "",
                404: "",
                500: ""
            }
        };

        if (valid)
            localizationPromises = {
                responseMsg: translateService.getTranslation("TOASTFEEDBACK.STUDENT.VALIDATEARCHIVED.responseMsg.valid"),
                401: translateService.getTranslation("TOASTFEEDBACK.STUDENT.VALIDATEARCHIVED.failureMsg.401"),
                404: translateService.getTranslation("TOASTFEEDBACK.STUDENT.VALIDATEARCHIVED.failureMsg.404"),
                500: translateService.getTranslation("TOASTFEEDBACK.STUDENT.VALIDATEARCHIVED.failureMsg.500")
            };
        else
            localizationPromises = {
                responseMsg: translateService.getTranslation("TOASTFEEDBACK.STUDENT.VALIDATEARCHIVED.responseMsg.invalid"),
                401: translateService.getTranslation("TOASTFEEDBACK.STUDENT.VALIDATEARCHIVED.failureMsg.401"),
                404: translateService.getTranslation("TOASTFEEDBACK.STUDENT.VALIDATEARCHIVED.failureMsg.404"),
                500: translateService.getTranslation("TOASTFEEDBACK.STUDENT.VALIDATEARCHIVED.failureMsg.500")
            };

        return $q.all(localizationPromises)
            .then(function (translations) {
                settings.responseMsg = translations.responseMsg;
                settings.failureMsg[401] = translations[401];
                settings.failureMsg[404] = translations[404];
                settings.failureMsg[500] = translations[500];
                return toastedHttp.post(settings);
            });
    }

    function submitComment(id, comment){
        var localizationPromises = {
            responseMsg: translateService.getTranslation("TOASTFEEDBACK.STUDENT.SUBMITCOMMENT.responseMsg"),
            401: translateService.getTranslation("TOASTFEEDBACK.STUDENT.SUBMITCOMMENT.failureMsg.401"),
            404: translateService.getTranslation("TOASTFEEDBACK.STUDENT.SUBMITCOMMENT.failureMsg.404"),
            500: translateService.getTranslation("TOASTFEEDBACK.STUDENT.SUBMITCOMMENT.failureMsg.500")
        };

        var json = { id: id,
            comment: comment};

        var settings = {
            data: json,
            topUrl: myConfig.submitComment,
            responseMsg: "",
            failureMsg: {
                401: "",
                404: "",
                500: ""
            }
        };

        return $q.all(localizationPromises)
            .then(function (translations) {
                settings.responseMsg = translations.responseMsg;
                settings.failureMsg[401] = translations[401];
                settings.failureMsg[404] = translations[404];
                settings.failureMsg[500] = translations[500];
                return toastedHttp.post(settings);
            });
    }
}
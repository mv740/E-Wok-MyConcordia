'use strict';

angular
    .module('myApp')
    .factory('adminService', adminService);

adminService.$inject = ['$q', 'translateService', 'toastedHttpService', 'myConfig'];

function adminService($q, translateService, toastedHttp, myConfig) {

    var service = {
        submitUpdatePeriod: submitUpdatePeriod,
        getUpdatePeriod: getUpdatePeriod
    };

    return service;

    //////////////////////////////////////

    function submitUpdatePeriod(updatePeriod){
        var localizationPromises = {
            responseMsg: translateService.getTranslation("TOASTFEEDBACK.ADMIN.SUBMITUPDATEPERIOD.responseMsg"),
            401: translateService.getTranslation("TOASTFEEDBACK.ADMIN.SUBMITUPDATEPERIOD.failureMsg.401"),
            404: translateService.getTranslation("TOASTFEEDBACK.ADMIN.SUBMITUPDATEPERIOD.failureMsg.404"),
            500: translateService.getTranslation("TOASTFEEDBACK.ADMIN.SUBMITUPDATEPERIOD.failureMsg.500")
        };

        var settings = {
            data: updatePeriod,
            topUrl: myConfig.picturePeriod,
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

    function getUpdatePeriod() {
        var localizationPromises = {
            401: translateService.getTranslation("TOASTFEEDBACK.ADMIN.GETUPDATEPERIOD.failureMsg.401"),
            404: translateService.getTranslation("TOASTFEEDBACK.ADMIN.GETUPDATEPERIOD.failureMsg.404"),
            500: translateService.getTranslation("TOASTFEEDBACK.ADMIN.GETUPDATEPERIOD.failureMsg.500")
        };

        var settings = {
            topUrl: myConfig.getUpdatePeriod,
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
}
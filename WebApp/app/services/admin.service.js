'use strict';

angular
    .module('myApp')
    .factory('adminService', adminService);

adminService.$inject = ['$q', '$translate','toastedHttpService', 'myConfig', 'adminToastFeedback'];

function adminService($q, $translate,toastedHttp, myConfig, adminToastFeedback) {

    var service = {
        submitUpdatePeriod: submitUpdatePeriod,
        getUpdatePeriod: getUpdatePeriod
    }

    return service;

    //////////////////////////////////////

    function submitUpdatePeriod(updatePeriod){
        var localizationPromises = {
            responseMsg: getTranslation("TOASTFEEDBACK.ADMIN.SUBMITUPDATEPERIOD.responseMsg"),
            401: getTranslation("TOASTFEEDBACK.ADMIN.SUBMITUPDATEPERIOD.failureMsg.401"),
            404: getTranslation("TOASTFEEDBACK.ADMIN.SUBMITUPDATEPERIOD.failureMsg.404"),
            500: getTranslation("TOASTFEEDBACK.ADMIN.SUBMITUPDATEPERIOD.failureMsg.500")
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
                settings.responseMsg = translations.response;
                settings.failureMsg[401] = translations[401];
                settings.failureMsg[404] = translations[404];
                settings.failureMsg[500] = translations[500];
                return toastedHttp.post(settings);
            });
    }

    function getUpdatePeriod() {
        var localizationPromises = {
            401: getTranslation("TOASTFEEDBACK.ADMIN.GETUPDATEPERIOD.failureMsg.401"),
            404: getTranslation("TOASTFEEDBACK.ADMIN.GETUPDATEPERIOD.failureMsg.404"),
            500: getTranslation("TOASTFEEDBACK.ADMIN.GETUPDATEPERIOD.failureMsg.500")
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

    function getTranslation(path) {
        var deferred = $q.defer();

        $translate(path)
            .then(function(translation) {
                deferred.resolve(translation);
            });

        return deferred.promise;
    }
}
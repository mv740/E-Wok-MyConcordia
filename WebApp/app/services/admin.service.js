'use strict';

angular
    .module('myApp')
    .factory('adminService', adminService);

adminService.$inject = ['toastedHttpService', 'myConfig', 'adminToastFeedback'];

function adminService(toastedHttp, myConfig, adminToastFeedback) {

    var service = {
        submitUpdatePeriod: submitUpdatePeriod,
        getUpdatePeriod: getUpdatePeriod
    }

    return service;

    //////////////////////////////////////

    function submitUpdatePeriod(updatePeriod){
        var settings = {
            data: updatePeriod,
            topUrl: myConfig.picturePeriod,
            responseMsg: adminToastFeedback.submitUpdatePeriod.responseMsg,
            failureMsg: adminToastFeedback.submitUpdatePeriod.failureMsg
        };
        return toastedHttp.post(settings);
    }

    function getUpdatePeriod() {
        var settings = {
            topUrl: myConfig.getUpdatePeriod,
            failureMsg: adminToastFeedback.getUpdatePeriod.failureMsg
        };
        return toastedHttp.get(settings);
    }

}
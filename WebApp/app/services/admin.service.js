'use strict';

angular
    .module('myApp')
    .factory('adminService', adminService);

adminService.$inject = ['toastedHttpService', 'myConfig'];

function adminService(toastedHttp, myConfig) {

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
            responseMsg: "Period updated",
            failureMsg: {
                401: "Please Login",
                404: "Update Period not found",
                500: "Sorry! Our servers are down :("
            }
        };
        return toastedHttp.post(settings);
    }

    function getUpdatePeriod() {
        var settings = {
            topUrl: myConfig.getUpdatePeriod,
            failureMsg: {
                401: "Please Login",
                404: "Update Period not found",
                500: "Sorry! Our servers are down :("
            }
        };
        return toastedHttp.get(settings);
    }

}
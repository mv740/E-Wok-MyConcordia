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
        return toastedHttp.post(updatePeriod, myConfig.picturePeriod);
    }

    function getUpdatePeriod() {
        return toastedHttp.get({topUrl: myConfig.getUpdatePeriod});
    }

}
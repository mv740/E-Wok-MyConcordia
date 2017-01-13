'use strict';

angular
    .module('myApp')
    .factory('studentService', studentService);

studentService.$inject = ['$http', '$q', 'myConfig'];

function studentService($http, $q, myConfig) {

    var service = {
        sendValidation: sendValidation,
        getStudentPictures: getStudentPictures,
        getStudentLogs: getStudentLogs,
        getStudents: getStudents,
        search: search,
        getUpdatePeriod: getUpdatePeriod,
        validateArchived: validateArchived
    };

    return service;
    /////////////////////

    function sendValidation(id, valid) {
        var deferred = $q.defer();

        var json = {
            id: parseInt(id),
            valid: valid
        };

        $http({
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: 'POST',
            url: myConfig.baseUrl + myConfig.validatePhoto,
            data: json
        }).then(
        function (success) {
            deferred.resolve(success);
        },
        function (failure) {
            console.log('validate failure');
        });

        return deferred.promise;
    }

    function getStudentPictures(id) {
        var deferred = $q.defer();

        $http.get(myConfig.baseUrl + myConfig.getStudentPictures + id).then(function (value) {
            deferred.resolve(value);
        },function (failure) {
            console.log('getStudentPicture failure');
        });

        return deferred.promise;
    }

    function getStudentLogs(netname) {
        var deferred = $q.defer();

        $http.get(myConfig.baseUrl + myConfig.getLogs + netname).then(function (value) {
            deferred.resolve(value);
        },function (failure) {
            console.log('getStudentLogs failure');
        });

        return deferred.promise;
    }

    function getStudents() {
        var deferred = $q.defer();

        $http.get(myConfig.baseUrl + myConfig.getStudents).then(function (value) {
            deferred.resolve(value);
        },function (failure) {
            console.log('getStudents failure');
        });

        return deferred.promise;
    }

    function search(params) {
        var deferred = $q.defer();

        $http({
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: 'POST',
            url: myConfig.baseUrl + myConfig.search,
            data: params
        }).then(
        function (value) {
            deferred.resolve(value);
        },
        function (failure) {
            console.log('search failure');
            deferred.resolve(failure);
        });

        return deferred.promise;
    }

    function getUpdatePeriod() {
        var deferred = $q.defer();

        $http.get(myConfig.baseUrl + myConfig.getUpdatePeriod).then(function (value) {
            deferred.resolve(value);
        },function (failure) {
            console.log('updating period failure');
        });

        return deferred.promise;
    }

    function validateArchived(id, valid){

        var deferred = $q.defer();

        var req = {
            method: 'POST',
            url: myConfig.baseUrl + myConfig.validateArchived,
            headers: {
                'Content-Type': "JSON"
            },
            data: { id: id,
            valid: valid}
        }

        $http(req).then(function (value) {
            deferred.resolve(value);
        }, function(){
            console.log("sendPictureBackToValidation failure");
        });

        return deferred.promise;

    }

}
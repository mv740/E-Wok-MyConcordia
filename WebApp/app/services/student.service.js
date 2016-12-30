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
        getUpdatePeriod: getUpdatePeriod
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
        });

        return deferred.promise;
    }

    function getStudentLogs(netname) {
        var deferred = $q.defer();

        $http.get(myConfig.baseUrl + myConfig.getLogs + netname).then(function (value) {
            deferred.resolve(value);
        });

        return deferred.promise;
    }

    function getStudents() {
        var deferred = $q.defer();

        $http.get(myConfig.baseUrl + myConfig.getStudents).then(function (value) {
            deferred.resolve(value);
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
            console.log('validate success');
            deferred.resolve(value);
        },
        function (failure) {
            console.log('validate failure');
        });

        return deferred.promise;
    }

    function getUpdatePeriod() {
        var deferred = $q.defer();

        $http.get(myConfig.baseUrl + myConfig.getUpdatePeriod).then(function (value) {
            deferred.resolve(value);
        });

        return deferred.promise;
    }

}
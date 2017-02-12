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
        getUpdatePeriod: getUpdatePeriod,
        validateArchived: validateArchived,
        submitComment: submitComment
    };

    return service;
    /////////////////////

    function sendValidation(id, valid) {
        var deferred = $q.defer();

        var json = {
            id: parseInt(id),
            valid: valid
        };

        toastedHttp.post(json, myConfig.validatePhoto).then(function(success) {
            deferred.resolve(success);
        },
            function(failure){
                console.log('validate failure');
            }
        );

        return deferred.promise;
    }

    function getStudentPictures(id) {
        var deferred = $q.defer();

        toastedHttp.get({param: id, topUrl: myConfig.getStudentPictures}).then(function (value) {
            deferred.resolve(value);
        },function (failure) {
            console.log('getStudentPicture failure');
        });

        return deferred.promise;
    }

    function getStudentLogs(netname) {
        var deferred = $q.defer();

        toastedHttp.get({param: netname, topUrl: myConfig.getLogs}).then(function (value) {
            deferred.resolve(value);
        },function (failure) {
            console.log('getStudentLogs failure');
        });

        return deferred.promise;
    }

    function getStudents() {
        var deferred = $q.defer();

        toastedHttp.get({topUrl: myConfig.getStudents}).then(function (value) {
            deferred.resolve(value);
        },function (failure) {
            console.log('getStudents failure');
        });

        return deferred.promise;
    }

    function search(params) {
        var deferred = $q.defer();

        toastedHttp.post(params, myConfig.search).then(
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

        toastedHttp.get({topUrl: myConfig.getUpdatePeriod}).then(function (value) {
            deferred.resolve(value);
        },function (failure) {
            console.log('updating period failure');
        });

        return deferred.promise;
    }

    function validateArchived(id, valid){

        var deferred = $q.defer();

        var json = { id: id,
            valid: valid};

        toastedHttp.post(json, myConfig.validateArchived).then(function (value) {
            deferred.resolve(value);
        }, function(){
            console.log("sendPictureBackToValidation failure");
        });

        return deferred.promise;

    }

    function submitComment(id, comment){

        var deferred = $q.defer();

        var json = { id: id,
            comment: comment};

        toastedHttp.post(json, myConfig.submitComment).then(function (value) {
            deferred.resolve(value);
        }, function(){
            console.log("submitComment failure");
        });

        return deferred.promise;

    }

}
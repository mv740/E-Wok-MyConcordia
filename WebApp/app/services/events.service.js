'use strict';

angular
    .module('myApp')
    .factory('eventService', eventService);

function eventService() {

    var service = {
        getEvent: getEvent,
        getAllEvents: getAllEvents,
        submit: submit
    }

    return service;

    function getEvent(id) {
        var deferred = $q.defer();

        $http.get(myConfig.baseUrl + myConfig.getEvent + id).then(function (value) {
            deferred.resolve(value);
        },function (failure) {
            console.log('getEvent failure');
        });

        return deferred.promise;
    }

    function submit(event){
        var deferred = $q.defer();

        $http({
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: 'POST',
            url: myConfig.baseUrl + myConfig.submitEvent,
            data: event
        }).then(
            function (value) {
                deferred.resolve(value);
            },
            function (failure) {
                console.log('submitEvent failure');
                deferred.resolve(failure);
            });

        return deferred.promise;
    }

    function getAllEvents() {
        var deferred = $q.defer();

        $http.get(myConfig.baseUrl + myConfig.getEvent).then(function (value) {
            deferred.resolve(value);
        },function (failure) {
            console.log('getAllEvents failure');
        });

        return deferred.promise;
    }
}
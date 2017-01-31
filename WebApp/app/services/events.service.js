'use strict';

angular
    .module('myApp')
    .factory('eventService', eventService);

function eventService() {

    var service = {
        getEvent: getEvent,
        getAllEvents: getAllEvents
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
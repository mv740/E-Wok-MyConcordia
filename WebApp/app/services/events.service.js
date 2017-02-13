'use strict';

angular
    .module('myApp')
    .factory('eventService', eventService);

eventService.$inject = [ '$q', 'toastedHttpService', 'myConfig'];

function eventService($q, toastedHttp, myConfig) {

    var service = {
        getThisEvent: getThisEvent,
        getAllEvents: getAllEvents,
        submit: submit,
        updateEvent: updateEvent,
        deleteEvent: deleteEvent,
        setUserRole: setUserRole
    }

    return service;

    function getThisEvent(id) {
        var deferred = $q.defer();

        toastedHttp.get({param:id, topUrl: myConfig.getEvent}).then(function (value) {
            deferred.resolve(value);
        },function (failure) {
            console.log('getThisEvent failure');
        });

        return deferred.promise;
    }

    function submit(event){
        var deferred = $q.defer();

        toastedHttp.post(event, myConfig.event).then(
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

       toastedHttp.get({topUrl: myConfig.event}).then(function (value) {
            deferred.resolve(value);
        },function (failure) {
            console.log('getAllEvents failure');
        });

        return deferred.promise;
    }

    function updateEvent(event) {
        var deferred = $q.defer();

        toastedHttp.put(event, myConfig.event).then(
            function (value) {
                deferred.resolve(value);
            },
            function (failure) {
                console.log('updateEvent failure');
                deferred.resolve(failure);
            });

        return deferred.promise;
    }

    function deleteEvent(event) {
        var deferred = $q.defer();

        toastedHttp.del(event, myConfig.event).then(
            function (value) {
                deferred.resolve(value);
            },
            function (failure) {
                console.log('deleteEvent failure');
                deferred.resolve(failure);
            });

        return deferred.promise;
    }

    function setUserRole(role) {
        var deferred = $q.defer();

        toastedHttp.put(role, myConfig.eventUser).then(
            function (value) {
                deferred.resolve(value);
            },
            function (failure) {
                console.log('setUserRole failure');
                deferred.resolve(failure);
            });

        return deferred.promise;
    }
}
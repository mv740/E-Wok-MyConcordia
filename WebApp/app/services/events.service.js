'use strict';

angular
    .module('myApp')
    .factory('eventService', eventService);

function eventService() {

    var service = {
        getThisEvent: getThisEvent,
        getAllEvents: getAllEvents,
        submit: submit,
        updateEvent: updateEvent,
        deleteEvent: deleteEvent
    }

    return service;

    function getThisEvent(id) {
        var deferred = $q.defer();

        $http.get(myConfig.baseUrl + myConfig.getEvent + id).then(function (value) {
            deferred.resolve(value);
        },function (failure) {
            console.log('getThisEvent failure');
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
            url: myConfig.baseUrl + myConfig.event,
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

        $http.get(myConfig.baseUrl + myConfig.event).then(function (value) {
            deferred.resolve(value);
        },function (failure) {
            console.log('getAllEvents failure');
        });

        return deferred.promise;
    }

    function updateEvent(event) {
        var deferred = $q.defer();

        $http({
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: 'PUT',
            url: myConfig.baseUrl + myConfig.event,
            data: event
        }).then(
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

        $http({
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            method: 'DELETE',
            url: myConfig.baseUrl + myConfig.event,
            data: event
        }).then(
            function (value) {
                deferred.resolve(value);
            },
            function (failure) {
                console.log('deleteEvent failure');
                deferred.resolve(failure);
            });

        return deferred.promise;
    }
}
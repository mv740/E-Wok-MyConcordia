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
        setUserRole: setUserRole,
        getEventAttendees: getEventAttendees
    }

    return service;

    //////////////////////////////////////

    function getThisEvent(id) {
        return toastedHttp.get({param:id, topUrl: myConfig.getEvent});
    }

    function submit(event){
        return toastedHttp.post(event, myConfig.event);
    }

    function getAllEvents() {
       return toastedHttp.get({topUrl: myConfig.event});
    }

    function updateEvent(event) {
        return toastedHttp.put(event, myConfig.event);
    }

    function deleteEvent(event) {
        return toastedHttp.del(event, myConfig.event);
    }

    function setUserRole(role) {
        return toastedHttp.put(role, myConfig.eventUser);
    }

    function getEventAttendees(id) {
        return toastedHttp.get({topUrl: myConfig.eventAttendees.replace("IDTOKEN", id)});
    }

    function addUserToEvent() {

    }
}
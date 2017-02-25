'use strict';

angular
    .module('myApp')
    .factory('eventService', eventService);

eventService.$inject = ['toastedHttpService', 'myConfig'];

function eventService(toastedHttp, myConfig) {

    var service = {
        getThisEvent: getThisEvent,
        getAllEvents: getAllEvents,
        submit: submit,
        updateEvent: updateEvent,
        setUserRole: setUserRole,
        getEventAttendees: getEventAttendees,
        addUser: addUser
        cancelEvent: cancelEvent,
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
       return toastedHttp.get({topUrl: myConfig.getEvents});
    }

    function updateEvent(event) {
        return toastedHttp.put(event, myConfig.event);
    }

    function cancelEvent(event) {
        return toastedHttp.del(event, myConfig.event);
    }

    function setUserRole(user) {
        return toastedHttp.put(user, myConfig.eventUser);
    }

    function getEventAttendees(id) {
        return toastedHttp.get({topUrl: myConfig.eventAttendees.replace("IDTOKEN", id) + "/true"});
    }

    function addUser(user) {
        return toastedHttp.post(user, myConfig.eventUser);
    }
}
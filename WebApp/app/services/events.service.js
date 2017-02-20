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
        cancelEvent: cancelEvent,
        setUserRole: setUserRole
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

    function setUserRole(role) {
        return toastedHttp.put(role, myConfig.setEventUserRole);
    }
}
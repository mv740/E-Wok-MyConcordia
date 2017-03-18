'use strict';

angular
    .module('myApp')
    .factory('eventService', eventService);


eventService.$inject = ['toastedHttpService', 'myConfig', 'eventToastFeedback'];

function eventService(toastedHttp, myConfig, eventToastFeedback) {

    var service = {
        getThisEvent: getThisEvent,
        getAllEvents: getAllEvents,
        submit: submit,
        updateEvent: updateEvent,
        setUserRole: setUserRole,
        getEventAttendees: getEventAttendees,
        addUser: addUser,
        cancelEvent: cancelEvent,
        deleteUser: deleteUser,
        getStats: getStats
    }

    return service;

    //////////////////////////////////////

    function getThisEvent(id) {
        var settings = {
            param: id,
            topUrl: myConfig.getEvent,
            failureMsg: eventToastFeedback.getThisEvent.failureMsg
        };
        return toastedHttp.get(settings);
    }

    function submit(event){
        var settings = {
            data: event,
            topUrl: myConfig.event,
            responseMsg: eventToastFeedback.submit.responseMsg,
            failureMsg: eventToastFeedback.submit.failureMsg
        };
        return toastedHttp.post(settings);
    }

    function getAllEvents() {
        var settings = {
            topUrl: myConfig.getEvents,
            failureMsg: eventToastFeedback.getAllEvents.failureMsg
        };
       return toastedHttp.get(settings);
    }

    function updateEvent(event) {
        var settings = {
            data: event,
            topUrl: myConfig.event,
            responseMsg: eventToastFeedback.updateEvent.responseMsg,
            failureMsg: eventToastFeedback.updateEvent.failureMsg
        };
        return toastedHttp.put(settings);
    }

    function cancelEvent(event) {
        var settings = {
            data: event,
            topUrl: myConfig.event,
            responseMsg: eventToastFeedback.cancelEvent.responseMsg,
            failureMsg: eventToastFeedback.cancelEvent.failureMsg
        };
        return toastedHttp.del(settings);
    }

    function setUserRole(user) {
        var settings = {
            data: user,
            topUrl: myConfig.eventUser,
            responseMsg: eventToastFeedback.setUserRole.responseMsg,
            failureMsg: eventToastFeedback.setUserRole.failureMsg
        };
        return toastedHttp.put(settings);
    }

    function getEventAttendees(id) {
        var settings = {
            topUrl: myConfig.eventAttendees.replace("IDTOKEN", id) + "/true",
            failureMsg: eventToastFeedback.getEventAttendees.failureMsg
        };
        return toastedHttp.get(settings);
    }

    function addUser(user) {
        var settings = {
            data: user,
            topUrl: myConfig.eventUser,
            responseMsg: eventToastFeedback.addUser.responseMsg,
            failureMsg: eventToastFeedback.addUser.failureMsg
        };
        return toastedHttp.post(settings);
    }

    function deleteUser(user) {
        var settings = {
            data: user,
            topUrl: myConfig.eventUser,
            responseMsg: eventToastFeedback.deleteUser.responseMsg,
            failureMsg: eventToastFeedback.deleteUser.failureMsg
        };
        return toastedHttp.del(settings);
    }

    function getStats(eventId){
        return toastedHttp.get({topUrl: myConfig.getEventStats.replace("IDTOKEN", eventId)});
    }
}
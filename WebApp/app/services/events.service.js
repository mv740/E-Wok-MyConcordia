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
        addUser: addUser,
        cancelEvent: cancelEvent,
        deleteUser: deleteUser
    }

    return service;

    //////////////////////////////////////

    function getThisEvent(id) {
        var settings = {
            param: id,
            topUrl: myConfig.getEvent,
            failureMsg: {
                401: "Please Login",
                404: "Event not found",
                500: "Sorry! Our servers are down :("
            }
        };
        return toastedHttp.get(settings);
    }

    function submit(event){
        var settings = {
            data: event,
            topUrl: myConfig.event,
            responseMsg: "Event created",
            failureMsg: {
                401: "Please Login",
                500: "Sorry! Our servers are down :("
            }
        };
        return toastedHttp.post(settings);
    }

    function getAllEvents() {
        var settings = {
            topUrl: myConfig.getEvents,
            failureMsg: {
                401: "Please Login",
                500: "Sorry! Our servers are down :("
            }
        };
       return toastedHttp.get(settings);
    }

    function updateEvent(event) {
        var settings = {
            data: event,
            topUrl: myConfig.event,
            responseMsg: "Event updated",
            failureMsg: {
                401: "Please Login",
                404: "Event not found",
                500: "Sorry! Our servers are down :("
            }
        };
        return toastedHttp.put(settings);
    }

    function cancelEvent(event) {
        var settings = {
            data: event,
            topUrl: myConfig.event,
            responseMsg: "Event cancelled",
            failureMsg: {
                401: "Please Login",
                404: "Event not found",
                500: "Sorry! Our servers are down :("
            }
        };
        return toastedHttp.del(settings);
    }

    function setUserRole(user) {
        var settings = {
            data: user,
            topUrl: myConfig.eventUser,
            responseMsg: "Role updated",
            failureMsg: {
                401: "Please Login",
                404: "User not found",
                500: "Sorry! Our servers are down :("
            }
        };
        return toastedHttp.put(settings);
    }

    function getEventAttendees(id) {
        var settings = {
            topUrl: myConfig.eventAttendees.replace("IDTOKEN", id) + "/true",
            failureMsg: {
                401: "Please Login",
                404: "User not found",
                500: "Sorry! Our servers are down :("
            }
        };
        return toastedHttp.get(settings);
    }

    function addUser(user) {
        var settings = {
            data: user,
            topUrl: myConfig.eventUser,
            responseMsg: "Attendee added",
            failureMsg: {
                401: "Please Login",
                404: "User not found",
                409: "Attendee already added",
                500: "Sorry! Our servers are down :("
            }
        };
        return toastedHttp.post(settings);
    }

    function deleteUser(user) {
        var settings = {
            data: user,
            topUrl: myConfig.eventUser,
            responseMsg: "Attendee removed",
            failureMsg: {
                401: "Please Login",
                404: "User not found",
                500: "Sorry! Our servers are down :("
            }
        };
        return toastedHttp.del(settings);
    }
}
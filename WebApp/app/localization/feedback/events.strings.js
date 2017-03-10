/**
 * Created by Harrison on 2017-03-09.
 */

angular
    .module('myApp')
    .constant("eventToastFeedback", {
        "getThisEvent": {
            failureMsg: {
                401: "Please Login",
                404: "Event not found",
                500: "Sorry! Our servers are down :("
            }
        },
        "submit": {
            responseMsg: "Event created",
            failureMsg: {
                401: "Please Login",
                500: "Sorry! Our servers are down :("
            }
        },
        getAllEvents: {
            failureMsg: {
                401: "Please Login",
                500: "Sorry! Our servers are down :("
            }
        },
        "updateEvent" : {
            responseMsg: "Event updated",
            failureMsg: {
                401: "Please Login",
                404: "Event not found",
                500: "Sorry! Our servers are down :("
            }
        },
        "cancelEvent": {
            responseMsg: "Event cancelled",
            failureMsg: {
                401: "Please Login",
                404: "Event not found",
                500: "Sorry! Our servers are down :("
            }
        },
        "setUserRole": {
            responseMsg: "Role updated",
            failureMsg: {
                401: "Please Login",
                404: "User not found",
                500: "Sorry! Our servers are down :("
            }
        },
        "getEventAttendees": {
            failureMsg: {
                401: "Please Login",
                404: "User not found",
                500: "Sorry! Our servers are down :("
            }
        },
        "addUser": {
            responseMsg: "Attendee added",
            failureMsg: {
                401: "Please Login",
                404: "User not found",
                409: "Attendee already added",
                500: "Sorry! Our servers are down :("
            }
        },
        "deleteUser": {
            responseMsg: "Attendee removed",
            failureMsg: {
                401: "Please Login",
                404: "User not found",
                500: "Sorry! Our servers are down :("
            }
        }
    });

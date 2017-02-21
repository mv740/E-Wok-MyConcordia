/**
 * Created by franc on 1/30/2017.
 */

'use strict';

angular
    .module('myApp')
    .controller('EventController', EventController);

EventController.$inject = ['$modal', '$timeout', 'eventService'];

function EventController($modal, $timeout, eventService) {



    var event = this;
    event.submit = submit;
    event.modify = modify;
    event.openEventModal = openEventModal;
    event.create = create;
    event.checkAttendees = checkAttendees;
    event.openAttendeeModal = openAttendeeModal;

    getEvents();

    event.fpOptions = {
        navigation: false,
        keyboardScrolling: false
    };

    ////////////////////////////////////////////////////////////


    function submit(){
        if (event.creating.eventID) eventService.updateEvent(event.creating).then(function(){});
        else eventService.submit(event.creating).then(function(){});
    }

    function get(eventId) {
        eventService.getThisEvent(eventId).then(function(result) {
                event.selectedEvent = result;
        });

    }

    function getEvents() {
        eventService.getAllEvents().then(function(result) {
            event.events = result;
        });
    }

    function create(){
        event.creating = {};
    }

    function modify(eventTarget) {
        event.creating = eventTarget;
    }

    function openEventModal(eventTarget){
        $modal.open({templateUrl: "partials/event/eventModal/eventModal.html",
            controller: 'EventModalCtrl as eventModal',
            windowClass: 'app-modal-window',
            keyboard: true,
            resolve: {
                event: function () {
                    return eventTarget;
                }
            }});
    }

    function checkAttendees(eventTarget) {
        eventService.getEventAttendees(eventTarget.eventId).then(function (result) {
            event.attendees = result;
        });
    }

    function openAttendeeModal(attendeeTarget) {
        $modal.open({templateUrl: "partials/event/attendeeModal/attendeeModal.html",
            controller: 'AttendeeModalCtrl as attendeeModal',
            windowClass: 'app-modal-window',
            keyboard: true,
            resolve: {
                attendee: function () {
                    return attendeeTarget;
                }
            }})
    }
};
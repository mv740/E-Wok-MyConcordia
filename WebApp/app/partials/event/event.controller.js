/**
 * Created by franc on 1/30/2017.
 */

'use strict';

angular
    .module('myApp')
    .controller('EventController', EventController);

EventController.$inject = ['$modal', '$timeout', '$mdDialog', 'eventService'];

function EventController($modal, $timeout, $mdDialog, eventService) {



    var event = this;
    event.submit = submit;
    event.modify = modify;
    event.openEventModal = openEventModal;
    event.create = create;
    event.checkAttendees = checkAttendees;
    event.openAttendeeDialog = openAttendeeDialog;

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

    function openAttendeeDialog(attendeeTarget) {
        $mdDialog.show({
            controller: 'AttendeeModalCtrl as attendeeModal',
            templateUrl: "partials/event/attendeeModal/attendeeModal.html",
            parent: angular.element(document.body),
            targetEvent: attendeeTarget,
            clickOutsideToClose:true,
            locals: { attendee: attendeeTarget }
        })
            .then(function(answer) {
                eventService.setUserRole(answer).then(
                    function(response) {
                        console.log("attendee role modified successfully");
                    },
                    function (failure) {
                        console.log("failed to modify attendee role");
                    });
            }, function() {
                console.log("no role was set for this attendee");
            });
    }
};
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

    //TEMPORARY BEGIN
    var tmpEvent = {
        id: "1234",
        name: "coolEvent",
        room: "centreBell",
        startDateStamp: "1997-07-16T19:20:30.45+01:00",
        endDateStamp: "1997-07-16T19:20:30.45+01:00",
        description: "omg so nice event",
        location: "at ma' place"
    };

    var tmpEvent2 = {
        id: "134",
        name: "colEvent",
        room: "cenreBell",
        startDateStamp: "1597-07-16T19:20:30.45+01:00",
        endDateStamp: "1297-07-16T19:20:30.45+01:00",
        description: "og so nice event",
        location: "a ma' place"
    }
    event.events = [tmpEvent,tmpEvent2];
    //TEMPORARY END

    event.fpOptions = {
        navigation: false,
        keyboardScrolling: false
    };

    ////////////////////////////////////////////////////////////


    function submit(){
        eventService.submit(event.creating).then(function(){

        });
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
        moveSectionDown();
    }

    function modify(eventTarget) {
        event.creating = eventTarget;
        moveSectionDown();
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
};
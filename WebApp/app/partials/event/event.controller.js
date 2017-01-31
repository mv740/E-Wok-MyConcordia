/**
 * Created by franc on 1/30/2017.
 */

'use strict';

angular
    .module('myApp')
    .controller('EventController', EventController);

EventController.$inject = ['eventService'];

function EventController(eventService) {
    var event = this;

    event.creating = {};
    event.moveSectionUp = moveSectionUp;
    event.moveSectionDown = moveSectionDown;
    event.submit = submit;


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
    event.events = [tmpEvent,tmpEvent,tmpEvent,tmpEvent];
    //TEMPORARY END

    event.fpOptions = {
        navigation: false,
        keyboardScrolling: false
    };
    //having a timeout allows to execute after digest
    setTimeout(function(){
        $.fn.fullpage.setMouseWheelScrolling(false);
        $.fn.fullpage.setAllowScrolling(false);
    })



    ////////////////////////////////////////////////////////////

    function moveSectionUp(){
        $.fn.fullpage.moveSectionUp();
    }

    function moveSectionDown(){
        $.fn.fullpage.moveSectionDown();
    }

    function submit(){
        eventService.submit(event.creating);
    }

    function getEvent(eventId) {
        eventService.getThisEvent(eventId).then(function(result) {
                event.selectedEvent = result;
        });

    }

    function getEvents() {
        eventService.getAllEvents().then(function(result) {
            event.events = result;
        });
    }
};
/**
 * Created by franc on 1/30/2017.
 */

'use strict';

angular
    .module('myApp')
    .controller('EventController', EventController);

EventController.$inject = [];

function EventController() {
    var event = this;

    event.moveSectionUp = moveSectionUp;
    event.moveSectionDown = moveSectionDown;

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

    event.fpOptions = {
        navigation: false,
        keyboardScrolling: false
    };
    //having a timeout allows to execute after digest
    setTimeout(function(){
        $.fn.fullpage.setMouseWheelScrolling(false);
        $.fn.fullpage.setAllowScrolling(false);
    })

    function moveSectionUp(){
        $.fn.fullpage.moveSectionUp();
    }

    function moveSectionDown(){
        $.fn.fullpage.moveSectionDown();
    }

};
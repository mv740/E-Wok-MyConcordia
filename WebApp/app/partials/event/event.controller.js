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
    event.cancel = cancel;
    event.setFilter = setFilter;
    event.isFilterTarget = isFilterTarget;
    event.filters = [
        "All",
        "Cancelled",
        "Postponed",
        "Rescheduled",
        "Scheduled",
        "Open",
        "Closed"
    ];


    event.readonly = true;
    event.removable = false;

    getEvents();

    event.fpOptions = {
        navigation: false,
        keyboardScrolling: false
    };

    ////////////////////////////////////////////////////////////


    function submit(){
        if (event.creating.eventID) eventService.updateEvent(event.creating).then(function(){});
        else eventService.submit(event.creating).then(function(){
            event.fpControls.moveTo(1);
            getEvents();
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
    }

    function modify(eventTarget) {
        event.creating = eventTarget.information;
    }

    function cancel(eventTarget){
        eventService.cancelEvent(eventTarget.information).then(function(){
            getEvents();
        });
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

    function setFilter(filter){
        event.currentFilter = filter;
    }

    function isFilterTarget(result){
        return !event.currentFilter || (event.currentFilter == "All" || result.information.status == event.currentFilter || result.information.type == event.currentFilter);
    }
};